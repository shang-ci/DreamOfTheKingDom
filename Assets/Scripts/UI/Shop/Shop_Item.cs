using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Shop_Item : MonoBehaviour,IPointerClickHandler
{
    public Image icon;
    public TextMeshProUGUI des;
    private Item item;
    public CardDataSO cardData;//暂时这样存放卡牌数据,因为Item和CardDataSO在设计及的时候没有考虑到商店的需求

    public void SetShopItem(Item item)
    {
        this.item = item;
        icon.sprite = item.icon;
        des.text = item.des;
    }


    //处理点击购买事件
    public void OnPointerClick(PointerEventData eventData)
    {
        switch(item.itemType)
        {
            case ItemType.Equipment:
                EquipManager.instance.CreatItem(item);
                break;
            case ItemType.Card:
                CardManager.instance.UnlockCard(cardData);
                break;
        }
    }

    private void CreatCard()
    {
        throw new NotImplementedException();
    }
}
