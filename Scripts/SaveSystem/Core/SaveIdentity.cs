using System.Xml;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

public class SaveIdentity : MonoBehaviour
{
    [SerializeField] private string uniqueID = "ID";
    public string UniqueID => uniqueID;
    public bool IsSceneObject = true;
    public string PrefabName;

    private void SetID()
    {
        if (uniqueID == "")
        {
            Debug.Log("Genrated new ID");
            uniqueID = GUID.Generate().ToString();
            return;
        }

        Debug.Log("ID already set: " + uniqueID);
    }

    public void UpdateID()
    {
        Debug.Log("ID Updated");
        uniqueID = GUID.Generate().ToString();
    }


    private void OnValidate()
    {
#if UNITY_EDITOR
        if (IsPrefabAsset(gameObject))
            return;
#endif

        SetID();
    }

    private bool IsPrefabAsset(GameObject obj)
    {
        var status = PrefabUtility.GetPrefabAssetType(obj);
        return status != PrefabAssetType.NotAPrefab &&
               PrefabUtility.GetPrefabInstanceStatus(obj) == PrefabInstanceStatus.NotAPrefab;
    }
}