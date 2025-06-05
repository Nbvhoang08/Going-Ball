#if FIREBASE_REMOTE
using Firebase;
using Firebase.Extensions;
using Firebase.RemoteConfig;
#endif
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

namespace Hapiga.RemoteConfig
{
    public class FirebaseRemoteConfig : IRemoteConfig
    {
#if FIREBASE_REMOTE
        private DependencyStatus dependencyStatus = DependencyStatus.UnavailableOther;
#endif

        public bool isFirebaseInitialized = false;

        private RemoteConfigManager remoteConfigManager;

        public FirebaseRemoteConfig(RemoteConfigManager _remoteConfigManager)
        {
            remoteConfigManager = _remoteConfigManager;
        }

        public void Init()
        {
            InitializeFirebase();
        }
// #if FIREBASE_REMOTE
//             Debug.Log("Remote config fix dependencies");
//             FirebaseApp.CheckAndFixDependenciesAsync().ContinueWithOnMainThread(task =>
//             {
//                 dependencyStatus = task.Result;
//                 if (dependencyStatus == DependencyStatus.Available)
//                 {
//                     InitializeFirebase();
//                 }
//                 else
//                 {
//                     Debug.LogError(
//                         "Could not resolve all Firebase dependencies: " + dependencyStatus);
//                 }
//             });
// #endif
//         }

        public void InitializeFirebase()
        {
#if FIREBASE_REMOTE
            Debug.Log("Firebase fetch data");

            Dictionary<string, object> defaults =
                new Dictionary<string, object>(remoteConfigManager.remoteConfigData.RemoteConfigList.Count);
            // These are the values that are used if we haven't fetched data from the
            // server
            // yet, or if we ask for values that the server doesn't have:
            for (int i = 0; i < remoteConfigManager.remoteConfigData.RemoteConfigList.Count; i++)
            {
                defaults.Add(remoteConfigManager.remoteConfigData.RemoteConfigList[i].key,
                    remoteConfigManager.remoteConfigData.RemoteConfigList[i].defaultValue);
            }

            Firebase.RemoteConfig.FirebaseRemoteConfig.DefaultInstance.SetDefaultsAsync(defaults)
                .ContinueWithOnMainThread(
                    task =>
                    {
                        Debug.Log("RemoteConfig configured and ready!");
                        isFirebaseInitialized = true;
                        if (remoteConfigManager.remoteConfigData.isGetRemoteConfig)
                        {
                            GetRemoteConfig();
                        }
                    });
#endif
        }

        private Task GetRemoteConfig()
        {
#if FIREBASE_REMOTE
            Task fetchTask = Firebase.RemoteConfig.FirebaseRemoteConfig.DefaultInstance.FetchAsync(
                TimeSpan.Zero);
            return fetchTask.ContinueWithOnMainThread(FetchComplete);
#endif
            return null;
        }

        private void FetchComplete(Task fetchTask)
        {
#if FIREBASE_REMOTE
            if (fetchTask.IsCanceled)
            {
                Debug.Log("Fetch canceled.");
            }
            else if (fetchTask.IsFaulted)
            {
                Debug.Log("Fetch encountered an error.");
            }
            else if (fetchTask.IsCompleted)
            {
                Debug.Log("Fetch completed successfully!");
            }

            var info = Firebase.RemoteConfig.FirebaseRemoteConfig.DefaultInstance.Info;
            switch (info.LastFetchStatus)
            {
                case LastFetchStatus.Success:
                    Firebase.RemoteConfig.FirebaseRemoteConfig.DefaultInstance.ActivateAsync().ContinueWithOnMainThread(
                        task =>
                        {
                            Debug.Log(String.Format("Remote data loaded and ready (last fetch time {0}).",
                                info.FetchTime));
                            remoteConfigManager.FetchStatus = RemoteFetchStatus.FetchFinish;
                            remoteConfigManager.OnRemoteConfigFetched(true);
                        });

                    break;
                case LastFetchStatus.Failure:
                    switch (info.LastFetchFailureReason)
                    {
                        case FetchFailureReason.Error:
                            Debug.Log("Fetch failed for unknown reason");
                            break;
                        case FetchFailureReason.Throttled:
                            Debug.Log("Fetch throttled until " + info.ThrottledEndTime);
                            break;
                    }

                    remoteConfigManager.FetchStatus = RemoteFetchStatus.FetchFinish;
                    remoteConfigManager.OnRemoteConfigFetched(false);
                    break;
                case LastFetchStatus.Pending:
                    Debug.Log("Latest Fetch call still pending.");
                    remoteConfigManager.FetchStatus = RemoteFetchStatus.FetchFinish;
                    remoteConfigManager.OnRemoteConfigFetched(false);
                    break;
            }
#endif
        }

        public string GetStringValue(string paramKey)
        {
#if FIREBASE_REMOTE
            return Firebase.RemoteConfig.FirebaseRemoteConfig.DefaultInstance.GetValue(paramKey).StringValue;
#endif
            return string.Empty;
        }

        public int GetIntValue(string paramKey)
        {
#if FIREBASE_REMOTE
            return (int)Firebase.RemoteConfig.FirebaseRemoteConfig.DefaultInstance.GetValue(paramKey).LongValue;
#endif
            return 0;
        }

        public bool GetBoolValue(string paramKey)
        {
#if FIREBASE_REMOTE
            return Firebase.RemoteConfig.FirebaseRemoteConfig.DefaultInstance.GetValue(paramKey).BooleanValue;
#endif
            return false;
        }
    }
}