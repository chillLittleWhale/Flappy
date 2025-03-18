using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using UnityEngine;

namespace AjaxNguyen.Utility.Service
{
    public class JsonDataService
    {
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

        public bool Save<T>(T data, string name, bool overwrite = true)
        {
            string path = GetPathToFile(name);

            if (!overwrite && File.Exists(path))
            {
                Debug.LogWarning($"The file '{name}.{fileExtension}' already exists and cannot be overwritten.");
                return false;
            }

            File.WriteAllText(path, JsonConvert.SerializeObject(data, Formatting.Indented));
            return true;
        }

        public T Load<T>(string name) //where T : new()
        {
            string path = GetPathToFile(name);

            if (!File.Exists(path))
            {
                TextAsset defaultData = Resources.Load<TextAsset>("Default" + name);
                string defaultDataJson = defaultData.text;
                File.WriteAllText(path, defaultDataJson);
                Debug.LogWarning($"GameData with name '{name}' does not exist, create default version of it.");
                // return default;//new T();
            }

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