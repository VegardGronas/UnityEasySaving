using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class LoadManager
{
    public static void Load(SaveFile saveFile)
    {
        foreach (var objData in saveFile.Objects)
        {
            // --------------------------
            // 1. Handle scene objects
            // --------------------------
            if (objData.IsSceneObject)
            {
                BaseSave existing = SaveTracker
                    .GetAllSaveables()
                    .FirstOrDefault(s => s.UniqueID == objData.UniqueID);

                if (existing != null)
                {
                    existing.LoadData(objData.CustomDataJson);
                }
                else
                {
                    Debug.LogWarning($"Scene object with ID {objData.UniqueID} not found in scene.");
                }

                continue; // IMPORTANT!
            }

            // --------------------------
            // 2. Handle runtime objects
            // --------------------------

            // Step A: Check if object already exists
            BaseSave existingRuntime = SaveTracker
                .GetAllSaveables()
                .FirstOrDefault(s => s.UniqueID == objData.UniqueID);

            if (existingRuntime != null)
            {
                Debug.LogWarning($"Runtime object {objData.UniqueID} already exists. Loading into existing instance.");
                existingRuntime.LoadData(objData.CustomDataJson);
                continue; // DO NOT instantiate
            }

            // Step B: Instantiate prefab
            GameObject prefab = Resources.Load<GameObject>(objData.PrefabName);

            if (prefab == null)
            {
                Debug.LogError("Prefab not found in Resources: " + objData.PrefabName);
                continue;
            }

            GameObject instance = GameObject.Instantiate(prefab);
            BaseSave newSave = instance.GetComponent<BaseSave>();

            if (newSave == null)
            {
                Debug.LogError("Instantiated prefab does not have BaseSave: " + objData.PrefabName);
                GameObject.Destroy(instance);
                continue;
            }

            // Assign proper ID for runtime object
            newSave.UpdateID();
            newSave.IsSceneObject = false;

            // Load data
            newSave.LoadData(objData.CustomDataJson);
        }
    }
}