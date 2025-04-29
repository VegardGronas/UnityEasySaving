# UnityEasySaving
 Modular saving system for Unity.

Modular Saving System for Unity
This is a modular saving system designed for Unity, allowing developers to easily implement flexible and customizable save/load functionality across various types of objects in their games. 
The system is built to be easily extendable, enabling quick integration of new save data types while keeping the codebase organized and scalable.

Features:
Modular & Extensible: Add new saveable data types (e.g., transforms, inventories, health) with minimal effort.

Scene Management: Supports saving and loading both in-scene objects and dynamically spawned objects.

Customizable: Easily define what data to save for each object (e.g., transform, inventory, stats).

Save Profiles: Manage multiple save profiles (coming soon).

Async Saving: Implement asynchronous saving for faster performance (coming soon).

Editor Tools: Debug and manage save data directly in Unity Editor (coming soon).

How to Use:
Add the BaseSave script to any GameObject you want to be saveable.

Create a custom class that inherits from BaseSave and implement the SaveData and LoadData methods.

Use SaveManager to save/load data, specifying the desired file path.

Example:
csharp
Copy
Edit
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

//Example of customdata that can be saved
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
Feel free to contribute or suggest new features!
If you want to make this system even better, pull requests are always welcome.