using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Hapiga.RemoteConfig.Editor
{
    public static class RemoteConfigModuleMenu
    {
        [MenuItem("GameObject/Hapiga Package/RemoteConfigManager", false, 0)]
        public static void AddAdManager()
        {
            Object remoteConfigManager = AssetDatabase.LoadAssetAtPath<Object>("Packages/com.hapiga.remoteconfig/Runtime/Prefabs/RemoteConfigManager.prefab");
            if (remoteConfigManager != null)
            {
                PrefabUtility.InstantiatePrefab(remoteConfigManager);
            }
            else
            {
                Debug.LogError("Cannot find AdManager prefab");
            }
        }
    }
}