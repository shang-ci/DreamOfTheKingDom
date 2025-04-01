using System;
using System.Collections.Generic;
using UnityEngine;

public class EquipManager : MonoBehaviour
{
    public static EquipManager instance;
    public Transform equipmentParent;
    public Equipment_Item equipmentItemPrefab;
    public List<Equipment_ItemData> equipmentItemsData = new List<Equipment_ItemData>();
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
        foreach(var item in equipmentItemsData)
        {
            if(item.Timing == _timing)
            {
                item.Execute(TurnBaseManager.instance.player, TurnBaseManager.instance.player);
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
