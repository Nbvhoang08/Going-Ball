using UnityEngine;
using Hapiga.Core.Runtime.Extensions;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace QuickEngine.Core
{
    public partial class Q
    {
        private static string _PATH = "Packages/com.Hapiga.ui/Editor/Quick";
        public static string PATH
        {
            get
            {
                if(_PATH.IsNullOrEmpty())
                {
                    _PATH = IO.File.GetRelativeDirectoryPath("Quick");
                }
                return _PATH;
            }
        }

        public static string QUICK_EDITOR_PATH { get { return PATH +"/Editor/"; } }
        public static string QUICK_ENGINE_PATH { get { return PATH + "/Engine/"; } }

        public static T GetResource<T>(string resourcesPath, string fileName) where T : ScriptableObject
        {
            return (T)Resources.Load(resourcesPath + fileName, typeof(T));
        }

#if UNITY_EDITOR
        public static T CreateAsset<T>(string relativePath, string fileName, string extension = ".asset") where T : ScriptableObject
        {
            T asset = ScriptableObject.CreateInstance<T>();
            AssetDatabase.CreateAsset(asset, relativePath + fileName + extension);
            EditorUtility.SetDirty(asset);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
            return asset;
        }

        public static T CreateAsset<T>(string relativePath, string fileName, string extension, bool saveAssetDatabase = true, bool refreshAssetDatabase = true) where T : ScriptableObject
        {
            T asset = ScriptableObject.CreateInstance<T>();
            AssetDatabase.CreateAsset(asset, relativePath + fileName + extension);
            EditorUtility.SetDirty(asset);
            if(saveAssetDatabase) { AssetDatabase.SaveAssets(); }
            if(refreshAssetDatabase) { AssetDatabase.Refresh(); }
            return asset;
        }

        public static void MoveAssetToTrash(string relativePath, string fileName, bool saveAssetDatabase = true, bool refreshAssetDatabase = true, bool printDebugMessage = true)
        {
            if(relativePath.IsNullOrEmpty()) { return; }
            if(fileName.IsNullOrEmpty()) { return; }
            if(AssetDatabase.MoveAssetToTrash(relativePath + fileName + ".asset"))
            {
                if(printDebugMessage) { Debug.Log("The " + fileName + ".asset file has been moved to trash."); }
                if(saveAssetDatabase) { AssetDatabase.SaveAssets(); }
                if(refreshAssetDatabase) { AssetDatabase.Refresh(); }
            }
        }

        public static Texture GetTexture(string filePath, string fileName, string fileExtension = ".png")
        {
            return AssetDatabase.LoadAssetAtPath<Texture>(filePath + fileName + fileExtension);
        }
#endif

    }
}
