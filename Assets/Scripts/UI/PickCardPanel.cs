using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TextCore;
using UnityEngine.UIElements;

public class PickCardPanel : MonoBehaviour
{
    public CardManager cardManager;

    private VisualElement rootElement;
    public VisualTreeAsset cardTemplate;//一个卡牌模板
    private VisualElement cardContainer;//放待抽取卡牌的容器
    private CardDataSO currentCardData;

    private Button confirmButton;

    private List<Button> cardButtons = new List<Button>();

    [Header("广播事件")]
    public ObjectEventSO finishPickCardEvent;

    private void OnEnable() 
    {
        rootElement = GetComponent<UIDocument>().rootVisualElement;
        cardContainer = rootElement.Q<VisualElement>("Container");
        confirmButton = rootElement.Q<Button>("ConfirmButton");

        confirmButton.clicked += OnConfirmButtonClicked;

        // 初始化三张卡牌
        for (int i = 0; i < 3; i++)
        {
            var card = cardTemplate.Instantiate();
            var data = cardManager.GetNewCardData();//获得新的卡牌数据——原版数据

            // 填充卡牌数据到卡牌模板中
            InitCard(card, data);

            var cardButton = card.Q<Button>("Card");//获取卡牌身上的按钮
            cardContainer.Add(card);//将卡牌添加到容器中
            cardButtons.Add(cardButton);

            cardButton.clicked += () => OnCardClicked(cardButton, data);
        }
    }

    //点击确认按钮时，解锁卡牌——广播，关闭抽卡面板、隐藏抽卡按钮
    private void OnConfirmButtonClicked()
    {
        cardManager.UnlockCard(currentCardData);//解锁卡牌——原版数据
        finishPickCardEvent.RaiseEvent(null, this);
    }

    //点击卡牌时，高亮未选中的卡牌
    private void OnCardClicked(Button cardButton, CardDataSO data)
    {
        currentCardData = data;

        Debug.Log($"Card clicked: {currentCardData.cardName}");

        for (int i = 0; i < cardButtons.Count; i++)
        {
            if (cardButtons[i] == cardButton)
            {
                cardButtons[i].SetEnabled(false);
            }
            else
            {
                cardButtons[i].SetEnabled(true);
            }
        }
    }

    //把卡牌数据填充到卡牌模板中
    public void InitCard(VisualElement card, CardDataSO cardData)
    {
        var cardSpriteElement = card.Q<VisualElement>("CardSprite");
        var cardCost = card.Q<Label>("EnergyCost");
        var cardDescription = card.Q<Label>("CardDescription");
        var cardType = card.Q<Label>("CardType");
        var cardName = card.Q<Label>("CardName");

        cardSpriteElement.style.backgroundImage = new StyleBackground(cardData.cardImage);
        cardName.text = cardData.cardName;
        cardCost.text = cardData.cost.ToString();
        cardDescription.text = cardData.description;
        cardType.text = cardData.cardType switch
        {
            CardType.Attack => "攻击",
            CardType.Defense => "技能",
            CardType.Abilities => "能力",
            _ => throw new System.NotImplementedException(),
        };
    }
}
