using System;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace GameObjects.Managers
{
    public class ConnectionManager : MonoBehaviour
    {
        public static ConnectionManager instance;
        
        public SignalR signalR { get; private set; }

        public string connectionString;

        #region Base Function
        void Awake()
        {
            if (instance == null)
            {
                instance = this;
                DontDestroyOnLoad(this);
            }
        }

        private void Start()
        {
            this.signalR = new SignalR();

            this.signalR.Init(connectionString);
            
            #region Connection
            this.signalR.ConnectionStarted += (sender, args) =>
            {
                Debug.Log("Connected.");
            };

            this.signalR.ConnectionClosed += (sender, args) =>
            {
                Debug.Log("Connection closed.");
            };
            #endregion

            this.signalR.Connect();
        }

        #endregion
    }
}
