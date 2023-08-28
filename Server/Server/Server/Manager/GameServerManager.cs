using Mapster;
using Protocol.MessageBody;
using System.Collections.Concurrent;

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

        public GameUserDto CreateGameUser(string connectionId, string nickname)
        {
            var newUser = new GameUser
            {
                connectionId = connectionId,
                userId = new string(Enumerable.Repeat(_chars, _gameConstantManager.GetGameConstant<int>(GameConstantKey.UserIdLength))
                    .Select(s => s[_random.Next(s.Length)]).ToArray()),
                nickname = nickname,
                spawnToken = this._random.Next(),
                spawnTime = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds()
            };
            
            _userInfo.TryAdd(connectionId, newUser);
            
            return newUser.Adapt<GameUserDto>();
        }

        public List<GameUserDto> GetAllUsers()
        {
            return this._userInfo.Values.ToList().Adapt<List<GameUserDto>>();
        }

        public GameUserDto? RemoveGameUser(string connectionId)
        {
            this._userInfo.TryRemove(connectionId, out var user);

            return user?.Adapt<GameUserDto>();
        }

        public void RefreshUserPosition(string connectionId, string position, string bodyRotation)
        {
            this._userInfo[connectionId].positionString = position;
            this._userInfo[connectionId].bodyRotationString = bodyRotation;
        }
    }
}
