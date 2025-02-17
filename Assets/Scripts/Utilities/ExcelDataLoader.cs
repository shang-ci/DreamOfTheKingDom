using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using ExcelDataReader;
using UnityEngine;

public class ExcelDataLoader : MonoBehaviour
{
    public static ExcelDataLoader Instance { get; private set; }

    public string filePath;
    public List<CardDataSO> cardDataList = new List<CardDataSO>();
    public MapConfigSO mapConfig;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
        filePath = Application.dataPath + "/Data/DreamOfTheKing.xlsx";
        LoadExcelData(filePath);
    }

    private void LoadExcelData(string path)
    {
        using (var stream = File.Open(path, FileMode.Open, FileAccess.Read))
        {
            using (var reader = ExcelReaderFactory.CreateReader(stream))
            {
                //避免第一行的列名被读取出错
                var configuration = new ExcelDataSetConfiguration
                {
                    ConfigureDataTable = _ => new ExcelDataTableConfiguration
                    {
                        UseHeaderRow = true // 告诉读取器第一行是列名
                    }
                };

                var result = reader.AsDataSet(configuration);

                // 读取卡牌数据
                var cardsTable = result.Tables["Cards"];

                // 打印列名以进行调试
                foreach (DataColumn column in cardsTable.Columns)
                {
                    Debug.Log($"Column: {column.ColumnName}");
                }

                foreach (DataRow row in cardsTable.Rows)
                {
                    try
                    {
                        CardDataSO cardData = ScriptableObject.CreateInstance<CardDataSO>();

                        string name = row["Name"].ToString();
                        Sprite image = LoadSprite(row["ImagePath"].ToString());
                        int cost = Convert.ToInt32(row["Cost"].ToString());
                        CardType cardType = Enum.Parse<CardType>(row["CardType"].ToString());
                        string description = row["Description"].ToString();
                        List<Effect> effects = LoadEffects(row["Effects"].ToString());
                        List<StatusEffect> statusEffects = LoadStatusEffects(row["StatusEffects"].ToString());

                        cardData.Initialize(name, image, cost, cardType, description, effects, statusEffects);
                        cardDataList.Add(cardData);

                        // 打印每一行的数据以进行调试
                        Debug.Log($"Loaded card: {name}, {image}, {cost}, {cardType}, {description}, {effects.Count} effects, {statusEffects.Count} status effects");
                    }
                    catch (Exception ex)
                    {
                        Debug.LogError($"Error reading row: {ex.Message}");
                    }
                }

                // 读取地图配置数据
                var mapConfigTable = result.Tables["MapConfig"];

                foreach (DataColumn column in mapConfigTable.Columns)
                {
                    Debug.Log($"Column: {column.ColumnName}");
                }

                List<RoomBlueprint> roomBlueprints = new List<RoomBlueprint>();
                foreach (DataRow row in mapConfigTable.Rows)
                {
                    int min = Convert.ToInt32(row["Min"].ToString());
                    int max = Convert.ToInt32(row["Max"].ToString());
                    RoomType roomType = Enum.Parse<RoomType>(row["RoomType"].ToString());//将Excel中的字符串转换为对应的枚举类型

                    RoomBlueprint blueprint = new RoomBlueprint
                    {
                        min = min,
                        max = max,
                        roomType = roomType
                    };
                    roomBlueprints.Add(blueprint);
                }

                mapConfig.Initialize(roomBlueprints);
            }
        }
    }

    private Sprite LoadSprite(string path)
    {
        return Resources.Load<Sprite>(path);
    }

    private List<Effect> LoadEffects(string effectsData)
    {
        // 这里假设 effectsData 是一个逗号分隔的效果名称列表
        List<Effect> effects = new List<Effect>();

        if (string.IsNullOrEmpty(effectsData))
        {
            return effects;
        }

        string[] effectNames = effectsData.Split(',');
        foreach (string effectName in effectNames)
        {
            // 假设你有一个方法来加载Effect实例
            Effect effect = Resources.Load<Effect>($"Effects/{effectName}");
            if (effect != null)
            {
                effects.Add(effect);
            }
            else
            {
                Debug.LogError($"Effect not found: {effectName}");
            }
        }
        return effects;
    }

    private List<StatusEffect> LoadStatusEffects(string statusEffectsData)
    {
        // 这里假设 statusEffectsData 是一个逗号分隔的状态效果名称列表
        List<StatusEffect> statusEffects = new List<StatusEffect>();

        if (string.IsNullOrEmpty(statusEffectsData))
        {
            return statusEffects;
        }

        string[] statusEffectNames = statusEffectsData.Split(',');
        foreach (string statusEffectName in statusEffectNames)
        {
            // 假设你有一个方法来加载StatusEffect实例
            StatusEffect statusEffect = Resources.Load<StatusEffect>($"StatusEffects/{statusEffectName}");
            if (statusEffect != null)
            {
                statusEffects.Add(statusEffect);
            }
            else
            {
                Debug.LogError($"StatusEffect not found: {statusEffectName}");
            }
        }
        return statusEffects;
    }
}