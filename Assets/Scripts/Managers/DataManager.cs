using System.IO;
using Newtonsoft.Json;
using UnityEngine;

public class DataManager : MonoBehaviour
{
    // 文件存储路径
    private string jsonFolder;
    private string jsonFileName = "map.json";

    public static DataManager instance = null;
    
    private void Awake() 
    {
        instance = this;

        // 定义地图序列化的路径
        jsonFolder = Application.persistentDataPath + "/save/";
    }

    /// <summary>
    /// 将 mapLayout 序列化成字符串，并写入到文件中
    /// </summary>
    public void SaveToFile(MapLayoutSO mapLayout)
    {
        var filePath = jsonFolder + jsonFileName;
        var jsonData = JsonConvert.SerializeObject(mapLayout);
        if (!File.Exists(filePath))
        {
            Directory.CreateDirectory(jsonFolder);
        }
        File.WriteAllText(filePath, jsonData);
    }

    /// <summary>
    /// 从文件中读取数据，并写入到 mapLayout
    /// </summary>
    public void LoadFromFile(MapLayoutSO mapLayout)
    {
        // 首先把 mapLayout 清空一下
        mapLayout.mapRoomDataList.Clear();
        mapLayout.linePositionList.Clear();

        // 读取 JSON
        var filePath = jsonFolder + jsonFileName;
        if (File.Exists(filePath))
        {
            var stringData = File.ReadAllText(filePath);
            JsonConvert.PopulateObject(stringData, mapLayout);
        }
    }
}
