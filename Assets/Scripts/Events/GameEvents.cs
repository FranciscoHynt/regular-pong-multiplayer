using Enums;
using UnityEngine.Events;

namespace Events
{
    public static class GameEvents
    {
        public static readonly GoalEvent GoalEvent = new GoalEvent();
    }
    
    public class GoalEvent : UnityEvent<PlayerSide> {}
}