using System.Collections.Generic;

namespace Tanks.Gameplay
{
    public static class Events
    {
        public static RoundStartingEvent RoundStartingEvent = new RoundStartingEvent();
        public static RoundStartedEvent RoundStartedEvent = new RoundStartedEvent();
        public static RoundEndingEvent RoundEndingEvent = new RoundEndingEvent();
    }

    public class RoundStartingEvent : GameEvent
    {
        public int roundNumber;
    }

    public class RoundStartedEvent : GameEvent { }

    public class RoundEndingEvent : GameEvent
    {
        public List<Team> teams;
        public Team roundWinnerTeam;
        public Team gameWinnerTeam;
    }
}

