using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_CardSlot : MonoBehaviour,IPointerDownHandler
{
    public Image itemImage;
    public Text itemText;
    public CardDataSO cardDataItem;

    [SerializeField] private GameObject cardInitPrefab;
     public Transform cardInitParent;

    //private void Start()
    //{
    //    transform.parent.GetChild(0).GetComponent<UI_CardSlot>().SetCardInit();//默认展示第一个卡牌
    //}

    public void SetItem(CardDataSO cardData)
    {
        cardDataItem = cardData;

        itemImage.sprite = cardData.cardImage;
        itemText.text = cardData.cardName;
    }

    public void  OnPointerDown(PointerEventData eventData)
    {
        SetCardInit();
    }

    private void SetCardInit()
    {
        Destroy(cardInitParent.GetChild(0).gameObject);

        GameObject newCard = Instantiate(cardInitPrefab, cardInitParent);
        var card = newCard.GetComponent<CardInit>();
        card.Init(cardDataItem);

        // 修改RectTransform的positionX
        RectTransform rectTransform = newCard.GetComponent<RectTransform>();
        Vector3 position = rectTransform.position;
        position = new Vector3(0,0,0);
        rectTransform.anchoredPosition = position;
    }
}
