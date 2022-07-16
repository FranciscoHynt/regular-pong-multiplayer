using Enums;
using UnityEngine.Events;

namespace Events
{
    public static class GameEvents
    {
        public static readonly GoalEvent GoalEvent = new GoalEvent();
        public static readonly ShowScoreEvent ShowScoreEvent = new ShowScoreEvent();
    }
    
    public class GoalEvent : UnityEvent<PlayerSide> {}
    public class ShowScoreEvent : UnityEvent<ShowScoreData> {}

    public readonly struct ShowScoreData
    {
        public readonly int score;
        public readonly PlayerSide side;

        public ShowScoreData(int score, PlayerSide side)
        {
            this.score = score;
            this.side = side;
        }
    }
}