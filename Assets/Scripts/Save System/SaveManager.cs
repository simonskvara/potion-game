using System;
using System.IO;
using UnityEngine;

public static class SaveManager
{
    public static event Action OnSaved;
    public static event Action OnLoaded;
    public static event Action OnSaveDeleted;

    private const string FileName = "SaveData.json";
    public static string FilePath => Path.Combine(Application.persistentDataPath, FileName);
    private static SaveData data;

    public static SaveData Data
    {
        get
        {
            EnsureLoaded();
            return data;
        }
    }

    /// <summary>
    /// Writes and saves current SaveData
    /// </summary>
    public static void Save()
    {
        EnsureLoaded();

        try
        {
            File.WriteAllText(FilePath, JsonUtility.ToJson(data, true));
            Debug.Log("Saved game");
        }
        catch (Exception e)
        {
            Debug.LogError($"Failed to write a save file at {FilePath}: {e.Message}");
            return; // don't fire OnSaved if the write actually failed
        }

        try
        {
            OnSaved?.Invoke();
        }
        catch (Exception e)
        {
            Debug.LogError($"A save callback (OnSaved) threw after a successful write: {e}");
        }
    }

    public static void Load()
    {
        if (File.Exists(FilePath))
        {
            try
            {
                data = JsonUtility.FromJson<SaveData>(File.ReadAllText(FilePath));

                // migration here if need (if data.version < currentVersion)
            }
            catch (Exception e)
            {
                Debug.LogError($"Failed to Load save file: {e.Message}");
                data = new SaveData();
            }
        }
        else
        {
            data = new SaveData();
        }
        OnLoaded?.Invoke();
    }

    /// <summary>
    /// Delete the save file to restart game progress
    /// </summary>
    public static void DeleteSave()
    {
        if (File.Exists(FilePath))
        {
            File.Delete(FilePath);
            Debug.Log("Save Deleted");
        }
        else
        {
            Debug.Log("No save file found to delete");
        }

        data = new SaveData();

        OnSaveDeleted?.Invoke();
    }

    private static void EnsureLoaded()
    {
        if (data == null)
            Load();
    }
}
