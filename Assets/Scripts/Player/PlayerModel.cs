using Enums;

namespace Player
{
    public class PlayerModel
    {
        public int Score
        {
            get => score;
            private set => score = value;
        }
        
        public int ConnectionId
        {
            get => connectionId;
            private set => connectionId = value;
        }
        
        public PlayerSide Side
        {
            get => side;
            private set => side = value;
        }
        
        private int score;
        private int connectionId;
        private PlayerSide side;

        public PlayerModel(int connectionId, PlayerSide side)
        {
            this.score = 0;
            this.side = side;
            this.connectionId = connectionId;
        }

        public void AddScore()
        {
            score++;
        }
    }
}