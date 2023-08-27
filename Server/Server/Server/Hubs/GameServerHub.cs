using Microsoft.AspNetCore.SignalR;
using Newtonsoft.Json;
using Protocol;
using Protocol.MessageBody;
using Server.Manager;

namespace Server.Hubs
{
    public class GameServerHub : Hub
    {
        private readonly GameServerManager _gameServerManager;

        public GameServerHub(GameServerManager gameServerManager)
        {
            this._gameServerManager = gameServerManager;
        }

        #region Connection

        public override async Task OnConnectedAsync()
        {
            await base.OnConnectedAsync();
        }

        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            var disconnectedGameUser = this._gameServerManager.RemoveGameUser(Context.ConnectionId);

            await Clients.Others.SendAsync(MessageNameKey.UserDisconnected, 
                JsonConvert.SerializeObject(disconnectedGameUser));

            await base.OnDisconnectedAsync(exception);
        }

        #endregion

        public async Task FirstAccessInfo(string nickname)
        {
            var newUser = this._gameServerManager.CreateGameUser(Context.ConnectionId, nickname);
            var allUser = this._gameServerManager.GetAllUsers();

            var serializedNewUser = JsonConvert.SerializeObject(newUser);

            await Clients.Others.SendAsync(MessageNameKey.UserConnected,
                serializedNewUser);
            await Clients.Caller.SendAsync(MessageNameKey.FirstAccessInfo,
                serializedNewUser,
                JsonConvert.SerializeObject(allUser));
        }

        public async Task CharacterMovement(string movementRaw)
        {
            var movement = JsonConvert.DeserializeObject<CharacterMovement>(movementRaw);

            this._gameServerManager.RefreshUserPosition(Context.ConnectionId, movement.position);

            await Clients.All.SendAsync(MessageNameKey.CharacterMovement,
                JsonConvert.SerializeObject(movement));
        }
               
    }
}
