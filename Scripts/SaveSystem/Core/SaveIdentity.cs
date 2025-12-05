using System;
using UnityEditor;
using UnityEngine;

public class SaveIdentity : MonoBehaviour
{
    [SerializeField] private string uniqueID = "ID";
    public string UniqueID => uniqueID;
    public bool IsSceneObject = true;
    public string PrefabName;

    private void OnEnable()
    {
        if(!IsSceneObject)
        {
            SetID();
        }
    }

    public void SetID()
    {
        if (uniqueID == "" || uniqueID == "ID")
        {
            Debug.Log("Genrated new ID");
            uniqueID = Guid.NewGuid().ToString();
            return;
        }
    }

    public void SetID(string uniqueID)
    {
        this.uniqueID = uniqueID;
    }

    public void UpdateID()
    {
        Debug.Log("ID Updated + " + gameObject.name);
        uniqueID = Guid.NewGuid().ToString();
    }

    private void OnValidate()
    {
#if UNITY_EDITOR
        if (IsPrefabAsset(gameObject))
            return;
#endif

        SetID();
    }

#if UNITY_EDITOR
    private bool IsPrefabAsset(GameObject obj)
    {
        var status = PrefabUtility.GetPrefabAssetType(obj);
        return status != PrefabAssetType.NotAPrefab &&
               PrefabUtility.GetPrefabInstanceStatus(obj) == PrefabInstanceStatus.NotAPrefab;
    }
#endif
}