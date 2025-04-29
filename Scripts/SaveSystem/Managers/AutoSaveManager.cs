using UnityEngine;

public class AutoSaveManager : MonoBehaviour
{
    public static AutoSaveManager Instance { get; private set; }

    public float saveInterval = 60f; 
    public bool autosaveEnabled = true;

    private float timer;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Update()
    {
        if (!autosaveEnabled) return;

        timer += Time.deltaTime;
        if (timer >= saveInterval)
        {
            SaveManager.Save(SaveProfileManager.GetSaveFilePath());
            Debug.Log("Autosaved!");
            timer = 0f;
        }
    }
}