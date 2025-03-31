using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Shop_Item : MonoBehaviour,IPointerClickHandler
{
    private Item item;
    public Image icon;
    public TextMeshProUGUI des;
    public TextMeshProUGUI price;
    //public CardDataSO cardData;//��ʱ������ſ�������,��ΪItem��CardDataSO����Ƽ���ʱ��û�п��ǵ��̵������

    public void SetShopItem(Item item)
    {
        this.item = item;

        if (item.itemType == ItemType.Card) { 
            var cardData = item as CardDataSO;
            icon.sprite = cardData.cardImage;
            des.text = cardData.description;
            price.text = cardData.price.ToString();
        }else if (item.itemType == ItemType.Equipment)
        {
            icon.sprite = item.icon;
            des.text = item.des;
            price.text = item.price.ToString();
        }

    }


    //�����������¼�
    public void OnPointerClick(PointerEventData eventData)
    {
        if (TurnBaseManager.instance.player.growthSystem.GetCoin() > item.price) {
            //����item���������ֱ𴫵�����
            switch (item.itemType)
            {
                case ItemType.Equipment:
                    EquipManager.instance.CreatEquipmentItem(item as Equipment_ItemData);
                    break;
                case ItemType.Card:
                    CardManager.instance.UnlockCard(item as CardDataSO);
                    break;
            }
            TurnBaseManager.instance.player.growthSystem.ExpendCoin(item.price);
        }
    }



    private void CreatCard()
    {
        throw new NotImplementedException();
    }
}
