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
                    new KeyValuePair<string, string>(GameConstantKey.UserTokenLength,"16"),
                    new KeyValuePair<string, string>(GameConstantKey.RoomTokenLength,"9"),
                    new KeyValuePair<string, string>(GameConstantKey.MaxUserCount,"16"),

                    new KeyValuePair<string, string>(GameConstantKey.SpawnXPosMax,"300"),
                    new KeyValuePair<string, string>(GameConstantKey.SpawnXPosMin,"600"),
                    new KeyValuePair<string, string>(GameConstantKey.SpawnYPosMax,"300"),
                    new KeyValuePair<string, string>(GameConstantKey.SpawnYPosMin,"600"),

                    new KeyValuePair<string, string>(GameConstantKey.SpawnZPos,"100")
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
        public const string UserTokenLength = "UserTokenLength";
        public const string RoomTokenLength = "RoomTokenLength";
        public const string MaxUserCount = "MaxUserCount";

        public const string SpawnXPosMax = "SpawnXPosMax";
        public const string SpawnXPosMin = "SpawnXPosMin";
        public const string SpawnYPosMax = "SpawnYPosMax";
        public const string SpawnYPosMin = "SpawnYPosMin";
        public const string SpawnZPos = "SpawnZPos";
    }
}
