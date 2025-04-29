using System.IO;
using System.Collections.Generic;
using UnityEngine;

public static class SaveProfileManager
{
    private static string activeProfile = "Default";
    private static readonly string basePath = Path.Combine(Application.persistentDataPath, "Saves");

    public static void SetActiveProfile(string profileName)
    {
        activeProfile = profileName;
    }

    public static string GetActiveProfileName() => activeProfile;

    public static string GetActiveProfilePath()
    {
        string path = Path.Combine(basePath, activeProfile);
        if (!Directory.Exists(path))
        {
            Directory.CreateDirectory(path);
        }
        return path;
    }

    public static string GetSaveFilePath() => Path.Combine(GetActiveProfilePath(), "save.json");

    public static List<string> ListProfiles()
    {
        if (!Directory.Exists(basePath))
            return new List<string>();

        List<string> profiles = new();
        foreach (var dir in Directory.GetDirectories(basePath))
        {
            profiles.Add(Path.GetFileName(dir));
        }
        return profiles;
    }

    public static void DeleteProfile(string profileName)
    {
        string path = Path.Combine(basePath, profileName);
        if (Directory.Exists(path))
        {
            Directory.Delete(path, true);
        }

        // Optional: fallback to Default if active profile was deleted
        if (activeProfile == profileName)
        {
            activeProfile = "Default";
        }
    }
}
