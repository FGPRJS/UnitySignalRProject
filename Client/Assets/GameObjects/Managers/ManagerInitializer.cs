using System;
using UnityEngine;

namespace GameObjects.Managers
{
    public class ManagerInitializer : MonoBehaviour
    {
        public ConnectionManager connectionManager;
        public ConnectionLogger connectionLogger;
        public GameSceneManager gameSceneManager;

        private void Start()
        {
            connectionManager.Initialize();
            connectionLogger.Initialize();
            gameSceneManager.Initialize();
        }
    }
}
