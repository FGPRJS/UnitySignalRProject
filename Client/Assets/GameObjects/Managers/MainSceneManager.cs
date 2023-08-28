using System.Collections.Generic;
using Cinemachine;
using GameObjects.Characters;
using GameObjects.Locations;
using Newtonsoft.Json;
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

        public float dismatchDistanceStandard = 1;

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
            ConnectionManager.instance.signalR.On<string>(
                MessageNameKey.CharacterMovement, (characterMovementRaw) =>
                {
                    var characterMovement = JsonConvert.DeserializeObject<CharacterMovement>(characterMovementRaw);
            
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
                        var receivedPosition = VectorConverter.ToUnityVector3(characterMovement.position);
                        var receivedRotation = VectorConverter.ToUnityQuaternion(characterMovement.rotation);
                        
                        if (Vector3.Distance(targetCharacter.transform.position, receivedPosition) > dismatchDistanceStandard)
                        {
                            var transform1 = targetCharacter.transform;
                            transform1.position = receivedPosition;
                            transform1.rotation = receivedRotation;
                        }
                
                        targetCharacter.MoveCharacter(movement);
                    }
                });
            ConnectionManager.instance.signalR.On<string>(
                MessageNameKey.UserDisconnected, (disconnectedUserRaw) =>
                {
                    var disconnectedUser = JsonConvert.DeserializeObject<GameUserDto>(disconnectedUserRaw);

                    this._otherPlayerCharacter.Remove(disconnectedUser.userId, out var otherPlayer);
                    
                    Object.Destroy(otherPlayer.gameObject);
                });
            ConnectionManager.instance.signalR.On<string>(
                MessageNameKey.UserConnected,
                connectedUserRaw =>
                {
                    var connectedUser = JsonConvert.DeserializeObject<GameUserDto>(connectedUserRaw);
                
                var otherPlayerPosition = 
                    VectorConverter.ToUnityVector3(connectedUser.positionString);
                var otherPlayerBodyRotation =
                    VectorConverter.ToUnityQuaternion(connectedUser.bodyRotationString);

                var newOtherPlayerCharacter = AddNewCharacter(
                    connectedUser, 
                    otherPlayerPosition,
                    otherPlayerBodyRotation);
                
                this._otherPlayerCharacter.Add(connectedUser.userId, newOtherPlayerCharacter);
            });
        }

        private void OnDisable()
        {
            this._playerInputActions.Character.Disable();
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

            this._playerCharacter = AddNewCharacter(
                gameManager.currentPlayer, 
                currentUserPosition,
                spawnZoneTarget.transform.rotation);

            playerCamera.Follow = this._playerCharacter.head.transform;
            
            #endregion
            
            #region Instantiate Others

            foreach (var otherPlayer in GameManager.instance.otherPlayers.Values)
            {
                var otherPlayerPosition = 
                    VectorConverter.ToUnityVector3(otherPlayer.positionString);
                var otherPlayerBodyRotation =
                    VectorConverter.ToUnityQuaternion(otherPlayer.bodyRotationString);

                var newOtherPlayerCharacter = AddNewCharacter(otherPlayer, otherPlayerPosition, otherPlayerBodyRotation);
                
                this._otherPlayerCharacter.Add(otherPlayer.userId, newOtherPlayerCharacter);
            }
            
            #endregion
        }

        private Character AddNewCharacter(
            GameUserDto player, 
            Vector3 currentPosition, 
            Quaternion currentBodyRotation)
        {
            var spawnOtherCharacterPathIndex = player.spawnToken % this.spawnCharacterPaths.Count;

            var otherPlayerCharacterResource =
                this._loadedCharacters[spawnCharacterPaths[spawnOtherCharacterPathIndex]];
                
            return Instantiate(otherPlayerCharacterResource,
                currentPosition,
                currentBodyRotation);
        }

        private void FixedUpdate()
        {
            var moveInput = this._playerInputActions.Character.Move.ReadValue<Vector2>();
            var characterTransform = this._playerCharacter.transform;
            var position = characterTransform.position;
            var bodyRotation = characterTransform.rotation;

            ConnectionManager.instance.signalR.Invoke(
                MessageNameKey.CharacterMovement, JsonConvert.SerializeObject(new CharacterMovement()
            {
                userId = GameManager.instance.currentPlayer.userId,
                movement = $"{moveInput.x},{moveInput.y}",
                rotation = $"{bodyRotation.x},{bodyRotation.y},{bodyRotation.z},{bodyRotation.w}",
                position = $"{position.x},{position.y},{position.z}"
            }));
        }
    }
}
