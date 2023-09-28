using UnityEngine;

namespace Unity.Services.Samples.Friends
{
    public static class ColorUtils
    {
        public static Color GetPresenceColor(int index)
        {
            var colorIndex = Mathf.Clamp(index, 0, s_PresenceUIColors.Length - 1);
            return s_PresenceUIColors[colorIndex];
        }

        //Mapping of colors to Availability
        static Color[] s_PresenceUIColors =
        {
            GreenColor, //ONLINE
            YellowColor, //BUSY
            RedColor, //AWAY
            GrayColor, //INVISIBLE
            GrayColor, //OFFLINE
            new Color(1f, .4f, 1f) //UNKNOWN
        };

        public static Color DefaultNavBarTabColor => ColorUtility.TryParseHtmlString("#655EBC", out var color) ? color : Color.black;
        public static Color DefaultNavBarIconColor => Color.white;
        public static Color SelectedNavBarTabColor => ColorUtility.TryParseHtmlString("#3D3781", out var color) ? color : Color.black;
        public static Color GreenColor => ColorUtility.TryParseHtmlString("#7ED321", out var color) ? color : Color.green;
        public static Color YellowColor => ColorUtility.TryParseHtmlString("#FDCC51", out var color) ? color : Color.yellow;
        public static Color RedColor => ColorUtility.TryParseHtmlString("#F70808", out var color) ? color : Color.red;
        public static Color GrayColor => ColorUtility.TryParseHtmlString("#A4A4A4", out var color) ? color : Color.gray;
    }
}