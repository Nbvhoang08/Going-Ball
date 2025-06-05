using System.Threading.Tasks;
using System;
using System.Collections.Generic;
#if FIREBASE_ANALYTIC
using Firebase;
using Firebase.Analytics;
using Firebase.Extensions;
using UnityEngine;
#endif

namespace Hapiga.Tracking
{
    public interface ITracker
    {
        void Init();
        void TrackScreen(string screen);
        void TrackEvent(string _eventName);
        void TrackEvent(string _eventName, string _paramName, string _paramValue);
        void TrackEvent(string _eventName, string _paramName, int _paramValue);

        void TrackEvent(string _eventName, string _paramName1, string _paramValue1, string _paramName2,
            string _paramValue2);

        void TrackEvent(string _eventName, string _paramName1, string _paramValue1, string _paramName2,
            int _paramValue2);

        void TrackEvent(string _eventName, string _paramName1, int _paramValue1, string _paramName2,
            int _paramValue2);

        void TrackEvent(string _eventName, string _paramName1, string _paramValue1, string _paramName2,
            float _paramValue2);

        void TrackEvent(string _eventName, string _paramName1, string _paramValue1, string _paramName2,
            string _paramValue2, string _paramName3, string _paramValue3);

        void TrackEvent(string _eventName, string _paramName1, string _paramValue1, string _paramName2,
            string _paramValue2, string _paramName3, int _paramValue3);
#if FIREBASE_ANALYTIC
        void TrackEvent(string _eventName, Parameter[] parameterList);
#endif
        void TrackUserProperty(string _propertyName, string _propertyValue);
        void SetCollectDataEnabled(bool isEnabled);
        void TrackLevelStart(int level);
        void TrackLevelCompleted(int level, int playTime);
        void TrackLevelFail(int level, int playTime);
    }
}