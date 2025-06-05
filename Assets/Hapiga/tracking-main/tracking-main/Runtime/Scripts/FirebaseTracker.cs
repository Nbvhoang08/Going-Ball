#if FIREBASE_ANALYTIC
using Firebase;
using Firebase.Analytics;
using Firebase.Extensions;
using UnityEngine;
#endif
using System.Threading.Tasks;

namespace Hapiga.Tracking
{
   public class FirebaseTracker : ITracker
    {
        private bool canCollectData = true;
        private bool isAvailable;

        public void Init()
        {
        }

        public FirebaseTracker()
        {
#if FIREBASE_ANALYTIC
            FirebaseApp.CheckAndFixDependenciesAsync().ContinueWithOnMainThread(task =>
            {
                if (task.Result == DependencyStatus.Available)
                {
                    Debug.Log("Firebase correctly Initialized");
                    isAvailable = true;
                    TrackingManager.Instance.firebase_init = true;
                }
                else
                {
                    Debug.Log("Could not resolve all Firebase dependencies: " + task.Result);
                }
            });
#endif
        }

        public void TrackScreen(string screen)
        {
            if (!isAvailable) return;
#if FIREBASE_ANALYTIC
            FirebaseAnalytics.LogEvent(FirebaseAnalytics.EventScreenView, FirebaseAnalytics.ParameterScreenName,
                screen);
#endif
        }

        public void TrackEvent(string _eventName)
        {
            if (!isAvailable) return;
#if FIREBASE_ANALYTIC
            FirebaseAnalytics.LogEvent(_eventName);
#endif
        }

        public void TrackEvent(string _eventName, string _paramName, string _paramValue)
        {
            if (!isAvailable) return;
#if FIREBASE_ANALYTIC
            FirebaseAnalytics.LogEvent(_eventName, _paramName, _paramValue);
            
#endif
        }

        public void TrackEvent(string _eventName, string _paramName, int _paramValue)
        {
            if (!isAvailable) return;
#if FIREBASE_ANALYTIC
            FirebaseAnalytics.LogEvent(_eventName, _paramName, _paramValue);
#endif
        }

        public void TrackEvent(string _eventName, string _paramName1, string _paramValue1, string _paramName2,
            string _paramValue2)
        {
            if (!isAvailable) return;
#if FIREBASE_ANALYTIC
            FirebaseAnalytics.LogEvent(_eventName, new Parameter[]
            {
                new Parameter(_paramName1, _paramValue1),
                new Parameter(_paramName2, _paramValue2)
            });
#endif
        }

        public void TrackEvent(string _eventName, string _paramName1, string _paramValue1, string _paramName2,
            int _paramValue2)
        {
            if (!isAvailable) return;
#if FIREBASE_ANALYTIC
            FirebaseAnalytics.LogEvent(_eventName, new Parameter[]
            {
                new Parameter(_paramName1, _paramValue1),
                new Parameter(_paramName2, _paramValue2)
            });
#endif
        }

        public void TrackEvent(string _eventName, string _paramName1, int _paramValue1, string _paramName2,
            int _paramValue2)
        {
            if (!isAvailable) return;
#if FIREBASE_ANALYTIC
            FirebaseAnalytics.LogEvent(_eventName, new Parameter[]
            {
                new Parameter(_paramName1, _paramValue1),
                new Parameter(_paramName2, _paramValue2)
            });
#endif
        }

        public void TrackEvent(string _eventName, string _paramName1, string _paramValue1, string _paramName2,
            float _paramValue2)
        {
            if (!isAvailable) return;
#if FIREBASE_ANALYTIC
            FirebaseAnalytics.LogEvent(_eventName, new Parameter[]
            {
                new Parameter(_paramName1, _paramValue1),
                new Parameter(_paramName2, _paramValue2)
            });
#endif
        }

        public void TrackEvent(string _eventName, string _paramName1, string _paramValue1, string _paramName2,
            string _paramValue2,
            string _paramName3, string _paramValue3)
        {
            if (!isAvailable) return;
#if FIREBASE_ANALYTIC
            FirebaseAnalytics.LogEvent(_eventName, new Parameter[]
            {
                new Parameter(_paramName1, _paramValue1),
                new Parameter(_paramName2, _paramValue2),
                new Parameter(_paramName3, _paramValue3)
            });
#endif
        }

        public void TrackEvent(string _eventName, string _paramName1, string _paramValue1, string _paramName2,
            string _paramValue2,
            string _paramName3, int _paramValue3)
        {
            if (!isAvailable) return;
#if FIREBASE_ANALYTIC
            FirebaseAnalytics.LogEvent(_eventName, new Parameter[]
            {
                new Parameter(_paramName1, _paramValue1),
                new Parameter(_paramName2, _paramValue2),
                new Parameter(_paramName3, _paramValue3)
            });
#endif
        }

#if FIREBASE_ANALYTIC
        public void TrackEvent(string _eventName, Parameter[] parameterList)
        {
            if (!isAvailable) return;
            if (parameterList.Length > 0)
            {
                FirebaseAnalytics.LogEvent(_eventName, parameterList);
            }
            else
            {
                FirebaseAnalytics.LogEvent(_eventName);
            }
        }
#endif

        public void TrackUserProperty(string _propertyName, string _propertyValue)
        {
            if (!isAvailable) return;
#if FIREBASE_ANALYTIC
            FirebaseAnalytics.SetUserProperty(_propertyName, _propertyValue);
#endif
        }

        public void SetCollectDataEnabled(bool isEnabled)
        {
            canCollectData = isEnabled;
        }

        public void TrackLevelStart(int level)
        {
            TrackEvent("play_level", "level", level.ToString());
        }

        public void TrackLevelCompleted(int level, int playTime)
        {
            TrackEvent("finish_level", "level", level.ToString(), "play_time", playTime);
        }

        public void TrackLevelFail(int level, int playTime)
        {
            TrackEvent("fail_level", "level", level.ToString(), "play_time", playTime);
        }
    }
}