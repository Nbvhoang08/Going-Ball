using UnityEditor;
using UnityEngine;

namespace Hapiga.Tracking.Editor
{
    public class TrackingEditorMenu
    {
        [MenuItem("GameObject/Hapiga Package/TrackingManager", false, 0)]
        public static void AddAdManager()
        {
            Object prefab = AssetDatabase.LoadAssetAtPath<Object>("Packages/com.hapiga.tracking/Runtime/Prefabs/TrackingManager.prefab");
            if (prefab != null)
            {
                PrefabUtility.InstantiatePrefab(prefab);
            }
            else
            {
                Debug.LogError("Cannot find TrackingManager prefab");
            }
        }
    }
}