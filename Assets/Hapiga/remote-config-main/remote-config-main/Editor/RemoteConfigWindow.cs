using UnityEditor;
#if ODIN_INSPECTOR
using Sirenix.OdinInspector.Editor;
using Sirenix.Utilities;
using Sirenix.Utilities.Editor;
#endif

namespace Hapiga.RemoteConfig.Editor
{
#if ODIN_INSPECTOR
    public class RemoteConfigWindow : OdinMenuEditorWindow
    {
        private static RemoteConfigData config;

        [MenuItem("Hapiga Game/Remote Config #&r")]
        private static void OpenWindow()
        {
            var window = GetWindow<RemoteConfigWindow>();
            window.position = GUIHelper.GetEditorWindowRect().AlignCenter(1000, 600);
        }
        protected override OdinMenuTree BuildMenuTree()
        {
            var  tree = new OdinMenuTree(true);
            var customMenuStyle = new OdinMenuStyle
            {
                BorderPadding = 0f,
                AlignTriangleLeft = true,
                TriangleSize = 16f,
                TrianglePadding = 0f,
                Offset = 20f,
                Height = 23,
                IconPadding = 0f,
                BorderAlpha = 0.323f
            };
            tree.DefaultMenuStyle = customMenuStyle;
            tree.Config.DrawSearchToolbar = true;
            config = RemoteConfigData.Instance;
            tree.AddObjectAtPath("Data Config", config);
            return tree;
        }
    }
#else
    public class RemoteConfigWindow 
    {
        [MenuItem("Hapiga Game/Remote Config #&r")]
        static void OpenWarningPanel()
        {
            EditorUtility.DisplayDialog("Install Odin", "Please Install Odin Package to use Remote Config Window", "OK");
        }
    }
#endif
}