using System;
using System.IO;
using UnityEngine;

namespace SaveSystem
{ 
    public static class SaveManager
    {
        public static void Savebyjson(string saveFieName, object data)
        {
            string json = JsonUtility.ToJson(data);
            var path = Path.Combine(Application.persistentDataPath, saveFieName);

            try
            {
                File.WriteAllText(path, json);
                Debug.Log($"成功保存到 ： {path}");
            }
            catch (Exception e)
            {
                Debug.LogError($"Failed to save data to {path}: {e.Message}");
            }

        }

        public static T Loadbyjson<T>(string saveFieName)
        {
            var path = Path.Combine(Application.persistentDataPath, saveFieName);

            try
            {
                var json = File.ReadAllText(path);
                var data = JsonUtility.FromJson<T>(json);

                return data;
            }
            catch(Exception e)
            {
                Debug.LogError($"Failed to load data from {path}: {e.Message}");

                return default;
            }
        }

        public static void DeleteSaveFile(string saveFieName)
        {
            var path = Path.Combine(Application.persistentDataPath, saveFieName);

            try
            {
                File.Delete(path);
            }
            catch (Exception e)
            {
                Debug.LogError($"Failed to delete save file {path}: {e.Message}");
            }
        }
    }
}
