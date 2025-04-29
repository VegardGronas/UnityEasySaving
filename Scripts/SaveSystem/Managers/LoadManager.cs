using System.Collections.Generic;
using UnityEngine;

public static class LoadManager
{
    public static void Load(SaveFile saveFile)
    {
        foreach (var objData in saveFile.Objects)
        {
            if (objData.IsSceneObject)
            {
                // Look for existing object in SaveTracker
                foreach (var saveable in SaveTracker.GetAllSaveables())
                {
                    if (saveable.UniqueID == objData.UniqueID)
                    {
                        saveable.LoadData(objData.CustomDataJson);
                        break;
                    }
                }
            }
            else
            {
                // Need to instantiate from prefab
                GameObject prefab = Resources.Load<GameObject>(objData.PrefabName);
                if (prefab != null)
                {
                    GameObject instance = GameObject.Instantiate(prefab);
                    BaseSave saveable = instance.GetComponent<BaseSave>();
                    if (saveable != null)
                    {
                        saveable.UniqueID = objData.UniqueID;
                        saveable.IsSceneObject = false;
                        saveable.LoadData(objData.CustomDataJson);
                    }
                }
                else
                {
                    Debug.LogError("Prefab not found in Resources: " + objData.PrefabName);
                }
            }
        }
    }
}