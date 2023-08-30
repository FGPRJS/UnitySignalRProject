using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using Protocol;
using Protocol.MessageBody;
using UnityEngine;
using UnityEngine.Events;

namespace GameObjects.Managers
{
    public class GameManager : MonoBehaviour
    {
        public static GameManager instance;

        public GameUserDto currentPlayer { get; private set; }
        public Dictionary<string, GameUserDto> otherPlayers { get; private set; }

        public UnityEvent<GameUserDto> UserConnectedEvent;
        public UnityEvent<GameUserDto> UserDisconnectedEvent;
        public UnityEvent<CharacterMovement> CharacterMovementEvent;
        
        private void Awake()
        {
            if (instance == null)
            {
                instance = this;
                DontDestroyOnLoad(this);
            }

            otherPlayers = new Dictionary<string, GameUserDto>();
            StartCoroutine(WaitForConnectionManager());
        }
        
        IEnumerator WaitForConnectionManager()
        {
            while((ConnectionManager.instance == null) 
                  || (ConnectionManager.instance.signalR == null))
            {
                yield return null;
            }
            
            ConnectionManager.instance.signalR.On<string, string>(
                MessageNameKey.FirstAccessInfo, 
                (currentRaw, otherUsersRaw) =>
                {
                    this.currentPlayer = JsonConvert.DeserializeObject<GameUserDto>(currentRaw);
                    var otherUsers = JsonConvert.DeserializeObject<List<GameUserDto>>(otherUsersRaw);
                    
                    this.otherPlayers = otherUsers.ToDictionary((user) => user.userId);
                
                    GameSceneManager.instance.LoadScene(GameSceneName.MainScene);
                });
            
            ConnectionManager.instance.signalR.On<string>(
                MessageNameKey.UserConnected,(userRaw) =>
                {
                    var user = JsonConvert.DeserializeObject<GameUserDto>(userRaw);
                    
                    this.otherPlayers.Add(user.userId, user);
                    
                    this.UserConnectedEvent.Invoke(user);
                });
            
            ConnectionManager.instance.signalR.On<string>(
                MessageNameKey.UserDisconnected,(disconnectedUserRaw) =>
                {
                    var disconnectedUser = JsonConvert.DeserializeObject<GameUserDto>(disconnectedUserRaw);

                    this.otherPlayers.Remove(disconnectedUser.userId);
                    
                    this.UserDisconnectedEvent.Invoke(disconnectedUser);
                });

            ConnectionManager.instance.signalR.On<string>(
                MessageNameKey.CharacterMovement, (characterMovementRaw) =>
                {
                    var characterMovement = JsonConvert.DeserializeObject<CharacterMovement>(characterMovementRaw);
                    
                    this.CharacterMovementEvent.Invoke(characterMovement);
                });
        }
    }
}
