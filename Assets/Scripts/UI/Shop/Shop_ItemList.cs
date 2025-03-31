using System.Collections.Generic;
using UnityEngine;

public class Shop_ItemList : MonoBehaviour
{
    public List<Equipment_ItemData> equipmentItems;//�������е�װ��
    public List<CardDataSO> cardDataSOItems;//�洢���еĿ�������
    public Shop_Item shopEquipmentItemPrefab;//����Ԥ����
    //public CardInit cardInitPrefab;//����Ԥ����
    public Transform shopParent;//��Ʒ������

    private void Awake()
    {
        SetEquipmentItems();//Ĭ���ȴ�װ���̵�
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            SetEquipmentItems();
        }
    }

    //���װ�����̵���
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