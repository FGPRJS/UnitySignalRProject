using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using GameObjects.Characters;
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
        public UnityEvent<GameUserDto> newUserConnectedEvent;
        public UnityEvent<GameUserDto> userDisconnectedEvent;
        
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
            
            ConnectionManager.instance.signalR.On<GameUserDto, GameUserDto[]>(
                Protocol.MessageNameKey.FirstAccessInfo, 
                (current, otherUsers) =>
                {
                    this.currentPlayer = current;
                    this.otherPlayers = otherUsers.ToDictionary((user) => user.userId);
                
                    GameSceneManager.instance.LoadScene(GameSceneName.MainScene);
                });
            
            ConnectionManager.instance.signalR.On<GameUserDto>(
                Protocol.MessageNameKey.UserConnected, 
                (user) =>
                {
                    this.otherPlayers.Add(user.userId, user);
                    
                    newUserConnectedEvent.Invoke(user);
                });
        }
    }
}
