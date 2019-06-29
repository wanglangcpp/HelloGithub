using UnityEngine;
using System.Collections;
using System;

namespace Genesis.GameClient
{
    public static class TimeUtility
    {
        public static bool IsSameDay(DateTime utcDateTime, int hours = 0, int minutes = 0, int seconds = 0, int milliseconds = 0)
        {
            DateTime utcNow = GameEntry.Time.LobbyServerUtcTime;
            DateTime utcLast = new DateTime(utcNow.Year, utcNow.Month, utcNow.Day, hours, minutes, seconds, milliseconds, DateTimeKind.Utc);
            if (utcLast > utcNow)
            {
                utcLast = utcLast.AddDays(-1);
            }

            return utcDateTime >= utcLast;
        }

        public static bool IsSameDay(DateTime utcDateTime, TimeSpan timeSpan)
        {
            return IsSameDay(utcDateTime, timeSpan.Hours, timeSpan.Minutes, timeSpan.Seconds, timeSpan.Milliseconds);
        }

        public static DateTime GetNearestUtcTime(int hours = 0, int minutes = 0, int seconds = 0, int milliseconds = 0)
        {
            DateTime utcNow = GameEntry.Time.LobbyServerUtcTime;
            DateTime utcNearest = new DateTime(utcNow.Year, utcNow.Month, utcNow.Day, hours, minutes, seconds, milliseconds, DateTimeKind.Utc);
            if (utcNearest < utcNow)
            {
                utcNearest = utcNearest.AddDays(1);
            }

            return utcNearest;
        }

        public static DateTime GetNearestUtcTime(TimeSpan timeSpan)
        {
            return GetNearestUtcTime(timeSpan.Hours, timeSpan.Minutes, timeSpan.Seconds, timeSpan.Milliseconds);
        }

    }
}