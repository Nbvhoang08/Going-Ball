using UnityEngine;
using System;
using QuickEngine.Core;
using Hapiga.Core.Runtime.Extensions;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Hapiga.UI
{
    [Serializable]
    public partial class DUI
    {
        #region Context Menu Settings

        public const int MENU_PRIORITY_UIELEMENT = 1;
        public const string COMPONENT_MENU_UIELEMENT = "UI/UIElement";
        public const string GAMEOBJECT_MENU_UIELEMENT = "GameObject/UI/UIElement";

        public const int MENU_PRIORITY_UIPANEL = 1;
        public const string COMPONENT_MENU_UIPANEL = "UI/UIPanel";
        public const string GAMEOBJECT_MENU_UIPANEL = "GameObject/UI/UIPanel";

        public const int MENU_PRIORITY_UIBACKGROUND = 2;
        public const string COMPONENT_MENU_UIBACKGROUND = "UI/UIElementBackground";
        public const string GAMEOBJECT_MENU_UIBACKGROUND = "GameObject/UI/UIElementBackground";

        public const int MENU_PRIORITY_UIBUTTON = 3;
        public const string COMPONENT_MENU_UIBUTTON = "UI/UIButton";
        public const string GAMEOBJECT_MENU_UIBUTTON = "GameObject/UI/UIButton";

        #endregion

        public const string DEFAULT_BUTTON_NAME = "~Button Name~";

        public const string BACK_BUTTON_NAME = "Back";

        public const int GLOBAL_EDITOR_WIDTH = 420;
        public const int BAR_HEIGHT = 20;
        public const int MINI_BAR_HEIGHT = 18;
        public enum EventType { ButtonClick }

        public static string PATH = "Packages/com.Hapiga.ui/Editor"; //"Assets/Hapiga_Packages/com.Hapiga.ui/Editor";;

        public const string RESOURCES_PATH_DUIDATA = "";
        public static string RELATIVE_PATH_DUIDATA { get { return PATH + "/Resources/" + RESOURCES_PATH_DUIDATA; } }

        public const string RESOURCES_PATH_UIELEMENTS = "DUI/UIElements/";
        public const string RESOURCES_PATH_UIBUTTONS = "DUI/UIButtons/";

        public static string RELATIVE_PATH_UIELEMENTS { get { return PATH + "/Resources/" + RESOURCES_PATH_UIELEMENTS; } }
        public static string RELATIVE_PATH_UIBUTTONS { get { return PATH + "/Resources/" + RESOURCES_PATH_UIBUTTONS; } }

        public const string RESOURCES_PATH_SETTINGS = "DUI/Settings/";
        public static string RELATIVE_PATH_SETTINGS { get { return PATH + "/Resources/" + RESOURCES_PATH_SETTINGS; } }
        public const string SETTINGS_FILENAME = "DUISettings";

        public static T GetResource<T>(string resourcesPath, string fileName) where T : ScriptableObject
        {
            return (T)Resources.Load(resourcesPath + fileName, typeof(T));
        }

        private static DUISettings _DUISettings;
        public static DUISettings DUISettings
        {
            get
            {
                if (_DUISettings == null)
                {
#if UNITY_EDITOR
                    if (!AssetDatabase.IsValidFolder(DUI.PATH + "/Resources")) { AssetDatabase.CreateFolder(DUI.PATH, "Resources"); }
                    if (!AssetDatabase.IsValidFolder(DUI.PATH + "/Resources/DUI")) { AssetDatabase.CreateFolder(DUI.PATH + "/Resources", "DUI"); }
                    if (!AssetDatabase.IsValidFolder(DUI.PATH + "/Resources/DUI/Settings")) { AssetDatabase.CreateFolder(DUI.PATH + "/Resources/DUI", "Settings"); }
#endif
                    _DUISettings = Q.GetResource<DUISettings>(RESOURCES_PATH_SETTINGS, SETTINGS_FILENAME);
                }

#if UNITY_EDITOR && !dUI_SOURCE
                if (_DUISettings == null)
                {
                    _DUISettings = Q.CreateAsset<DUISettings>(RELATIVE_PATH_SETTINGS, SETTINGS_FILENAME);
                }
#endif
                return _DUISettings;
            }
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

#region UI built-in sprites
        private const string kStandardSpritePath = "UI/Skin/UISprite.psd";
        private const string kBackgroundSpriteResourcePath = "UI/Skin/Background.psd";
        private const string kInputFieldBackgroundPath = "UI/Skin/InputFieldBackground.psd";
        private const string kKnobPath = "UI/Skin/Knob.psd";
        private const string kCheckmarkPath = "UI/Skin/Checkmark.psd";

        public static Sprite UISprite { get { return AssetDatabase.GetBuiltinExtraResource<Sprite>(kStandardSpritePath); ; } }
        public static Sprite Background { get { return AssetDatabase.GetBuiltinExtraResource<Sprite>(kBackgroundSpriteResourcePath); ; } }
        public static Sprite FieldBackground { get { return AssetDatabase.GetBuiltinExtraResource<Sprite>(kInputFieldBackgroundPath); ; } }
        public static Sprite Knob { get { return AssetDatabase.GetBuiltinExtraResource<Sprite>(kKnobPath); ; } }
        public static Sprite Checkmark { get { return AssetDatabase.GetBuiltinExtraResource<Sprite>(kCheckmarkPath); ; } }
#endregion
#endif
    }
}
