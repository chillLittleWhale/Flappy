using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using UnityEngine;

namespace AjaxNguyen.Core.Service
{
    public class JsonDataService
    {
        JsonSerializer serializer = new();
        string dataPath;
        string fileExtension = "json";

        public JsonDataService()
        {
            dataPath = Application.persistentDataPath;
        }

        string GetPathToFile(string fileName)
        {
            return Path.Combine(dataPath, string.Concat(fileName, ".", fileExtension));
        }

        // public void Save(GameData data, bool overwrite = true)
        // {
        //     string fileLocation = GetPathToFile(data.fileName);

        //     if (!overwrite && File.Exists(fileLocation))
        //     {
        //         throw new IOException($"The file '{data.fileName}.{fileExtension}' already exists and cannot be overwritten.");
        //     }

        //     File.WriteAllText(fileLocation, serializer.Serialize(data));
        // }

        // public GameData Load(string name)
        // {
        //     string fileLocation = GetPathToFile(name);

        //     if (!File.Exists(fileLocation))
        //     {
        //         Debug.Log($"No existed GameData with name '{name}', create new one.");
        //         return new GameData();
        //     }

        //     return serializer.Deserialize<GameData>(File.ReadAllText(fileLocation));
        // }

        public bool Save<T>(T data, string name, bool overwrite = true)
        {
            string path = GetPathToFile(name);

            if (!overwrite && File.Exists(path))
            {
                Debug.LogWarning($"The file '{name}.{fileExtension}' already exists and cannot be overwritten.");
                return false;
            }

            // File.WriteAllText(fileLocation, serializer.Serialize(data));
            File.WriteAllText(path, JsonConvert.SerializeObject(data, Formatting.Indented)); // testing
            return true;
        }

        public T Load<T>(string name) //where T : new()
        {
            string path = GetPathToFile(name);

            if (!File.Exists(path))
            {
                Debug.LogWarning($"No existed GameData with name '{name}', create new one."); //TODO: 
                return default;//new T();
            }

            // return serializer.Deserialize<T>(File.ReadAllText(fileLocation));
            try
            {
            return JsonConvert.DeserializeObject<T>(File.ReadAllText(path));
            }
            catch (Exception ex)
            {
                Debug.LogWarning($"Load JsonData Fail due to: {ex.Message} {ex.StackTrace}");
                return default;
            }
        }

        public void Delete(string name)
        {
            string fileLocation = GetPathToFile(name);

            if (File.Exists(fileLocation))
            {
                File.Delete(fileLocation);
            }
        }

        public void DeleteAll()
        {
            foreach (string filePath in Directory.GetFiles(dataPath))
            {
                File.Delete(filePath);
            }
        }

        public IEnumerable<string> ListSaves()
        {
            foreach (string path in Directory.EnumerateFiles(dataPath))
            {
                if (Path.GetExtension(path) == fileExtension)
                {
                    yield return Path.GetFileNameWithoutExtension(path);
                }
            }
        }
    }
}