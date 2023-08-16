using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Protocol;
using Protocol.MessageBody;
using UnityEngine;

namespace GameObjects.Managers
{
    public class GameManager : MonoBehaviour
    {
        public static GameManager instance;

        public GameUserDto currentUser { get; private set; }
        public Dictionary<string, GameUserDto> otherUsers { get; private set; }

        private void Awake()
        {
            if (instance == null)
            {
                instance = this;
                DontDestroyOnLoad(this);
            }

            otherUsers = new Dictionary<string, GameUserDto>();
            StartCoroutine(WaitForConnectionManager());
        }

        private void FirstAccessInfo(GameUserDto currentUser, GameUserDto[] otherUsers)
        {
            this.currentUser = currentUser;
            this.otherUsers = otherUsers.ToDictionary((user) => user.userId);
            
            GameSceneManager.instance.LoadScene(GameSceneName.MainScene);
        }

        IEnumerator WaitForConnectionManager()
        {
            while((ConnectionManager.instance == null) 
                  || (ConnectionManager.instance.signalR == null))
            {
                yield return null;
            }
            
            ConnectionManager.instance.signalR.On<GameUserDto, GameUserDto[]>(
                Protocol.MessageNameKey.FirstAccessInfo, FirstAccessInfo);
        }
    }
}
