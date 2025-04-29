using UnityEngine;

[RequireComponent(typeof(SaveIdentity))]
public abstract class BaseSave : MonoBehaviour
{
    protected SaveIdentity identity;

    protected virtual void Awake()
    {
        identity = GetComponent<SaveIdentity>();
        if (identity == null)
        {
            Debug.LogError("SaveIdentity missing on GameObject with BaseSave!");
        }
    }

    protected virtual void Start()
    {
        SaveTracker.Register(this);
    }

    protected virtual void OnDestroy()
    {
        SaveTracker.Unregister(this);
    }

    // Use properties to allow setting values if needed during loading
    public string UniqueID
    {
        get => identity?.UniqueID;
        set
        {
            if (identity != null)
            {
                identity.UniqueID = value;
            }
        }
    }

    public bool IsSceneObject
    {
        get => identity?.IsSceneObject ?? true;
        set
        {
            if (identity != null)
            {
                identity.IsSceneObject = value;
            }
        }
    }

    public string PrefabName
    {
        get => identity?.PrefabName;
        set
        {
            if (identity != null)
            {
                identity.PrefabName = value;
            }
        }
    }

    public abstract string SaveData();
    public abstract void LoadData(string json);
}