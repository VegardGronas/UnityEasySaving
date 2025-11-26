using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(SaveIdentity))]
public class SaveIdentityEditor : Editor
{
    SerializedProperty uniqueIDProp;

    private void OnEnable()
    {

    }

    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
    }
}