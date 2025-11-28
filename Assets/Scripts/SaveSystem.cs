using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public static class SaveSystem
{
    private static readonly string savePath = Application.persistentDataPath + "/savefile.json";

    public static void Save(SaveData data)
    {
        string json = JsonUtility.ToJson(data, true);
        File.WriteAllText(savePath, json);
        Debug.Log("Game Saved: " + savePath);
    }

    public static SaveData Load()
    {
        if (!File.Exists(savePath))
        {
            Debug.LogWarning("Save file not found, creating new data.");
            return null;
        }

        string json = File.ReadAllText(savePath);

        if (string.IsNullOrEmpty(json) || json.Length < 5)
        {
            Debug.LogWarning("Save file is empty or invalid, ignoring load.");
            return null;
        }

        try
        {
            return JsonUtility.FromJson<SaveData>(json);
        }
        catch (System.Exception e)
        {
            Debug.LogError("Error loading save file: " + e.Message);
            return null;
        }
    }
}


