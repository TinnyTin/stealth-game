using Newtonsoft.Json;
using System.IO;
using UnityEngine;

/*
 * CS6457 Attributions
 * Tiny Brain
 * Original Author:     Tom
 * Contributors:
 */

public class GameSaveUtil
{
    public void WriteSave(GameSave gameSave, string saveName)
    {
        string filePath = Path.Combine(Application.persistentDataPath, saveName);
        using StreamWriter file = File.CreateText(filePath);
        using JsonTextWriter writer = new(file);
        JsonSerializer serializer = new();
        serializer.Serialize(writer, gameSave);
        Debug.Log($"Finished writing game save data to {filePath}");
    }

    public GameSave LoadSave(string saveName)
    {
        string filePath = Path.Combine(Application.persistentDataPath, saveName);

        try
        {
            using StreamReader file = File.OpenText(filePath);
            JsonSerializer serializer = new();
            GameSave gameSave = (GameSave)serializer.Deserialize(file, typeof(GameSave));
            return gameSave;
        }
        catch (DirectoryNotFoundException)
        {
            Debug.LogWarning($"GameSaveUtil::LoadSave: Could not find persistent data path directory: {Application.persistentDataPath}");
        }
        catch (FileNotFoundException)
        {
            Debug.LogWarning($"GameSaveUtil::LoadSave: Could not find save file at: {filePath}");
        }
        catch (JsonException ex)
        {
            Debug.LogWarning($"GameSaveUtil::LoadSave: An error occurred while deserializing {filePath}. The message is: {ex.Message}");
        }

        return null;
    }
}
