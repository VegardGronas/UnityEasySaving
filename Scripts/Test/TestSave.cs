using UnityEngine;

public class TestSave : MonoBehaviour
{
    private string savePath;

    private void Awake()
    {
        savePath = Application.persistentDataPath + "/save.json";
    }

    public void Save()
    {
        SaveManager.Save(savePath);
    }

    public void Load()
    {
        SaveFile loaded = SaveManager.Load(savePath);
    }
}