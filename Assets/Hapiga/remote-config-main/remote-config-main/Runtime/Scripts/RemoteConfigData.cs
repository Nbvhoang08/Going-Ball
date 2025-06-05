using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using Hapiga.Core.Runtime.Extensions;
using UnityEditor;
#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#endif

namespace Hapiga.RemoteConfig
{
    [CreateAssetMenu(fileName = "RemoteConfigData", menuName = "HapigaPackages/RemoteConfigData", order = 1)]
    public class RemoteConfigData : ScriptableObject
    {
        public bool isGetRemoteConfig;

        [Space]
#if ODIN_INSPECTOR
        [ValidateInput("IsValidRemoteConfigList", "", InfoMessageType.Error)]
        [TableList(AlwaysExpanded = true, ShowIndexLabels = true)]
        [Searchable]
#endif
        public List<RemoteConfigEntry> RemoteConfigList;

        public Dictionary<string, object> RemoteConfigDict;
        private static string defaultAssetFolder = "HapigaPackages/RemoteConfig";

        public static RemoteConfigData Instance
        {
            get => GetInstance();
        }

        private static RemoteConfigData GetInstance()
        {
            #if UNITY_EDITOR
            RemoteConfigData value =
                AssetDatabase.LoadAssetAtPath<RemoteConfigData>(
                    $"Assets/{defaultAssetFolder}/RemoteConfigData.asset");
            if (value == null)
            {
                value = CreateInstance<RemoteConfigData>();
                if (!Directory.Exists($"{Application.dataPath}/{defaultAssetFolder}"))
                {
                    Directory.CreateDirectory($"{Application.dataPath}/{defaultAssetFolder}");
                }

                AssetDatabase.CreateAsset(value, $"Assets/{defaultAssetFolder}/RemoteConfigData.asset");
            }

            return value;
            #endif
            return null;
        }

        private bool IsValidRemoteConfigList(List<RemoteConfigEntry> _list, ref string defaulMess)
        {
            if (_list.IsNullOrEmpty())
            {
                return true;
            }

            int index = 0;
            HashSet<string> names = new HashSet<string>();
            foreach (RemoteConfigEntry remoteConfigEntry in _list)
            {
                if (remoteConfigEntry.key.IsNullOrWhitespace() || remoteConfigEntry.defaultValue.IsNullOrWhitespace())
                {
                    defaulMess = $"element {index} : name or default value MUST NOT EMPTY";
                    return false;
                }

                if (!remoteConfigEntry.IsValidDefaultValue())
                {
                    defaulMess = $"element {index} : default value IS INVALID";
                    return false;
                }

                if (names.Contains(remoteConfigEntry.key))
                {
                    defaulMess = $"element {index} : Key is already existed";
                    return false;
                }

                names.Add(remoteConfigEntry.key);
                index++;
            }

            defaulMess = string.Empty;
            return true;
        }

        public void SetDefaultValue()
        {
            RemoteConfigDict = new Dictionary<string, object>(RemoteConfigList.Count);
            foreach (RemoteConfigEntry remoteConfigEntry in RemoteConfigList)
            {
                switch (remoteConfigEntry.type)
                {
                    case RemoteConfigType.BOOL:
                        if (bool.TryParse(remoteConfigEntry.defaultValue.Trim().ToLower(), out bool boolResult))
                        {
                            remoteConfigEntry.SetValue(boolResult);
                            RemoteConfigDict.Add(remoteConfigEntry.key, boolResult);
                        }

                        break;
                    case RemoteConfigType.INT:
                        if (int.TryParse(remoteConfigEntry.defaultValue.Trim().ToLower(), out int intResult))
                        {
                            remoteConfigEntry.SetValue(intResult);
                            RemoteConfigDict.Add(remoteConfigEntry.key, intResult);
                        }

                        break;
                    case RemoteConfigType.STRING:
                        remoteConfigEntry.SetValue(remoteConfigEntry.defaultValue.Trim());
                        RemoteConfigDict.Add(remoteConfigEntry.key, remoteConfigEntry.defaultValue.Trim());
                        break;
                }
            }
        }
    }

    [Serializable]
    public class RemoteConfigEntry
    {
        public string key;
        public RemoteConfigType type;
        public string defaultValue;
        public string value;

        public bool IsValidDefaultValue()
        {
            switch (type)
            {
                case RemoteConfigType.BOOL:
                    if (defaultValue.IsNullOrWhitespace() ||
                        !bool.TryParse(defaultValue.Trim().ToLower(), out bool boolResult))
                    {
                        return false;
                    }

                    break;
                case RemoteConfigType.INT:
                    if (defaultValue.IsNullOrWhitespace() ||
                        !int.TryParse(defaultValue.Trim().ToLower(), out int result))
                    {
                        return false;
                    }

                    break;
                case RemoteConfigType.STRING:
                    break;
            }

            return true;
        }

        public void SetValue(bool boolValue)
        {
            value = boolValue.ToString();
        }

        public void SetValue(int intValue)
        {
            value = intValue.ToString();
        }

        public void SetValue(string stringValue)
        {
            value = stringValue;
        }
    }
}