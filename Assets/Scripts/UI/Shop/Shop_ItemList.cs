using System.Collections.Generic;
using UnityEngine;

public class Shop_ItemList : MonoBehaviour
{
    public List<Item> equipmentItems;//储存所有的装备
    public Shop_Item shopEquipmentItemPrefab;//遗物预制体
    public CardInit cardInitPrefab;//卡牌预制体
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

    //添加义务装备到商店里
    private void SetEquipmentItems()
    {
        foreach (Transform child in shopParent)
        {
            Destroy(child.gameObject);
        }

        foreach (Item item in equipmentItems)
        {
            var newItem = Instantiate(shopEquipmentItemPrefab, shopParent);
            newItem.SetShopItem(item);
        }
    }
}
