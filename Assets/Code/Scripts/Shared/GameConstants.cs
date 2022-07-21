namespace Tanks.Shared
{
    public static class GameConstants
    {
        public const string BUTTON_NAME_SUBMIT = "Submit";
        public const string AXIS_NAME_HORIZONTAL_UI = "HorizontalUI";
        public const string AXIS_NAME_VERTICAL_UI = "VerticalUI";
        public const string BUTTON_NAME_PAUSE = "Pause";

        public enum GameMode
        {
            SOLO_1VS1,
            TEAM_2VS2,
            PVP_1VS1,
        }
    }
}
