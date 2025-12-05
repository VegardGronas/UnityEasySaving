using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class LoadManager
{
    public static void Load(SaveFile saveFile)
    {
        Debug.Log("Loading: " + saveFile.Objects.Count + " Objects");

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
            BaseSave[] newSaves = instance.GetComponents<BaseSave>();

            if (newSaves.Length <= 0)
            {
                Debug.LogError("Instantiated prefab does not have BaseSave: " + objData.PrefabName);
                GameObject.Destroy(instance);
                continue;
            }

            foreach (BaseSave save in newSaves)
            {
                // Assign proper ID for runtime object
                save.IsSceneObject = false;
                save.UniqueID = objData.UniqueID;

                // Load data
                save.LoadData(objData.CustomDataJson);
            }
        }
    }

    public static IEnumerator LoadAsync(SaveFile saveFile, Action<float> onProgress = null, int batchSize = 5)
    {
        int totalObjects = saveFile.Objects.Count;
        int processed = 0;

        foreach (var objData in saveFile.Objects)
        {
            // --------------------------
            // 1. Scene objects
            // --------------------------
            if (objData.IsSceneObject)
            {
                BaseSave existing = SaveTracker
                    .GetAllSaveables()
                    .FirstOrDefault(s => s.UniqueID == objData.UniqueID);

                if (existing != null)
                    existing.LoadData(objData.CustomDataJson);
                else
                    Debug.LogWarning($"Scene object with ID {objData.UniqueID} not found in scene.");

                processed++;
                if (processed % batchSize == 0)
                {
                    onProgress?.Invoke((float)processed / totalObjects);
                    yield return null; // allow frame to update
                }

                continue;
            }

            // --------------------------
            // 2. Runtime objects
            // --------------------------
            BaseSave existingRuntime = SaveTracker
                .GetAllSaveables()
                .FirstOrDefault(s => s.UniqueID == objData.UniqueID);

            if (existingRuntime != null)
            {
                existingRuntime.LoadData(objData.CustomDataJson);
                processed++;
                if (processed % batchSize == 0)
                {
                    onProgress?.Invoke((float)processed / totalObjects);
                    yield return null;
                }
                continue;
            }

            GameObject prefab = Resources.Load<GameObject>(objData.PrefabName);
            if (prefab == null)
            {
                Debug.LogError("Prefab not found in Resources: " + objData.PrefabName);
                processed++;
                if (processed % batchSize == 0)
                {
                    onProgress?.Invoke((float)processed / totalObjects);
                    yield return null;
                }
                continue;
            }

            GameObject instance = GameObject.Instantiate(prefab);
            BaseSave[] newSaves = instance.GetComponents<BaseSave>();

            if (newSaves.Length <= 0)
            {
                Debug.LogError("Instantiated prefab does not have BaseSave: " + objData.PrefabName);
                GameObject.Destroy(instance);
                processed++;
                if (processed % batchSize == 0)
                {
                    onProgress?.Invoke((float)processed / totalObjects);
                    yield return null;
                }
                continue;
            }

            foreach (BaseSave save in newSaves)
            {
                // Assign proper ID for runtime object
                save.IsSceneObject = false;
                save.UniqueID = objData.UniqueID;

                // Load data
                save.LoadData(objData.CustomDataJson);
            }

            processed++;
            if (processed % batchSize == 0)
            {
                onProgress?.Invoke((float)processed / totalObjects);
                yield return null; // yield control to allow frame to render
            }
        }

        // Final progress update
        onProgress?.Invoke(1f);
    }
}