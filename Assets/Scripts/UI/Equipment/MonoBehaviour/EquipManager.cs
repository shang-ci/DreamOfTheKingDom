using System.Collections.Generic;
using UnityEngine;

public class EquipManager : MonoBehaviour
{
    public static EquipManager instance;
    public Transform equipmentParent;
    public Equipment_Item equipmentItemPrefab;
    public List<Item> equipmentItems = new List<Item>();



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

    //添加到manager中
    [ContextMenu("测试获得装备")]
    public void CreatEquipmentItem(Equipment_ItemData item)
    {
        equipmentItems.Add(item);
        AddEquipmentItem(item);
    } 
    
    //创建装备到equipmentParent里
    public void AddEquipmentItem(Equipment_ItemData item)
    {
        // 实例化新的装备项
        Equipment_Item newItem = Instantiate(equipmentItemPrefab, equipmentParent);

        // 设置装备项的数据
        newItem.SetEquipmentItem(item);
    }

    public void text()
    {
        AddEquipmentItem(equipmentItems[0] as Equipment_ItemData);
    }

}
