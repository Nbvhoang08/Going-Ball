using System.IO;
using UnityEngine;

public static class SaveSystem
{
    static readonly string SaveFolder = Path.Combine(Application.persistentDataPath, "NeiDatas");
    static readonly string FileName = "userData.json";

    public static void SavePlayer(UserData data)
    {
        if (data == null)
        {
            Debug.LogWarning("SavePlayer failed: data is null.");
            return;
        }

        try
        {
            if (!Directory.Exists(SaveFolder))
                Directory.CreateDirectory(SaveFolder);

            string json = JsonUtility.ToJson(data, true);
            string fullPath = Path.Combine(SaveFolder, FileName);
            File.WriteAllText(fullPath, json);

            Debug.Log("✅ Save successful at: " + fullPath);
        }
        catch (System.Exception e)
        {
            Debug.LogError("❌ SavePlayer failed: " + e.Message);
        }
    }

    public static UserData LoadData()
    {
        string fullPath = Path.Combine(SaveFolder, FileName);
        if (!File.Exists(fullPath))
        {
            Debug.LogWarning("⚠ LoadData failed: file not found at " + fullPath);
            return null;
        }

        try
        {
            string json = File.ReadAllText(fullPath);
            UserData data = JsonUtility.FromJson<UserData>(json);
            return data;
        }
        catch (System.Exception e)
        {
            Debug.LogError("❌ LoadData failed: " + e.Message);
            return null;
        }
    }
}
