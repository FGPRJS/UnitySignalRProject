using System.Collections.Concurrent;
using System.Numerics;

namespace Server.Manager
{
    public class GameServerManager
    {
        private readonly GameConstantManager _gameConstantManager;

        private const string _chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
        private readonly Random _random;

        private readonly ConcurrentDictionary<string, GameUser> _userInfo;

        public GameServerManager(GameConstantManager gameConstantManager)
        {
            this._random = new Random();

            this._userInfo = new ConcurrentDictionary<string, GameUser>();
            _gameConstantManager = gameConstantManager;
        }

        public GameUser CreateGameUser()
        {
            var newUserToken = new string(Enumerable.Repeat(
                    _chars,
                    _gameConstantManager.GetGameConstant<int>(GameConstantKey.UserTokenLength))
                .Select(s => s[this._random.Next(s.Length)]).ToArray());

            var newUser = new GameUser
            {
                token = newUserToken,
                position = new Vector3(
                    this._random.Next(
                        _gameConstantManager.GetGameConstant<int>(GameConstantKey.SpawnXPosMin),
                        _gameConstantManager.GetGameConstant<int>(GameConstantKey.SpawnXPosMax)),
                    this._random.Next(
                        _gameConstantManager.GetGameConstant<int>(GameConstantKey.SpawnYPosMin),
                        _gameConstantManager.GetGameConstant<int>(GameConstantKey.SpawnYPosMax)),
                    _gameConstantManager.GetGameConstant<int>(GameConstantKey.SpawnZPos))
            };

            this._userInfo.TryAdd(newUserToken, newUser);

            return newUser;
        }
    }

    public class GameUser
    {
        public string token { get; set; }
        public Vector3 position { get; set; }
    }
}
