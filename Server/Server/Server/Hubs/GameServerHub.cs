using Microsoft.AspNetCore.SignalR;
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

            await Clients.Others.SendAsync(MessageNameKey.UserDisconnected, disconnectedGameUser);

            await base.OnDisconnectedAsync(exception);
        }

        #endregion

        public async Task FirstAccessInfo(string nickname)
        {
            var newUser = this._gameServerManager.CreateGameUser(Context.ConnectionId, nickname);
            var allUser = this._gameServerManager.GetAllUsers();

            await Clients.Caller.SendAsync(MessageNameKey.FirstAccessInfo, newUser, allUser);
            await Clients.Others.SendAsync(MessageNameKey.UserConnected, newUser);
        }

        public async Task CharacterMovement(CharacterMovement movement)
        {
            await Clients.All.SendAsync(MessageNameKey.CharacterMovement, movement);
        }
               
    }
}
