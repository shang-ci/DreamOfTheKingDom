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
        // ����״̬Ч���ı��¼�
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

    //��ӵ�manager��
    public void CreatEquipmentItem(Equipment_ItemData item)
    {
        AddEquipmentItem(item);
    } 
    
    //����װ����equipmentParent��
    public void AddEquipmentItem(Equipment_ItemData item)
    {
        // ʵ�����µ�װ����
        Equipment_Item newItem = Instantiate(equipmentItemPrefab, equipmentParent);
        // ����װ���������
        newItem.SetEquipmentItem(item);

        equipmentItems.Add(newItem);
        equipmentItemsData.Add(item);
    }

    public void text()
    {
        AddEquipmentItem(equipmentItemsData[0] as Equipment_ItemData);
    }

}
