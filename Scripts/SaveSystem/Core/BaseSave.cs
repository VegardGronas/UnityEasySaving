using UnityEngine;

public abstract class BaseSave : MonoBehaviour
{
    public bool IsSceneObject = true;
    public string UniqueID = System.Guid.NewGuid().ToString();
    public string PrefabName;

    protected virtual void Start()
    {
        SaveTracker.Register(this);
    }

    protected virtual void OnDestroy()
    {
        SaveTracker.Unregister(this);
    }

    public abstract string SaveData();
    public abstract void LoadData(string json);
}