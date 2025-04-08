using System;
using System.Collections.Generic;
using UnityEngine;

public class EquipManager : MonoBehaviour
{
    public static EquipManager instance;
    public Transform equipmentParent;
    public Equipment_Item equipmentItemPrefab;
    public List<Equipment_ItemData> equipmentItemsData = new List<Equipment_ItemData>();//������������װ�����ݣ���ȡʱ�ټ��س���
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
        foreach (var item in equipmentItems)//�������е�װ�������ܲ���ִ����
        {
            if (item.item.Timing == _timing)//�����עװ����ʱ����������Ч����ʱ��
            {
                switch (item.item.TargetType)//Ϊÿ��װ���ҵ�Ŀ�ꡪ��ע�⣺Ҫ�ǹ�������һ������Ҫ������Щ��ɫ��״̬
                {
                    case EquipmentTargetType.Self:
                        item.target = GameManager.instance.playerRandomCharacter;
                        break;
                    case EquipmentTargetType.Target:
                        item.target = GameManager.instance.enemyRandomCharacter;
                        break;
                    case EquipmentTargetType.Our:
                        item.targets = GameManager.instance.playerCharacters;
                        break;
                    case EquipmentTargetType.Enemies:
                        item.targets = GameManager.instance.enemyCharacters;
                        break;
                    case EquipmentTargetType.ALL:
                        item.targets = GameManager.instance.allCharacters;
                        break;
                    case EquipmentTargetType.Random:
                        item.target = GameManager.instance.randomCharacter;
                        break;
                }

                if (item.target != null)
                    item.Execute(GameManager.instance.playerRandomCharacter, item.target);
                else if (item.targets != null)
                    item.Execute(GameManager.instance.playerRandomCharacter, item.targets);
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
