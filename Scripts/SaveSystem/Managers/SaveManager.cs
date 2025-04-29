using System.Collections.Generic;
using System.IO;
using UnityEngine;

public static class SaveManager
{
    public static void Save(string path)
    {
        var allData = new SaveFile();

        foreach (var saveable in SaveTracker.GetAllSaveables())
        {
            ObjectSaveData obj = new ObjectSaveData
            {
                UniqueID = saveable.UniqueID,
                IsSceneObject = saveable.IsSceneObject,
                PrefabName = saveable.PrefabName,
                CustomDataJson = saveable.SaveData()
            };

            allData.Objects.Add(obj);
        }

        // Ensure the directory exists
        string directory = Path.GetDirectoryName(path);
        if (!Directory.Exists(directory))
        {
            Directory.CreateDirectory(directory);
        }

        string json = JsonUtility.ToJson(allData, true);
        File.WriteAllText(path, json);
        Debug.Log("Saved to " + path);
    }

    public static SaveFile Load(string path)
    {
        if (!File.Exists(path))
        {
            Debug.LogError("Save file not found: " + path);
            return null;
        }

        string json = File.ReadAllText(path);
        SaveFile data = JsonUtility.FromJson<SaveFile>(json);
        LoadManager.Load(data);
        return data;
    }
}

[System.Serializable]
public class SaveFile
{
    public List<ObjectSaveData> Objects = new List<ObjectSaveData>();
}

[System.Serializable]
public class ObjectSaveData
{
    public string UniqueID;
    public bool IsSceneObject;
    public string PrefabName;
    public string CustomDataJson;
}