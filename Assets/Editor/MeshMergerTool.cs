using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using UnityEditor.Formats.Fbx.Exporter;
using static Dreamteck.Splines.SplineMesh.Channel.MeshDefinition;

//#if UNITY_2020_3_OR_NEWER
//using UnityEditor.Formats.Fbx.Exporter;
//#endif

public class MeshMergerTool : EditorWindow
{
    [MenuItem("Tools/Mesh Merger Tool")]
    static void Init()
    {
        MeshMergerTool window = (MeshMergerTool)GetWindow(typeof(MeshMergerTool));
        window.titleContent = new GUIContent("Mesh Merger");
        window.Show();
    }

    private GameObject rootObject;
    private bool includeInactive = false;
    private bool saveAsAsset = false;
    private string assetName = "MergedMesh.asset";

    private enum ExportFormat { None, OBJ, FBX }
    private ExportFormat exportFormat = ExportFormat.None;

    void OnGUI()
    {
        GUILayout.Label("Merge Mesh Tool", EditorStyles.boldLabel);

        rootObject = (GameObject)EditorGUILayout.ObjectField("Root Object", rootObject, typeof(GameObject), true);
        includeInactive = EditorGUILayout.Toggle("Include Inactive", includeInactive);
        saveAsAsset = EditorGUILayout.Toggle("Save Merged Mesh", saveAsAsset);
        if (saveAsAsset)
        {
            assetName = EditorGUILayout.TextField("Asset Name", assetName);
        }

        exportFormat = (ExportFormat)EditorGUILayout.EnumPopup("Export Format", exportFormat);

        if (GUILayout.Button("Merge Meshes"))
        {
            if (rootObject != null)
            {
                MergeMeshes();
            }
            else
            {
                EditorUtility.DisplayDialog("Error", "Please assign a root GameObject!", "OK");
            }
        }
    }

    void MergeMeshes()
    {
        List<MeshFilter> meshFilters = rootObject.GetComponentsInChildren<MeshFilter>(includeInactive).ToList();
        List<MeshRenderer> meshRenderers = rootObject.GetComponentsInChildren<MeshRenderer>(includeInactive).ToList();

        if (meshFilters.Count == 0)
        {
            EditorUtility.DisplayDialog("No Meshes", "No MeshFilter found under the selected object!", "OK");
            return;
        }

        Dictionary<Material, List<CombineInstance>> matToCombine = new Dictionary<Material, List<CombineInstance>>();

        for (int i = 0; i < meshFilters.Count; i++)
        {
            var mesh = meshFilters[i].sharedMesh;
            if (mesh == null) continue;

            var materials = meshRenderers[i].sharedMaterials;
            var transformMatrix = rootObject.transform.worldToLocalMatrix * meshFilters[i].transform.localToWorldMatrix;


            for (int subMesh = 0; subMesh < mesh.subMeshCount; subMesh++)
            {
                if (subMesh >= materials.Length) continue;
                var mat = materials[subMesh];

                if (!matToCombine.ContainsKey(mat))
                {
                    matToCombine[mat] = new List<CombineInstance>();
                }

                CombineInstance ci = new CombineInstance();
                ci.mesh = mesh;
                ci.subMeshIndex = subMesh;
                ci.transform = transformMatrix;
                matToCombine[mat].Add(ci);
            }
        }

        List<Material> finalMaterials = new List<Material>();
        List<CombineInstance> finalCombinations = new List<CombineInstance>();

        foreach (var kvp in matToCombine)
        {
            Mesh subMesh = new Mesh();
            subMesh.indexFormat = UnityEngine.Rendering.IndexFormat.UInt32;
            subMesh.name = "SubMesh_" + kvp.Key.name;
            subMesh.CombineMeshes(kvp.Value.ToArray(), true, true);
            finalMaterials.Add(kvp.Key);

            CombineInstance finalCI = new CombineInstance();
            finalCI.mesh = subMesh;
            finalCI.subMeshIndex = 0;
            finalCI.transform = Matrix4x4.identity;
            finalCombinations.Add(finalCI);
        }

        Mesh mergedMesh = new Mesh();
        mergedMesh.name = "Merged_Mesh";
        mergedMesh.indexFormat = UnityEngine.Rendering.IndexFormat.UInt32;
        mergedMesh.CombineMeshes(finalCombinations.ToArray(), false, false); // keep submeshes

        GameObject mergedObj = new GameObject("MergedMesh_Object");

        // Gắn vào cùng cha (nếu có) và giữ nguyên vị trí world
        mergedObj.transform.SetParent(rootObject.transform.parent, true);

        // Đặt vị trí world giống rootObject
        mergedObj.transform.position = rootObject.transform.position;
        mergedObj.transform.rotation = rootObject.transform.rotation;
        mergedObj.transform.localScale = rootObject.transform.lossyScale;



        MeshFilter mf = mergedObj.AddComponent<MeshFilter>();
        MeshRenderer mr = mergedObj.AddComponent<MeshRenderer>();

        mf.sharedMesh = mergedMesh;
        mr.sharedMaterials = finalMaterials.ToArray();

        if (saveAsAsset)
        {
            string path = "Assets/" + assetName;
            AssetDatabase.CreateAsset(mergedMesh, path);
            AssetDatabase.SaveAssets();
            Debug.Log("Mesh saved to: " + path);
        }

        if (exportFormat == ExportFormat.OBJ)
        {
            string path = EditorUtility.SaveFilePanel("Export OBJ", "", "MergedMesh.obj", "obj");
            if (!string.IsNullOrEmpty(path))
            {
                ExportToOBJ(mergedMesh, finalMaterials.ToArray(), path);
                Debug.Log("Exported OBJ to: " + path);
            }
        }
        else if (exportFormat == ExportFormat.FBX)
        {
#if UNITY_2020_3_OR_NEWER
            ExportFBXBinary(mergedObj);
#else
Debug.LogWarning("FBX Exporter requires Unity 2020.3 or newer and FBX Exporter package.");
#endif


        }

        Selection.activeGameObject = mergedObj;
        Debug.Log("Merged mesh created successfully!");
    }

    public void ExportFBXBinary(GameObject mergedObj)
    {
        // Kiểm tra nếu thư mục chưa tồn tại, tạo thư mục mới
        string folderPath = "Assets/ExportedFBX";
        if (!AssetDatabase.IsValidFolder(folderPath))
        {
            AssetDatabase.CreateFolder("Assets", "ExportedFBX");
        }

        // Chọn vị trí xuất file
        string fbxPath = EditorUtility.SaveFilePanelInProject(
            "Export FBX",
            "MergedMesh",
            "fbx",
            "Choose location to save the FBX",
            folderPath
        );

        if (!string.IsNullOrEmpty(fbxPath))
        {
            // Lưu cài đặt hiện tại
            bool currentBinarySetting = EditorPrefs.GetBool("FbxExportInAscii", false);

            // Đặt xuất sang định dạng binary (ngược với ASCII)
            EditorPrefs.SetBool("FbxExportInAscii", false);

            // Xuất tệp FBX
            ModelExporter.ExportObject(fbxPath, mergedObj);

            // Khôi phục cài đặt cũ
            EditorPrefs.SetBool("FbxExportInAscii", currentBinarySetting);

            // Cập nhật Project sau khi xuất file
            AssetDatabase.Refresh();
            Debug.Log("Exported FBX to: " + fbxPath + " in Binary format");
        }
        else
        {
            Debug.LogWarning("FBX path is empty. Export aborted.");
        }
    }
    void ExportToOBJ(Mesh mesh, Material[] materials, string filePath)
    {
        using (StreamWriter sw = new StreamWriter(filePath))
        {
            sw.WriteLine("# Exported by Mesh Merger Tool");

            foreach (Vector3 v in mesh.vertices)
            {
                sw.WriteLine($"v {v.x} {v.y} {v.z}");
            }

            foreach (Vector3 n in mesh.normals)
            {
                sw.WriteLine($"vn {n.x} {n.y} {n.z}");
            }

            foreach (Vector2 uv in mesh.uv)
            {
                sw.WriteLine($"vt {uv.x} {uv.y}");
            }

            for (int i = 0; i < mesh.subMeshCount; i++)
            {
                sw.WriteLine($"g SubMesh_{i}");
                var triangles = mesh.GetTriangles(i);
                for (int t = 0; t < triangles.Length; t += 3)
                {
                    int v1 = triangles[t] + 1;
                    int v2 = triangles[t + 1] + 1;
                    int v3 = triangles[t + 2] + 1;
                    sw.WriteLine($"f {v1}/{v1}/{v1} {v2}/{v2}/{v2} {v3}/{v3}/{v3}");
                }
            }
        }
    }
}
