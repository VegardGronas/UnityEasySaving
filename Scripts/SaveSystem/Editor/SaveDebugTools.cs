using UnityEditor;
using UnityEngine;

public class SaveDebugTools : EditorWindow
{
    [MenuItem("Tools/Save Debug Tools")]
    public static void ShowWindow()
    {
        GetWindow<SaveDebugTools>("Save Debug Tools");
    }

    private void OnGUI()
    {
        // Directly access SaveTracker (since it's static)
        var saveables = SaveTracker.GetAllSaveables();

        if (saveables == null || saveables.Count == 0)
        {
            GUILayout.Label("No saveables found.");
        }
        else
        {
            GUILayout.Label("Tracked Saveables", EditorStyles.boldLabel);

            // Iterate through all tracked saveables
            foreach (var saveable in saveables)
            {
                if (saveable != null)
                {
                    GUILayout.BeginHorizontal();

                    // Display UniqueID and allow editing it
                    GUILayout.Label($"UniqueID: {saveable.UniqueID}");

                    // Add a button to remove it from the tracker
                    if (GUILayout.Button("Remove from Tracker"))
                    {
                        Destroy(saveable.gameObject);
                        Debug.Log($"Removed {saveable.UniqueID} from SaveTracker");
                    }

                    GUILayout.EndHorizontal();
                }
            }

            GUILayout.Space(20);

            if (GUILayout.Button("Save All"))
            {
                SaveManager.Save(Application.persistentDataPath + "/save.json");
                Debug.Log("Game saved!");
            }
        }

        if(Application.isPlaying)
        {
            if (GUILayout.Button("Load All"))
            {
                SaveFile loaded = SaveManager.Load(Application.persistentDataPath + "/save.json");
                Debug.Log("Game loaded!");
            }
        }
    }
}