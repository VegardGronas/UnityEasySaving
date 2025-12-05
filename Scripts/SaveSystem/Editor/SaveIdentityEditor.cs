using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(SaveIdentity))]
public class SaveIdentityEditor : Editor
{
    private SaveIdentity saveIdentity;

    private void OnEnable()
    {
        saveIdentity = (SaveIdentity)target;
        UpdatePath();
    }

    public override void OnInspectorGUI()
    {
        UpdatePath();
        DrawDefaultInspector();
    }

    private void UpdatePath()
    {
        string fullPath = AssetDatabase.GetAssetPath(saveIdentity.gameObject);
        if (string.IsNullOrEmpty(fullPath))
            return;

        int index = fullPath.IndexOf("Resources/");
        if (index < 0)
            return;

        // Extract path after Resources/
        string path = fullPath.Substring(index + "Resources/".Length);

        // Remove extension (.prefab)
        path = System.IO.Path.ChangeExtension(path, null);

        // Assign the full Resources path to PrefabName
        saveIdentity.PrefabName = path;

        // Mark as dirty so inspector refreshes
        EditorUtility.SetDirty(saveIdentity);
    }
}
