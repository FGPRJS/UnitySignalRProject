using System.Collections.Generic;
using Cinemachine;
using GameObjects.Characters;
using GameObjects.Locations;
using Protocol;
using Protocol.MessageBody;
using UnityEngine;
using Utilities;

namespace GameObjects.Managers
{
    public class MainSceneManager : MonoBehaviour
    {
        [Header("[Spawn]")]
        public List<SpawnZone> spawnZones;
        public List<string> spawnCharacterPaths;

        [Header("[Camera]")] 
        public CinemachineVirtualCamera playerCamera;
        
        [Header("[Control]")] 
        private PlayerInputActions _playerInputActions;

        public Character playerCharacter;
        public Dictionary<string, Character> otherPlayercharacter;
        
        private void Awake()
        {
            this._playerInputActions = new PlayerInputActions();

            otherPlayercharacter = new Dictionary<string, Character>();
        }
        
        private void OnEnable()
        {
            this._playerInputActions.Character.Enable();
            ConnectionManager.instance.signalR.On<CharacterMovement>(
                MessageNameKey.CharacterMovement, ApplyCharacterMovement);
        }

        private void OnDisable()
        {
            this._playerInputActions.Character.Disable();
        }

        private void ApplyCharacterMovement(CharacterMovement characterMovement)
        {
            var movement = VectorConverter.ToUnityVector2(characterMovement.movement);

            Character targetCharacter = null;
            
            if (characterMovement.userId == GameManager.instance.currentPlayer.userId)
            {
                targetCharacter = playerCharacter;
            }
            else
            {
                otherPlayercharacter.TryGetValue(
                    characterMovement.userId,
                    out targetCharacter);
            }
            
            if (targetCharacter != null)
            {
                targetCharacter.MoveCharacter(movement);
            }
        }

        // Start is called before the first frame update
        void Start()
        {
            var gameManager = GameManager.instance;

            #region Instantiate Players
            
            var spawnZoneTargetIndex = gameManager.currentPlayer.spawnToken % this.spawnZones.Count;
            var spawnZoneTarget = this.spawnZones[spawnZoneTargetIndex];

            var spawnCharacterPathIndex = gameManager.currentPlayer.spawnToken % this.spawnCharacterPaths.Count;
            var spawnCharacterPath = this.spawnCharacterPaths[spawnCharacterPathIndex];
            
            var currentUserPosition = spawnZoneTarget.gameObject.transform.position;
            
            var tankResource = Resources.Load<Character>(spawnCharacterPath);

            this.playerCharacter = Instantiate(tankResource,
                    currentUserPosition,
                    Quaternion.identity);

            playerCamera.Follow = this.playerCharacter.head.transform;
            
            #endregion
        }

        private void FixedUpdate()
        {
            var moveInput = this._playerInputActions.Character.Move.ReadValue<Vector2>();

            ConnectionManager.instance.signalR.Invoke(
                MessageNameKey.CharacterMovement, new CharacterMovement()
            {
                userId = GameManager.instance.currentPlayer.userId,
                movement = $"{moveInput.x},{moveInput.y}",
                rotation = ""
            });
        }
    }
}
