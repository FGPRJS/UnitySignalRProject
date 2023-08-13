using Microsoft.AspNetCore.SignalR;
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

        public async Task ConnectToServer()
        {
            var newUser = this._gameServerManager.CreateGameUser();

            await Clients.All.SendCoreAsync(MessageNameKey.UserConnected, new object?[]{newUser});
        }
    }

    internal class MessageNameKey
    {
        public const string UserConnected = "UserConnected";
    }
}
