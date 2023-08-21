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
        private Dictionary<string, Character> _loadedCharacters;

        [Header("[Camera]")] 
        public CinemachineVirtualCamera playerCamera;
        
        [Header("[Control]")] 
        private PlayerInputActions _playerInputActions;

        private Character _playerCharacter;
        private Dictionary<string, Character> _otherPlayerCharacter;
        
        private void Awake()
        {
            this._playerInputActions = new PlayerInputActions();

            this._otherPlayerCharacter = new Dictionary<string, Character>();
        }
        
        private void OnEnable()
        {
            this._playerInputActions.Character.Enable();
            ConnectionManager.instance.signalR.On<CharacterMovement>(
                MessageNameKey.CharacterMovement, ApplyCharacterMovement);
            GameManager.instance.newUserConnectedEvent.AddListener(newUser =>
            {
                var otherPlayerPosition = 
                    Utilities.VectorConverter.ToUnityVector3(newUser.positionString);

                var newOtherPlayerCharacter = AddNewCharacter(newUser, otherPlayerPosition);
                
                this._otherPlayerCharacter.Add(newUser.userId, newOtherPlayerCharacter);
            });
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
                targetCharacter = this._playerCharacter;
            }
            else
            {
                this._otherPlayerCharacter.TryGetValue(
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

            #region Initialize Base Data
            
            this._loadedCharacters = new Dictionary<string, Character>();
            foreach (var spawnCharacterPath in spawnCharacterPaths)
            {
                this._loadedCharacters.Add(spawnCharacterPath, Resources.Load<Character>(spawnCharacterPath));
            }
            
            #endregion
            
            #region Instantiate Players

            var spawnZoneTargetIndex = gameManager.currentPlayer.spawnToken % this.spawnZones.Count;
            var spawnZoneTarget = this.spawnZones[spawnZoneTargetIndex];
            
            var currentUserPosition = spawnZoneTarget.gameObject.transform.position;

            this._playerCharacter = AddNewCharacter(gameManager.currentPlayer, currentUserPosition);

            playerCamera.Follow = this._playerCharacter.head.transform;
            
            #endregion
            
            #region Instantiate Others

            foreach (var otherPlayer in GameManager.instance.otherPlayers.Values)
            {
                var otherPlayerPosition = 
                    Utilities.VectorConverter.ToUnityVector3(otherPlayer.positionString);

                var newOtherPlayerCharacter = AddNewCharacter(otherPlayer, otherPlayerPosition);
                
                this._otherPlayerCharacter.Add(otherPlayer.userId, newOtherPlayerCharacter);
            }
            
            #endregion
        }

        private Character AddNewCharacter(GameUserDto player, Vector3 currentPosition)
        {
            var spawnOtherCharacterPathIndex = player.spawnToken % this.spawnCharacterPaths.Count;

            var otherPlayerCharacterResource =
                this._loadedCharacters[spawnCharacterPaths[spawnOtherCharacterPathIndex]];
                
            return Instantiate(otherPlayerCharacterResource,
                currentPosition,
                Quaternion.identity);
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
