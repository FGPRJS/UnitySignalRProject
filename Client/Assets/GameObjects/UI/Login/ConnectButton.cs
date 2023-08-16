using System.Collections;
using GameObjects.Managers;
using Protocol;
using UnityEngine;
using UnityEngine.UI;

namespace GameObjects.UI.Login
{
    public class ConnectButton : MonoBehaviour
    {
        public Button button;
        
        private void Start()
        {
            button.enabled = false;
            
            StartCoroutine(WaitForConnectionManager());
        }
        
        IEnumerator WaitForConnectionManager()
        {
            while((ConnectionManager.instance == null) 
                  || (ConnectionManager.instance.signalR == null))
            {
                yield return null;
            }
            
            button.onClick.AddListener(() =>
            {
                ConnectionManager.instance.signalR
                    .Invoke(MessageNameKey.FirstAccessInfo, "Dummy1");
            });

            button.enabled = true;
        }
    }
}
