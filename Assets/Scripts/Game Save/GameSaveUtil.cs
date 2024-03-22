using Newtonsoft.Json;
using System.IO;
using UnityEngine;

public class GameSaveUtil
{
    public void WriteSave(GameSave gameSave, string saveName)
    {
        string filePath = Path.Combine(Application.persistentDataPath, saveName);
        
        using (StreamWriter file = File.CreateText(filePath))
        {
            using (JsonTextWriter writer = new JsonTextWriter(file))
            {
                JsonSerializer serializer = new();
                serializer.Serialize(writer, gameSave);
            }
        }
    }

    //public GameSave LoadSave(string directoryPath, string saveName)
    //{

    //}
}
