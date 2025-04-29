using System;
using UnityEngine;

public class SaveTransform : BaseSave
{
    public override string SaveData()
    {
        TransformData data = new(transform.position, transform.rotation);
        string json = JsonUtility.ToJson(data);
        return json;
    }

    public override void LoadData(string json)
    {
        TransformData data = JsonUtility.FromJson<TransformData>(json);
        transform.position = data.position;
        transform.rotation = data.rotation;
    }
}

[Serializable]
public class TransformData
{
    public Vector3 position;
    public Quaternion rotation;

    public TransformData(Vector3 pos, Quaternion rot)
    {
        position = pos;
        rotation = rot;
    }
}