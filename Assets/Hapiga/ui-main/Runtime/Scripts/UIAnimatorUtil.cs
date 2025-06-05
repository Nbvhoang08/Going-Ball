using System.Collections.Generic;
using UnityEngine;
using System;
using QuickEngine.Core;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Hapiga.UI
{
    [Serializable]
    public class UIAnimatorUtil
    {
        public const string UNCATEGORIZED_CATEGORY_NAME = "Uncategorized";
        public const string DEFAULT_PRESET_NAME = "DefaultPreset";

        public const string FOLDER_NAME_IN = "In/";
        public const string FOLDER_NAME_OUT = "Out/";
        public const string FOLDER_NAME_STATE = "State/";
        public const string FOLDER_NAME_LOOP = "Loop/";
        public const string FOLDER_NAME_PUNCH = "Punch/";

        public const string RESOURCES_PATH_ANIMATIONS = "DUI/Animations/";
        public const string RESOURCES_PATH_IN_ANIM_DATA = RESOURCES_PATH_ANIMATIONS + FOLDER_NAME_IN;
        public const string RESOURCES_PATH_OUT_ANIM_DATA = RESOURCES_PATH_ANIMATIONS + FOLDER_NAME_OUT;
        public const string RESOURCES_PATH_STATE_ANIM_DATA = RESOURCES_PATH_ANIMATIONS + FOLDER_NAME_STATE;
        public const string RESOURCES_PATH_LOOP_DATA = RESOURCES_PATH_ANIMATIONS + FOLDER_NAME_LOOP;
        public const string RESOURCES_PATH_PUNCH_DATA = RESOURCES_PATH_ANIMATIONS + FOLDER_NAME_PUNCH;

        public static string RELATIVE_PATH_ANIMATIONS { get { return DUI.PATH + "/Resources/DUI/Animations/"; } }
        public static string RELATIVE_PATH_IN_ANIM_DATA { get { return RELATIVE_PATH_ANIMATIONS + FOLDER_NAME_IN; } }
        public static string RELATIVE_PATH_OUT_ANIM_DATA { get { return RELATIVE_PATH_ANIMATIONS + FOLDER_NAME_OUT; } }

        public static T GetResource<T>(string resourcesPath, string fileName) where T : ScriptableObject
        {
            return (T)Resources.Load(resourcesPath + fileName, typeof(T));
        }

        public static Anim GetInAnim(string presetCategory, string presetName)
        {
            return Q.GetResource<AnimData>(RESOURCES_PATH_IN_ANIM_DATA + presetCategory + "/", presetName).data.Copy();
        }
        public static Anim GetOutAnim(string presetCategory, string presetName)
        {
            return Q.GetResource<AnimData>(RESOURCES_PATH_OUT_ANIM_DATA + presetCategory + "/", presetName).data.Copy();
        }
        public static Anim GetStateAnim(string presetCategory, string presetName)
        {
            return Q.GetResource<AnimData>(RESOURCES_PATH_STATE_ANIM_DATA + presetCategory + "/", presetName).data.Copy();
        }
        public static Loop GetLoop(string presetCategory, string presetName)
        {
            return Q.GetResource<LoopData>(RESOURCES_PATH_LOOP_DATA + presetCategory + "/", presetName).data.Copy();
        }
        public static Punch GetPunch(string presetCategory, string presetName)
        {
            return Q.GetResource<PunchData>(RESOURCES_PATH_PUNCH_DATA + presetCategory + "/", presetName).data.Copy();
        }

#if UNITY_EDITOR

        private static AnimData CreateAnimDataAsset(string relativePath, string presetCategory, string presetName, Anim anim)
        {
            AnimData asset = ScriptableObject.CreateInstance<AnimData>();
            asset.presetName = presetName;
            asset.presetCategory = presetCategory;
            asset.data = anim;
            if (!QuickEngine.IO.File.Exists(relativePath + presetCategory + "/"))
            {
                QuickEngine.IO.File.CreateDirectory(relativePath + presetCategory + "/");
            }
            AssetDatabase.CreateAsset(asset, relativePath + presetCategory + "/" + presetName + ".asset");
            EditorUtility.SetDirty(asset);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
            return asset;
        }


        public static AnimData CreateInAnimPreset(string presetCategory, string presetName, Anim anim)
        {
            return CreateAnimDataAsset(RELATIVE_PATH_IN_ANIM_DATA, presetCategory, presetName, anim);
        }
        public static AnimData CreateOutAnimPreset(string presetCategory, string presetName, Anim anim)
        {
            return CreateAnimDataAsset(RELATIVE_PATH_OUT_ANIM_DATA, presetCategory, presetName, anim);
        }

        private static List<string> RemoveEmptyPresetFolders(string relativePath, string[] directories)
        {
            List<string> list = new List<string>();
            list.AddRange(directories);
            bool refreshAssetsDatabase = false;
            for (int i = list.Count - 1; i >= 0; i--)
            {
                if (QuickEngine.IO.File.GetFilesNames(relativePath + list[i] + "/", "asset").Length == 0) //this is an empty folder -> delete it
                {
                    FileUtil.DeleteFileOrDirectory(relativePath + list[i]);
                    list.RemoveAt(i);
                    refreshAssetsDatabase = true;
                }
            }
            if (refreshAssetsDatabase) { AssetDatabase.Refresh(); }
            return list;
        }
#endif
    }
}
