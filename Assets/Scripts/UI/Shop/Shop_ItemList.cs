using System.Collections.Generic;
using UnityEngine;

public class Shop_ItemList : MonoBehaviour
{
    public List<Equipment_ItemData> equipmentItems;//储存所有的装备
    public List<CardDataSO> cardDataSOItems;//存储所有的卡牌数据
    public Shop_Item shopEquipmentItemPrefab;//遗物预制体
    //public CardInit cardInitPrefab;//卡牌预制体
    public Transform shopParent;//物品父物体

    private void Awake()
    {
        SetEquipmentItems();//默认先打开装备商店
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            SetEquipmentItems();
        }
    }

    //添加装备到商店里
    public void SetEquipmentItems()
    {
        foreach (Transform child in shopParent)
        {
            Destroy(child.gameObject);
        }

        foreach (var item in equipmentItems)
        {
            var newItem = Instantiate(shopEquipmentItemPrefab, shopParent);
            newItem.SetShopItem(item);
        }
    }

    public void SetCardItems()
    {
        foreach (Transform child in shopParent)
        {
            Destroy(child.gameObject);
        }
        foreach (var item in cardDataSOItems)
        {
            var newItem = Instantiate(shopEquipmentItemPrefab, shopParent);
            newItem.SetShopItem(item);
        }
    }
}