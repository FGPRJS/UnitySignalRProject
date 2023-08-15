using System;
using Protocol.MessageBody;
using UnityEngine;
using UnityEngine.Events;

namespace GameObjects.Managers
{
    public class ConnectionManager : MonoBehaviour
    {
        public static ConnectionManager instance;
        
        private SignalR signalR;

        public string connectionString;
            
        public UnityEvent<ConnectionMessage> connectionMessage;

        #region Base Function
        void Awake()
        {
            
        }

        public void Initialize()
        {
            DontDestroyOnLoad(this);
            instance = this;
            
            this.connectionMessage = new UnityEvent<ConnectionMessage>();
            this.signalR = new SignalR();

            this.signalR.Init(connectionString);
            
            #region Connection
            this.signalR.ConnectionStarted += (sender, args) =>
            {
                connectionMessage.Invoke(
                    new ConnectToServerCompleteMessage("Connection started.")
                );
            };

            this.signalR.ConnectionClosed += (sender, args) =>
            {
                connectionMessage.Invoke(
                    new DisconnectFromServerMessage("Connection closed.")
                );
            };
            #endregion
            
            #region Message
            this.signalR.On(Protocol.MessageNameKey.UserConnected, new Action<GameUser>((userInfo) =>
            {
                connectionMessage.Invoke(
                    new UserConnectedMessage(userInfo));
            }));
            #endregion
        }
        
        #endregion
        
        public void Connect()
        {
            connectionMessage.Invoke(
                new ConnectToServerMessage("Connecting..."));
            this.signalR.Connect();
        }
    }

    public abstract class ConnectionMessage
    {
        public string messageName;
    }

    public interface IHasDetailMessage<out T>
    {
        T GetDetailMessage();
    }
    
    #region Implements
    public class ConnectToServerMessage : ConnectionMessage, IHasDetailMessage<string>
    {
        private readonly string simpleMessage;
        
        public ConnectToServerMessage(string simpleMessage)
        {
            base.messageName = Protocol.MessageNameKey.ConnectToServer;
            this.simpleMessage = simpleMessage;
        }

        public string GetDetailMessage()
        {
            return this.simpleMessage;
        }
    }
    
    public class ConnectToServerCompleteMessage : ConnectionMessage, IHasDetailMessage<string>
    {
        private readonly string simpleMessage;
        
        public ConnectToServerCompleteMessage(string simpleMessage)
        {
            base.messageName = Protocol.MessageNameKey.ConnectToServerComplete;

            this.simpleMessage = simpleMessage;
        }

        public string GetDetailMessage()
        {
            return this.simpleMessage;
        }
    }
    
    public class DisconnectFromServerMessage : ConnectionMessage, IHasDetailMessage<string>
    {
        private readonly string simpleMessage;
        
        public DisconnectFromServerMessage(string simpleMessage)
        {
            base.messageName = Protocol.MessageNameKey.DisconnectFromServer;
            
            this.simpleMessage = simpleMessage;
        }

        public string GetDetailMessage()
        {
            return this.simpleMessage;
        }
    }
    
    public class UserConnectedMessage : ConnectionMessage, IHasDetailMessage<GameUser>
    {
        public Protocol.MessageBody.GameUser userData;
        
        public UserConnectedMessage(GameUser userData)
        {
            base.messageName = Protocol.MessageNameKey.UserConnected;
            this.userData = userData;
        }
        
        public GameUser GetDetailMessage()
        {
            return this.userData;
        }
    }
    #endregion
}
