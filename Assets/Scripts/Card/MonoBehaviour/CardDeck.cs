using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Rendering;

public class CardDeck : MonoBehaviour
{
    public CardManager cardManager;
    public CardLayoutManager layoutManager;
    public Vector3 deckPosition;

    // 抽牌堆
    private List<CardDataSO> drawDeck = new List<CardDataSO>();
    // 弃牌堆
    private List<CardDataSO> discardDeck = new List<CardDataSO>();
    // 当前手牌（每回合）
    private List<Card> handCardObjectList = new List<Card>();

    [Header("事件广播")]
    public IntEventSO drawCountEvent;
    public IntEventSO discardCountEvent;

    // TODO: 测试用
    private void Start() 
    {
        InitializeDeck();
    }

    public void InitializeDeck()
    {
        drawDeck.Clear();
        foreach (var entry in cardManager.currentLibrary.entryList)
        {
            for (int i = 0; i < entry.amount; i++)
            {
                drawDeck.Add(entry.cardData);
            }
        }

        // 洗牌/更新抽牌堆or弃牌堆的数字
        ShuffleDeck();
    }

    [ContextMenu("测试抽牌")]
    public void TestDrawCard()
    {
        DrawCard(1);
    }

    /// <summary>
    /// 事件监听函数
    /// </summary>
    public void NewTurnDrawCards()
    {
        DrawCard(4);
    }

    public void DrawCard(int amount)
    {
        for (int i = 0; i < amount; i++)
        {
            if (drawDeck.Count == 0)
            {
                // 抽牌堆为空，从弃牌堆中重新生成
                foreach (var item in discardDeck)
                {
                    drawDeck.Add(item);
                }
                ShuffleDeck();
            }

            // 从抽牌堆中抽一张牌
            CardDataSO cardData = drawDeck[0];
            drawDeck.RemoveAt(0);

            // 更新 UI 数字
            drawCountEvent.RaiseEvent(drawDeck.Count, this);

            var card = cardManager.GetCardObject().GetComponent<Card>();
            // 初始化
            card.Init(cardData);
            card.transform.position = deckPosition;

            // 将这张牌添加到手牌中
            handCardObjectList.Add(card);
            var delay = i * 0.2f;
            SetCardLayout(delay);
        }
    }

    private void SetCardLayout(float delay)
    {
        for (int i = 0; i < handCardObjectList.Count; i++)
        {
            Card currentCard = handCardObjectList[i];

            CardTransform cardTransform = layoutManager.GetCardTransform(i, handCardObjectList.Count);

            // currentCard.transform.SetPositionAndRotation(cardTransform.pos, cardTransform.rotation);

            // 卡牌能量判断
            currentCard.UpdateCardState();

            currentCard.isAnimating = true;
            currentCard.transform.DOScale(Vector3.one, 0.2f).SetDelay(delay).onComplete = () => {
                currentCard.transform.DOMove(cardTransform.pos, 0.5f).onComplete = () => {
                    currentCard.isAnimating = false;
                };
                currentCard.transform.DORotateQuaternion(cardTransform.rotation, 0.5f);
            };

            // 设置卡牌排序
            currentCard.GetComponent<SortingGroup>().sortingOrder = i;
            currentCard.UpdatePositionRotation(cardTransform.pos, cardTransform.rotation);
        }
    }

    /// <summary>
    /// 洗牌
    /// </summary>
    private void ShuffleDeck()
    {
        discardDeck.Clear();

        // 更新 UI 显示数量
        drawCountEvent.RaiseEvent(drawDeck.Count, this);
        discardCountEvent.RaiseEvent(discardDeck.Count, this);

        for (int i = 0; i < drawDeck.Count; i++)
        {
            CardDataSO temp = drawDeck[i];
            int randomIndex = Random.Range(i, drawDeck.Count);
            drawDeck[i] = drawDeck[randomIndex];
            drawDeck[randomIndex] = temp;
        }
    }

    /// <summary>
    /// 弃牌逻辑，事件函数
    /// </summary>
    /// <param name="obj"></param>
    public void DiscardCard(object obj)
    {
        Card card = obj as Card;

        discardDeck.Add(card.cardData);
        handCardObjectList.Remove(card);

        cardManager.DiscardCard(card.gameObject);

        // 更新弃牌堆 UI
        discardCountEvent.RaiseEvent(discardDeck.Count, this);

        SetCardLayout(0);
    }

    /// <summary>
    /// 事件监听函数
    /// </summary>
    public void OnPlayerTurnEnd()
    {
        for (int i = 0; i < handCardObjectList.Count; i++)
        {
            discardDeck.Add(handCardObjectList[i].cardData);
            cardManager.DiscardCard(handCardObjectList[i].gameObject);
        }

        handCardObjectList.Clear();
        discardCountEvent.RaiseEvent(discardDeck.Count, this);
    }

    /// <summary>
    /// 弃掉所有卡牌
    /// </summary>
    public void ReleaseAllCards(object obj)
    {
        foreach (var card in handCardObjectList)
        {
            cardManager.DiscardCard(card.gameObject);
        }

        handCardObjectList.Clear();
        InitializeDeck();
    }
}
