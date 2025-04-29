using UnityEditor;
using UnityEngine;

public class SaveDebugTools : EditorWindow
{
    private string newProfileName = "";

    [MenuItem("Tools/Save Debug Tools")]
    public static void ShowWindow()
    {
        GetWindow<SaveDebugTools>("Save Debug Tools");
    }

    private void OnGUI()
    {
        // --- Profile Section ---
        GUILayout.Label("Save Profile", EditorStyles.boldLabel);
        GUILayout.Label($"Active Profile: {SaveProfileManager.GetActiveProfileName()}", EditorStyles.helpBox);

        GUILayout.BeginHorizontal();
        newProfileName = GUILayout.TextField(newProfileName, GUILayout.MinWidth(100));
        if (GUILayout.Button("Set Profile") && !string.IsNullOrWhiteSpace(newProfileName))
        {
            SaveProfileManager.SetActiveProfile(newProfileName);
            Debug.Log("Active profile set to: " + newProfileName);
            newProfileName = "";
        }
        GUILayout.EndHorizontal();

        GUILayout.Space(15);

        // --- Saveables Section ---
        var saveables = SaveTracker.GetAllSaveables();
        if (saveables == null || saveables.Count == 0)
        {
            GUILayout.Label("No saveables found.");
        }
        else
        {
            GUILayout.Label("Tracked Saveables", EditorStyles.boldLabel);

            foreach (var saveable in saveables)
            {
                if (saveable != null)
                {
                    GUILayout.BeginHorizontal();

                    GUILayout.Label($"UniqueID: {saveable.UniqueID}");

                    if (GUILayout.Button("Remove from Tracker"))
                    {
                        Object.DestroyImmediate(saveable.gameObject); // Immediate because this is Editor context
                        Debug.Log($"Removed {saveable.UniqueID} from SaveTracker");
                    }

                    GUILayout.EndHorizontal();
                }
            }

            GUILayout.Space(20);

            if (GUILayout.Button("Save All"))
            {
                SaveManager.Save(SaveProfileManager.GetSaveFilePath());
                Debug.Log("Game saved!");
            }
        }

        if (Application.isPlaying)
        {
            GUILayout.Space(10);

            if (GUILayout.Button("Load All"))
            {
                SaveFile loaded = SaveManager.Load(SaveProfileManager.GetSaveFilePath());
                Debug.Log("Game loaded!");
            }
        }
    }
}