namespace Protocol.MessageBody
{
    public class GameUser
    {
        public string connectionId { get; set; }

        public string userId { get; set; }
        public string nickname { get; set; }

        public int spawnToken { get; set; }
        public long spawnTime { get; set; }

        public string? positionString { get; set; }
        public string? bodyRotationString { get; set; }
        public string? headRotationString { get; set; }
        public string? cannonRotationString { get; set; }
    }

    public class GameUserDto
    {
        public string userId { get; set; }
        public string nickname { get; set; }


        public int spawnToken { get; set; }
        public long spawnTime { get; set; }

        public string positionString { get; set; }
        public string bodyRotationString { get; set; }
        public string headRotationString { get; set; }
        public string cannonRotationString { get; set; }
    }
}
