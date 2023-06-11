using UnityEngine;
using System;
using System.Collections;
using System.Text.RegularExpressions;
using UnityEngine.Networking;

namespace DanielLochner.Assets.CreatureCreator
{
    public class WorldTimeManager : MonoBehaviourSingleton<WorldTimeManager>
    {
        #region Fields
        private TimeData initializedTimeData;
        private float initializedStartupTime;

        private const string API_URL = "https://worldtimeapi.org/api/ip";
        #endregion

        #region Properties
        public bool IsInitialized => initializedTimeData != null;

        public DateTime? Now
        {
            get
            {
                if (IsInitialized)
                {
                    return ParseDateTime(initializedTimeData.datetime).AddSeconds(Time.realtimeSinceStartup - initializedStartupTime);
                }
                return null;
            }
        }
        public DateTime? UtcNow
        {
            get
            {
                if (IsInitialized)
                {
                    return ParseDateTime(initializedTimeData.utc_datetime).AddSeconds(Time.realtimeSinceStartup - initializedStartupTime);
                }
                return null;
            }
        }
        #endregion

        #region Methods
        public void Start()
        {
            StartCoroutine(RetrieveCurrentDateTimeRoutine());
        }

        private IEnumerator RetrieveCurrentDateTimeRoutine()
        {
            UnityWebRequest webRequest = UnityWebRequest.Get(API_URL);

            yield return webRequest.SendWebRequest();

            if (webRequest.result == UnityWebRequest.Result.Success)
            {
                initializedTimeData = JsonUtility.FromJson<TimeData>(webRequest.downloadHandler.text);
                initializedStartupTime = Time.realtimeSinceStartup;
            }
            else
            {
                Debug.LogWarning($"Error retrieving current date time from WorldTime.org. ({webRequest.error})");
            }
        }

        private DateTime ParseDateTime(string datetime)
        {
            // Format: 2020-08-14T15:54:04+01:00

            // 0000-00-00
            string date = Regex.Match(datetime, @"^\d{4}-\d{2}-\d{2}").Value;
            // 00:00:00
            string time = Regex.Match(datetime, @"\d{2}:\d{2}:\d{2}").Value;

            return DateTime.Parse(string.Format("{0} {1}", date, time));
        }
        #endregion

        #region Structs
        public class TimeData
        {
            public string abbreviation;
            public string client_ip;
            public string datetime;
            public string dst;
            public string dst_from;
            public string dst_offset;
            public string dst_until;
            public string raw_offset;
            public string timezone;
            public string unixtime;
            public string utc_datetime;
            public string utc_offset;
        }
        #endregion
    }
}