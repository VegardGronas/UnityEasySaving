using System.Collections.Generic;
using UnityEngine;

public static class SaveTracker
{
    private static List<BaseSave> saveables = new List<BaseSave>();

    public static void Register(BaseSave saveable)
    {
        if (!saveables.Contains(saveable))
        {
            saveables.Add(saveable);
            foreach(BaseSave save in saveables)
            {
                if(save.UniqueID == saveable.UniqueID && save != saveable)
                {
                    Debug.LogWarning("Duplicate UniqueID detected: " + saveable.UniqueID + ". This may cause issues during saving/loading.");
                    save.UpdateID();
                }
            }
        }
    }

    public static void Unregister(BaseSave saveable)
    {
        if(saveables.Contains(saveable))
        {
            saveables.Remove(saveable);
        }
    }

    public static void Clear()
    {
        saveables.Clear();
    }

    public static List<BaseSave> GetAllSaveables()
    {
        return saveables;
    }
}
