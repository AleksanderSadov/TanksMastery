namespace Tanks.Shared
{
    public static class Events
    {
        public static RoundStartingEvent RoundStartingEvent = new RoundStartingEvent();
        public static RoundEndingEvent RoundEndingEvent = new RoundEndingEvent();
    }

    public class RoundStartingEvent : GameEvent
    {
        public int roundNumber;
    }

    public class RoundEndingEvent : GameEvent
    {
        TankManager[] playersTanks;
        TankManager roundWinner;
        TankManager gameWinner;
    }
}

