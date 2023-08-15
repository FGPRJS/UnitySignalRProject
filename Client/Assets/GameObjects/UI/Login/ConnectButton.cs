using GameObjects.Managers;
using UnityEngine;
using UnityEngine.UI;

namespace GameObjects.UI.Login
{
    public class ConnectButton : MonoBehaviour
    {
        public Button button;
        public ConnectionManager connectionManager;
        
        private void Awake()
        {
            this.button.onClick.AddListener(() =>
            {
                this.connectionManager.Connect();
            });
        }
    }
}
