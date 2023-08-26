using System;

namespace Protocol.MessageBody
{
    [Serializable]
    public class GameUser
    {
        public string connectionId;

        public string userId;
        public string nickname;

        public int spawnToken;
        public long spawnTime;

        public string? positionString;
        public string? bodyRotationString;
        public string? headRotationString;
        public string? cannonRotationString;
    }

    [Serializable]
    public class GameUserDto
    {
        public string userId;
        public string nickname;


        public int spawnToken;
        public long spawnTime;

        public string? positionString;
        public string? bodyRotationString;
        public string? headRotationString;
        public string? cannonRotationString;
    }
}
