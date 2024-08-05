using System;

namespace Chameleon.Game.ArcadeIdle.Helpers
{
    public static class TimeConverter
    {
        public static string ConvertSecondsToMinutes(int seconds)
        {
            int timespanMinutes = TimeSpan.FromSeconds(seconds).Minutes;
            seconds -= timespanMinutes * 60;
            int timespanSeconds = seconds;
            string convertedTimeString = timespanMinutes.ToString("00") + " : " + timespanSeconds.ToString("00");
            return convertedTimeString;
        }
    }
}