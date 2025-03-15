using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;

public class UI_Character : MonoBehaviour, IPointerDownHandler
{
    [SerializeField] private Transform cardSlotParent;
    [SerializeField] private GameObject cardSlotPrefab;
    [SerializeField] private Transform cardInitParent;
    [SerializeField] private List<CardDataSO> cardData;//��ʱֱ������ҵĿ��ƿ�


    //private void Awake()
    //{
    //    foreach (var entry in CardManager.Instance.currentLibrary.entryList)
    //    {
    //        for (int i = 0; i < entry.amount; i++)
    //        {
    //            cardData.Add(entry.cardData);
    //        }
    //    }
    //}

    void Start()
    {
        transform.parent.GetChild(0).GetComponent<UI_Character>().SetupCardList();//Ĭ���õ�һ����ɫ�Ŀ��ƿ�
        //SetupDefaultCraftWindow();
    }

    public void SetCharacter(List<CardDataSO> _cardData, Transform _cardSlotParent, Transform _cardInitParent)
    {
        this.cardData = _cardData;
        this.cardSlotParent = _cardSlotParent;
        this.cardInitParent = _cardInitParent;
    }

    //��ʾ�����б�
    public void SetupCardList()
    {
        for (int i = 0; i < cardSlotParent.childCount; i++)
        {
            Destroy(cardSlotParent.GetChild(i).gameObject);
        }

        for (int i = 0; i < cardData.Count; i++)
        {
            GameObject newSlot = Instantiate(cardSlotPrefab, cardSlotParent);
            UI_CardSlot cardSlot = newSlot.GetComponent<UI_CardSlot>();
            cardSlot.SetItem(cardData[i]);
            cardSlot.cardInitParent = cardInitParent;
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        SetupCardList();
    }

    public void SetupDefaultCraftWindow()
    {
        //if (craftEquipment[0] != null)
        //    //GetComponentInParent<UI>().craftWindow.SetupCraftWindow(craftEquipment[0]);
    }
}
