using System;
using UnityEngine;

public class SaveUser : BaseSave
{
    [SerializeField] string userName;

    public override string SaveData()
    {
        UserData data = new(userName);
        string json = JsonUtility.ToJson(data);
        return json;
    }

    public override void LoadData(string json)
    {
        UserData data = JsonUtility.FromJson<UserData>(json);
        userName = data.userName;

        Debug.Log("Welcome user: " + userName + ": Created at: " + data.creationDate);
    }
}

[Serializable]
public class UserData
{
    public string userName;
    public string creationDate;

    public UserData(string userName)
    {
        this.userName = userName;
        this.creationDate = DateTime.Now.ToString();
    }
}