using UnityEngine;

public class SaveIdentity : MonoBehaviour
{
    public string UniqueID = System.Guid.NewGuid().ToString();
    public bool IsSceneObject = true;
    public string PrefabName;
}