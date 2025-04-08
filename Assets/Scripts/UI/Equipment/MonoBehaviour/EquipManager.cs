using System;
using System.Collections.Generic;
using UnityEngine;

public class EquipManager : MonoBehaviour
{
    public static EquipManager instance;
    public Transform equipmentParent;
    public Equipment_Item equipmentItemPrefab;
    public List<Equipment_ItemData> equipmentItemsData = new List<Equipment_ItemData>();//可以用来保存装备数据，读取时再加载出来
    public List<Equipment_Item> equipmentItems = new List<Equipment_Item>();



    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            text();
        }
    }

    private void OnEnable()
    {
        // 监听状态效果改变事件
        EffectTimingManager.Instance.OnEffectTimingChanged.AddListener(OnEffectTimingChanged);
    }

    private void OnDisable()
    {
        EffectTimingManager.Instance?.OnEffectTimingChanged.RemoveListener(OnEffectTimingChanged);
    }

    private void OnEffectTimingChanged(EffectTiming timing)
    {
        ExecuteEquipmentEffect(timing);
    }


    public void ExecuteEquipmentEffect(EffectTiming _timing)
    {
        foreach (var item in equipmentItems)//遍历所有的装备，看能不能执行它
        {
            if (item.item.Timing == _timing)//这里关注装备的时机，而不是效果的时机
            {
                switch (item.item.TargetType)//为每个装备找到目标——注意：要是怪物死了一个，还要更新这些角色的状态
                {
                    case EquipmentTargetType.Self:
                        item.target = GameManager.instance.playerRandomCharacter;
                        break;
                    case EquipmentTargetType.Target:
                        item.target = GameManager.instance.enemyRandomCharacter;
                        break;
                    case EquipmentTargetType.Our:
                        item.targets = GameManager.instance.playerCharacters;
                        break;
                    case EquipmentTargetType.Enemies:
                        item.targets = GameManager.instance.enemyCharacters;
                        break;
                    case EquipmentTargetType.ALL:
                        item.targets = GameManager.instance.allCharacters;
                        break;
                    case EquipmentTargetType.Random:
                        item.target = GameManager.instance.randomCharacter;
                        break;
                }

                if (item.target != null)
                    item.Execute(GameManager.instance.playerRandomCharacter, item.target);
                else if (item.targets != null)
                    item.Execute(GameManager.instance.playerRandomCharacter, item.targets);
            }


        }
    }

    //添加到manager中
    public void CreatEquipmentItem(Equipment_ItemData item)
    {
        AddEquipmentItem(item);
    } 
    
    //创建装备到equipmentParent里
    public void AddEquipmentItem(Equipment_ItemData item)
    {
        // 实例化新的装备项
        Equipment_Item newItem = Instantiate(equipmentItemPrefab, equipmentParent);
        // 设置装备项的数据
        newItem.SetEquipmentItem(item);

        equipmentItems.Add(newItem);
        equipmentItemsData.Add(item);
    }

    public void text()
    {
        AddEquipmentItem(equipmentItemsData[0] as Equipment_ItemData);
    }

}
