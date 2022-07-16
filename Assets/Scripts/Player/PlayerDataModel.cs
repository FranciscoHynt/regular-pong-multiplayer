using Enums;

namespace Player
{
    public struct PlayerDataModel
    {
        public bool live { get; set; }
        public int score { get; set; }
        public PlayerSide side { get; set; }
    }
}