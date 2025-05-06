using System;
using UnityEngine;

public class SaveTransform : BaseSave
{
    public override string SaveData()
    {
        TransformData data = new(transform.position, transform.rotation, transform.localScale);
        string json = JsonUtility.ToJson(data);
        return json;
    }

    public override void LoadData(string json)
    {
        TransformData data = JsonUtility.FromJson<TransformData>(json);
        transform.position = data.position;
        transform.rotation = data.rotation;
        transform.localScale = data.scale;
    }
}

[Serializable]
public class TransformData
{
    public Vector3 position;
    public Quaternion rotation;
    public Vector3 scale;

    public TransformData(Vector3 pos, Quaternion rot, Vector3 scale)
    {
        position = pos;
        rotation = rot;
        this.scale = scale;
    }
}