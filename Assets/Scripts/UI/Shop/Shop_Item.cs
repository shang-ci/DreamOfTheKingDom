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
    public CardDataSO cardData;//��ʱ������ſ�������,��ΪItem��CardDataSO����Ƽ���ʱ��û�п��ǵ��̵������

    public void SetShopItem(Item item)
    {
        this.item = item;
        icon.sprite = item.icon;
        des.text = item.des;
    }


    //�����������¼�
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
