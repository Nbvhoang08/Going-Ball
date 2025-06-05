using UnityEditor;
using UnityEngine;
using System.IO;

public class TextureOptimize: EditorWindow
{
    private string selectedFolder = "Assets";
    private int selectedSizeIndex = 1;
    private int selectedCompressionIndex = 0;

    private readonly int[] sizeOptions = { 2048, 1024, 512, 256, 128 };
    private readonly string[] compressionOptions = {
        "ETC2_RGB4", "ETC2_RGBA8", "ASTC_6x6", "ASTC_8x8", "PVRTC_RGB4"
    };

    [MenuItem("Tools/Optimize/Texture Optimizer PRO")]
    public static void ShowWindow()
    {
        GetWindow<TextureOptimize>("Texture Optimizer PRO");
    }

    private void OnGUI()
    {
        GUILayout.Label("🛠 Texture Optimizer PRO", EditorStyles.boldLabel);
        GUILayout.Space(5);

        // Folder selection
        EditorGUILayout.LabelField("Selected Folder:", selectedFolder);
        if (GUILayout.Button("📁 Select Folder"))
        {
            string folderPath = EditorUtility.OpenFolderPanel("Select Folder to Optimize", "Assets", "");
            if (!string.IsNullOrEmpty(folderPath))
            {
                if (folderPath.StartsWith(Application.dataPath))
                {
                    selectedFolder = "Assets" + folderPath.Substring(Application.dataPath.Length);
                }
                else
                {
                    EditorUtility.DisplayDialog("Invalid Folder", "Please select a folder inside the project Assets folder.", "OK");
                }
            }
        }

        GUILayout.Space(10);

        // Size dropdown
        EditorGUILayout.LabelField("Max Texture Size:");
        selectedSizeIndex = EditorGUILayout.Popup(selectedSizeIndex, System.Array.ConvertAll(sizeOptions, s => s.ToString()));

        GUILayout.Space(5);

        // Compression format dropdown
        EditorGUILayout.LabelField("Compression Format:");
        selectedCompressionIndex = EditorGUILayout.Popup(selectedCompressionIndex, compressionOptions);

        GUILayout.Space(10);

        if (GUILayout.Button("🚀 Optimize Textures in Folder"))
        {
            OptimizeTextures(selectedFolder, sizeOptions[selectedSizeIndex], compressionOptions[selectedCompressionIndex]);
        }
    }

    private void OptimizeTextures(string folderPath, int maxSize, string compressionFormat)
    {
        string[] guids = AssetDatabase.FindAssets("t:Texture2D", new[] { folderPath });
        int changedCount = 0;

        foreach (string guid in guids)
        {
            string path = AssetDatabase.GUIDToAssetPath(guid);
            TextureImporter importer = AssetImporter.GetAtPath(path) as TextureImporter;

            if (importer == null || importer.textureShape != TextureImporterShape.Texture2D)
                continue;

            bool isChanged = false;
            // ✅ Set filter mode về Point (no filter)
            importer.filterMode = FilterMode.Point;
            BuildTarget platform = EditorUserBuildSettings.activeBuildTarget;
          
            TextureImporterPlatformSettings platformSettings = new TextureImporterPlatformSettings();

            // ✅ Set filter mode to Point (no filter)
            //Texture2D tex = AssetDatabase.LoadAssetAtPath<Texture2D>(path);
            //if (tex != null)
            //{
            //    tex.filterMode = FilterMode.Point;
            //    EditorUtility.SetDirty(tex);
            //}

            if (platform == BuildTarget.Android)
            {
                platformSettings.name = "Android";
                platformSettings.overridden = true;
                platformSettings.maxTextureSize = maxSize;

                switch (compressionFormat)
                {
                    case "ETC2_RGB4":
                        platformSettings.format = TextureImporterFormat.ETC2_RGB4;
                        break;
                    case "ETC2_RGBA8":
                        platformSettings.format = TextureImporterFormat.ETC2_RGBA8;
                        break;
                    case "ASTC_6x6":
                        platformSettings.format = TextureImporterFormat.ASTC_6x6;
                        break;
                    case "ASTC_8x8":
                        platformSettings.format = TextureImporterFormat.ASTC_8x8;
                        break;
                    case "PVRTC_RGB4":
                        platformSettings.format = TextureImporterFormat.PVRTC_RGB4;
                        break;
                }

                isChanged = true;
            }
            else if (platform == BuildTarget.iOS)
            {
                platformSettings.name = "iPhone";
                platformSettings.overridden = true;
                platformSettings.maxTextureSize = maxSize;

                switch (compressionFormat)
                {
                    case "ASTC_6x6":
                        platformSettings.format = TextureImporterFormat.ASTC_6x6;
                        break;
                    case "ASTC_8x8":
                        platformSettings.format = TextureImporterFormat.ASTC_8x8;
                        break;
                    case "PVRTC_RGB4":
                        platformSettings.format = TextureImporterFormat.PVRTC_RGB4;
                        break;
                    default:
                        platformSettings.format = TextureImporterFormat.ASTC_6x6;
                        break;
                }

                isChanged = true;
            }

            if (isChanged)
            {
                importer.SetPlatformTextureSettings(platformSettings);
                AssetDatabase.ImportAsset(path, ImportAssetOptions.ForceUpdate);
                changedCount++;
            }
        

    }

    EditorUtility.DisplayDialog("✅ Done", $"Optimized {changedCount} textures in '{folderPath}'", "OK");
    }
}
