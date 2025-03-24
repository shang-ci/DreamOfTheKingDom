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

    //��ӵ�manager��
    [ContextMenu("���Ի��װ��")]
    public void CreatEquipmentItem(Equipment_ItemData item)
    {
        equipmentItems.Add(item);
        AddEquipmentItem(item);
    } 
    
    //����װ����equipmentParent��
    public void AddEquipmentItem(Equipment_ItemData item)
    {
        // ʵ�����µ�װ����
        Equipment_Item newItem = Instantiate(equipmentItemPrefab, equipmentParent);

        // ����װ���������
        newItem.SetEquipmentItem(item);
    }

    public void text()
    {
        AddEquipmentItem(equipmentItems[0] as Equipment_ItemData);
    }

}
