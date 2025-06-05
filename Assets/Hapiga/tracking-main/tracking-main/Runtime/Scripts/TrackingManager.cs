using System.Collections.Generic;
using Firebase.Analytics;
using Hapiga.Core.Runtime.Singleton;
using UnityEngine;

namespace Hapiga.Tracking
{
    [Singleton("TrackingManager", true)]
    [DefaultExecutionOrder(-5000)]
    public class TrackingManager : Singleton<TrackingManager>
    {
         public bool firebase_init;
        public List<TrackerType> trackerTypes;
        public static List<ITracker> trackers;

        public static bool CanCollectUserData = true;

        public static void SetEnableUserDataCollected(bool isEnabled)
        {
            CanCollectUserData = isEnabled;
            for (int i = 0; i < trackers.Count; i++)
            {
                trackers[i].SetCollectDataEnabled(isEnabled);
            }
        }

        public override void Init()
        {
#if RELEASE
        Debug.unityLogger.logEnabled = false;
#else
            Debug.unityLogger.logEnabled = true;
#endif
            trackers = new List<ITracker>(trackerTypes.Count);
            for (int i = 0; i < trackerTypes.Count; i++)
            {
                ITracker tracker = null;
                switch (trackerTypes[i])
                {
                    case TrackerType.Firebase:
                        tracker = new FirebaseTracker();
                        break;
//					case TrackerType.Facebook:
//						tracker = new FacebookTracker();
//						break;
//					case TrackerType.GameAnalytic:
//						tracker = new GameAnalyticsTracker();
//						break;
                    // case TrackerType.ByteBrew:
//                        tracker = new ByteBrewTracker();
                        // break;
                }

                if (tracker != null)
                {
                    tracker?.Init();
                    trackers.Add(tracker);
                }
            }
        }

        public static void TrackScreen(string screen)
        {
            for (int i = 0; i < trackers.Count; i++)
            {
                trackers[i].TrackScreen(screen);
            }
        }

        public static void TrackEvent(string _eventName)
        {
            if (!CanCollectUserData)
            {
                return;
            }

            for (int i = 0; i < trackers.Count; i++)
            {
                trackers[i].TrackEvent(_eventName);
            }
        }

        public static void TrackEvent(string _eventName, string _paramName, string _paramValue)
        {
            if (!CanCollectUserData)
            {
                return;
            }

            for (int i = 0; i < trackers.Count; i++)
            {
                trackers[i].TrackEvent(_eventName, _paramName, _paramValue);
            }
        }

        public static void TrackEvent(string _eventName, string _paramName, int _paramValue)
        {
            if (!CanCollectUserData)
            {
                return;
            }

            for (int i = 0; i < trackers.Count; i++)
            {
                trackers[i].TrackEvent(_eventName, _paramName, _paramValue);
            }
        }

        public static void TrackEvent(string _eventName, string _paramName1, string _paramValue1, string _paramName2,
            string _paramValue2)
        {
            if (!CanCollectUserData)
            {
                return;
            }

            for (int i = 0; i < trackers.Count; i++)
            {
                trackers[i].TrackEvent(_eventName, _paramName1, _paramValue1, _paramName2, _paramValue2);
            }
        }

        public static void TrackEvent(string _eventName, string _paramName1, string _paramValue1, string _paramName2,
            int _paramValue2)
        {
            if (!CanCollectUserData)
            {
                return;
            }

            for (int i = 0; i < trackers.Count; i++)
            {
                trackers[i].TrackEvent(_eventName, _paramName1, _paramValue1, _paramName2, _paramValue2);
            }
        }

        public static void TrackEvent(string _eventName, string _paramName1, int _paramValue1, string _paramName2,
            int _paramValue2)
        {
            if (!CanCollectUserData)
            {
                return;
            }

            for (int i = 0; i < trackers.Count; i++)
            {
                trackers[i].TrackEvent(_eventName, _paramName1, _paramValue1, _paramName2, _paramValue2);
            }
        }

        public static void TrackEvent(string _eventName, string _paramName1, string _paramValue1, string _paramName2,
            float _paramValue2)
        {
            if (!CanCollectUserData)
            {
                return;
            }

            for (int i = 0; i < trackers.Count; i++)
            {
                trackers[i].TrackEvent(_eventName, _paramName1, _paramValue1, _paramName2, _paramValue2);
            }
        }

        public static void TrackEvent(string _eventName, string _paramName1, string _paramValue1, string _paramName2,
            string _paramValue2, string _paramName3, string _paramValue3)
        {
            if (!CanCollectUserData)
            {
                return;
            }

            for (int i = 0; i < trackers.Count; i++)
            {
                trackers[i].TrackEvent(_eventName, _paramName1, _paramValue1, _paramName2, _paramValue2, _paramName3,
                    _paramValue3);
            }
        }

        public static void TrackEvent(string _eventName, string _paramName1, string _paramValue1, string _paramName2,
            string _paramValue2, string _paramName3, int _paramValue3)
        {
            if (!CanCollectUserData)
            {
                return;
            }

            for (int i = 0; i < trackers.Count; i++)
            {
                trackers[i].TrackEvent(_eventName, _paramName1, _paramValue1, _paramName2, _paramValue2, _paramName3,
                    _paramValue3);
            }
        }
#if FIREBASE_ANALYTIC
        public static void TrackEvent(string _eventName, Parameter[] parameterList)
        {
            if (!CanCollectUserData)
            {
                return;
            }

            for (int i = 0; i < trackers.Count; i++)
            {
                trackers[i].TrackEvent(_eventName, parameterList);
            }
        }
#endif
        public static void TrackUserProperty(string _propertyName, string _propertyValue)
        {
            if (!CanCollectUserData)
            {
                return;
            }

            for (int i = 0; i < trackers.Count; i++)
            {
                trackers[i].TrackUserProperty(_propertyName, _propertyValue);
            }
        }

        public static void TrackLevelStart(int level)
        {
            if (!CanCollectUserData)
            {
                return;
            }

            for (int i = 0; i < trackers.Count; i++)
            {
                trackers[i].TrackLevelStart(level);
            }
        }

        public static void TrackLevelCompleted(int level, int playTime)
        {
            if (!CanCollectUserData)
            {
                return;
            }

            for (int i = 0; i < trackers.Count; i++)
            {
                trackers[i].TrackLevelCompleted(level, playTime);
            }
        }

        public static void TrackLevelFail(int level, int playTime)
        {
            if (!CanCollectUserData)
            {
                return;
            }

            for (int i = 0; i < trackers.Count; i++)
            {
                trackers[i].TrackLevelFail(level, playTime);
            }
        }
    }

    public enum TrackerType : byte
    {
        Firebase,
        Facebook,
        GameAnalytic
    }
}