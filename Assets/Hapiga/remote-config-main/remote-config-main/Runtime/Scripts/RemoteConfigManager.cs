using System.Collections;
using Hapiga.Core.Runtime.EventManager;
using Hapiga.Core.Runtime.Singleton;
using Hapiga.Tracking;
#if UNITY_EDITOR
using UnityEditor;
#endif
#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#endif
using UnityEngine;

namespace Hapiga.RemoteConfig
{
    [Singleton("RemoteConfigManager", true)]
    [DefaultExecutionOrder(-4999)]
    public class RemoteConfigManager : Singleton<RemoteConfigManager>
    {
        public RemoteFetchStatus FetchStatus { get; set; }
        public bool isFetchSuccess;
#if ODIN_INSPECTOR
        [InlineEditor]
#endif
        public RemoteConfigData remoteConfigData;

        private RemoteConfigFetched _remoteConfigFetchedEvent = new RemoteConfigFetched();
        private IRemoteConfig remoteConfigModule;

#if UNITY_EDITOR
        private void OnValidate()
        {
            if (remoteConfigData == null)
            {
                remoteConfigData = RemoteConfigData.Instance;
                EditorUtility.SetDirty(this);
            }
        }
#endif
        public override void Init()
        {
            // OnFetched();
            StartCoroutine(Wait());

            IEnumerator Wait()
            {
                var trackManager = TrackingManager.Instance;
                yield return new WaitUntil(() => trackManager.firebase_init);
                remoteConfigData.SetDefaultValue();
                if (remoteConfigData.isGetRemoteConfig)
                {
                    FetchStatus = RemoteFetchStatus.Fetching;
                    remoteConfigModule = new FirebaseRemoteConfig(this);
                    remoteConfigModule.Init();
                }
                else
                {
                    FetchStatus = RemoteFetchStatus.FetchFinish;
                    isFetchSuccess = true;
                }
            }
        }

        // public void OnFetched()
        // {
        //     remoteConfigData.SetDefaultValue();
        //     if (remoteConfigData.isGetRemoteConfig)
        //     {
        //         FetchStatus = RemoteFetchStatus.Fetching;
        //         remoteConfigModule = new FirebaseRemoteConfig(this);
        //         remoteConfigModule.Init();
        //     }
        //     else
        //     {
        //         FetchStatus = RemoteFetchStatus.FetchFinish;
        //     }
        // }

        public void OnRemoteConfigFetched(bool isSuccess)
        {
            if (isSuccess)
            {
                foreach (var remoteConfigEntry in remoteConfigData.RemoteConfigList)
                {
                    switch (remoteConfigEntry.type)
                    {
                        case RemoteConfigType.BOOL:
                            remoteConfigEntry.SetValue(GetAndTrackBool(remoteConfigEntry.key));
                            remoteConfigData.RemoteConfigDict[remoteConfigEntry.key] =
                                GetAndTrackBool(remoteConfigEntry.key);
                            break;
                        case RemoteConfigType.INT:
                            remoteConfigEntry.SetValue(GetAndTrackInt(remoteConfigEntry.key));
                            remoteConfigData.RemoteConfigDict[remoteConfigEntry.key] =
                                GetAndTrackInt(remoteConfigEntry.key);
                            break;
                        case RemoteConfigType.STRING:
                            remoteConfigEntry.SetValue(GetAndTrackString(remoteConfigEntry.key));
                            remoteConfigData.RemoteConfigDict[remoteConfigEntry.key] =
                                GetAndTrackString(remoteConfigEntry.key);
                            break;
                    }
                }
            }

            isFetchSuccess = true;
            Debug.Log("RemoteConfig Fetched " + isFetchSuccess);
//            _remoteConfigFetchedEvent.IsSuccess = isSuccess;
//            EventManager.Instance.Raise(_remoteConfigFetchedEvent);
        }

        public int GetInt(string remoteParam)
        {
            if (remoteConfigData.RemoteConfigDict.TryGetValue(remoteParam, out object entry))
            {
                return (int)entry;
            }

            Debug.LogError($"REMOTE CONFIG : cannot find parameter : {remoteParam}");
            return 0;
        }

        public bool GetBool(string remoteParam)
        {
            if (remoteConfigData.RemoteConfigDict.TryGetValue(remoteParam, out object entry))
            {
                return (bool)entry;
            }

            Debug.LogError($"REMOTE CONFIG : cannot find parameter : {remoteParam}");
            return false;
        }

        public string GetString(string remoteParam)
        {
            if (remoteConfigData.RemoteConfigDict.TryGetValue(remoteParam, out object entry))
            {
                return (string)entry;
            }

            Debug.LogError($"REMOTE CONFIG : cannot find parameter : {remoteParam}");
            return string.Empty;
        }

        private int GetAndTrackInt(string remoteParam)
        {
            int result = remoteConfigModule.GetIntValue(remoteParam);
            Debug.Log($"<color=cyan>{remoteParam} = {result}</color>");
            return result;
        }

        private string GetAndTrackString(string remoteParam)
        {
            string result = remoteConfigModule.GetStringValue(remoteParam);
            Debug.Log($"<color=cyan>{remoteParam} = {result}</color>");
            return result;
        }

        private bool GetAndTrackBool(string remoteParam)
        {
            bool result = remoteConfigModule.GetBoolValue(remoteParam);
            Debug.Log($"<color=cyan>{remoteParam} = {result}</color>");
            return result;
        }
    }

    public enum RemoteConfigType
    {
        BOOL,
        INT,
        STRING
    }

    public enum RemoteFetchStatus
    {
        NotFetch,
        Fetching,
        FetchFinish
    }
}