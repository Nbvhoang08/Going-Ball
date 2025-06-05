using UnityEditor;
using UnityEngine;

namespace Hapiga.Ads.Editor
{
    public class AdsEditorMenu
    {
        [MenuItem("GameObject/Hapiga Package/AdManager", false, 0)]
        public static void AddAdManager()
        {
            Object adManagerPrefab = AssetDatabase.LoadAssetAtPath<Object>("Packages/com.hapiga.ads/Runtime/Prefabs/AdManager.prefab");
            if (adManagerPrefab != null)
            {
                PrefabUtility.InstantiatePrefab(adManagerPrefab);
            }
            else
            {
                Debug.LogError("Cannot find AdManager prefab");
            }
        }
    }
}