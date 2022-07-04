using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
// using System.Runtime.Serialization.Formatters.Binary;

public static class SaveGame
{
    public static string path = "Assets/Resources/SavedData.json";
    public static GameData[] savedData;

    // Saves data about best scores from each level to JSON file
    public static void SaveData()
    {
        // Converting array to json
        string gameDataJson = JsonHelper.ToJson(savedData, true);

        // Writing to JSON file
        using StreamWriter writer = new StreamWriter(path);
        writer.Write(gameDataJson);
        writer.Close();
    }

    // Reading data from JSON file and converting to array
    public static GameData[] ReadData()
    {
        string fileContents = File.ReadAllText(path);
        GameData[] importedGameData = JsonHelper.FromJson<GameData>(fileContents);
        return importedGameData;
    }
}

// Helper class to convert JSON to array and vice versa
public static class JsonHelper
{
    // Converting JSON into array
    public static T[] FromJson<T>(string json)
    {
        Wrapper<T> wrapper = JsonUtility.FromJson<Wrapper<T>>(json);
        return wrapper.Items;
    }

    // Converting array into JSON
    public static string ToJson<T>(T[] array)
    {
        Wrapper<T> wrapper = new Wrapper<T>();
        wrapper.Items = array;
        return JsonUtility.ToJson(wrapper);
    }

    // Convertng array into JSON and making JSON look pretty
    public static string ToJson<T>(T[] array, bool prettyPrint)
    {
        Wrapper<T> wrapper = new Wrapper<T>();
        wrapper.Items = array;
        return JsonUtility.ToJson(wrapper, prettyPrint);
    }

    [System.Serializable]
    private class Wrapper<T>
    {
        public T[] Items;
    }
}
