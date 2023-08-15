using Protocol.MessageBody;
using UnityEngine;

namespace GameObjects.Managers
{
    public class ConnectionLogger : MonoBehaviour
    {
        public static ConnectionLogger instance;
        
        private ConnectionManager connectionManager;
        
        private void OnEnable()
        {
            
        }

        public void Initialize()
        {
            DontDestroyOnLoad(this);
            instance = this;

            connectionManager = ConnectionManager.instance;
            
            connectionManager.connectionMessage.AddListener((connectionMessage) =>
            {
                switch (connectionMessage.messageName)
                {
                    case Protocol.MessageNameKey.ConnectToServer:
                    case Protocol.MessageNameKey.ConnectToServerComplete:
                    case Protocol.MessageNameKey.DisconnectFromServer:
                        var detailStringMessage = connectionMessage as IHasDetailMessage<string>;
                        Debug.Log(detailStringMessage?.GetDetailMessage());
                        break;
                    
                    case Protocol.MessageNameKey.UserConnected:
                        var detailGameUserMessage = connectionMessage as IHasDetailMessage<GameUser>;
                        Debug.Log(detailGameUserMessage?.GetDetailMessage());
                        break;
                }
            });
        }
    }
}
