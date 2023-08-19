using System.Collections.Concurrent;

namespace Server.Manager
{
    public class GameConstantManager
    {
        private readonly ConcurrentDictionary<string, string> _gameConstants;

        public GameConstantManager()
        {
            this._gameConstants = new ConcurrentDictionary<string, string>(
                new []
                {
                    new KeyValuePair<string, string>(GameConstantKey.UserIdLength,"16"),
                    new KeyValuePair<string, string>(GameConstantKey.RoomTokenLength,"9"),
                    new KeyValuePair<string, string>(GameConstantKey.MaxUserCount,"16")
                });
        }

        public TType GetGameConstant<TType>(string key)
        {
            this._gameConstants.TryGetValue(key, out var gameConstantValue);

            if (gameConstantValue == null)
            {
                throw new ArgumentOutOfRangeException();
            }

            return (TType)Convert.ChangeType(gameConstantValue, typeof(TType));
        }
    }

    public class GameConstantKey
    {
        public const string UserIdLength = "UserIdLength";
        public const string RoomTokenLength = "RoomTokenLength";
        public const string MaxUserCount = "MaxUserCount";
    }
}
