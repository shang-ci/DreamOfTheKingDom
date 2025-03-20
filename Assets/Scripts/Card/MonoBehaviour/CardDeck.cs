using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Rendering;

//卡牌堆，用来保存卡牌数据，抽牌，弃牌，洗牌等操作——针对特殊的游戏可以提供别的函数，比如重新抽取、重置、弃牌等
public class CardDeck : MonoBehaviour
{
    public static CardDeck instance { get; private set; }

    public CardManager cardManager;//因为这里的manager它们都是持久的，不需要用单例模式，所以直接引用即可
    public CardLayoutManager layoutManager;//卡牌布局管理器
    public Vector3 deckPosition;//抽出来的卡牌的位置的初始位置

    // 抽牌堆
    [SerializeField] public List<CardDataSO> drawDeck = new List<CardDataSO>();
    // 弃牌堆
    private List<CardDataSO> discardDeck = new List<CardDataSO>();
    // 当前手牌（每回合）
    [SerializeField]public List<Card> handCardObjectList = new List<Card>();

    [Header("事件广播")]
    public IntEventSO drawCountEvent;
    public IntEventSO discardCountEvent;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // 这里要注意代码顺序问题，抽牌池要在cardManager.currentLibrary获取完才执行
    private void Start() 
    {
        InitializeDeck();
    }

    //初始化，将卡牌库中的卡牌复制到抽牌堆中——每开启一轮战斗时调用——抽牌堆拿到的都是原始卡牌数据
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
    /// 事件监听函数——玩家回合开始时调用——抽取4张卡牌
    /// </summary>
    public void NewTurnDrawCards()
    {
        DrawCard(4);
    }

    //抽取卡牌，从抽牌堆中抽取卡牌，放入手牌中——每回合开始时调用
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

            // 从抽牌堆中抽一张牌——抽的是卡牌数据，赋值给卡牌预制对象
            CardDataSO cardData = drawDeck[0];
            drawDeck.RemoveAt(0);

            // 更新 UI 数字
            drawCountEvent.RaiseEvent(drawDeck.Count, this);

            //拿到卡牌预制对象
            var card = cardManager.GetCardObject().GetComponent<Card>();

            // 初始化——把卡牌数据赋值给卡牌预制对象
            card.Init(cardData);
            card.transform.position = deckPosition;

            // 将这张牌添加到手牌中
            handCardObjectList.Add(card);
            var delay = i * 0.2f;

            // 设置卡牌的位置、旋转角度
            SetCardLayout(delay);
        }
    }

    //
    private void SetCardLayout(float delay)
    {
        for (int i = 0; i < handCardObjectList.Count; i++)
        {
            Card currentCard = handCardObjectList[i];

            CardTransform cardTransform = layoutManager.GetCardTransform(i, handCardObjectList.Count);

            // currentCard.transform.SetPositionAndRotation(cardTransform.pos, cardTransform.rotation);——这个是直接设置位置和旋转角度，没有动画效果

            // 卡牌能量判断加灰色效果
            currentCard.UpdateCardState();

            // 卡牌动画
            currentCard.isAnimating = true;
            //让卡牌从缩放为0的状态变为正常大小，而且是每个新抽出来的卡才放大，因为之前的已经是正常大小了所以只有薪酬出来的卡才放大
            currentCard.transform.DOScale(Vector3.one, 0.2f).SetDelay(delay).onComplete = () => {
                //卡牌放大完毕后，将卡牌移动到指定位置，有一个连续发拍的效果
                currentCard.transform.DOMove(cardTransform.pos, 0.5f).onComplete = () => {
                    currentCard.isAnimating = false;
                };
                currentCard.transform.DORotateQuaternion(cardTransform.rotation, 0.5f);
            };

            // 设置卡牌排序层级，保证卡牌的显示顺序
            currentCard.GetComponent<SortingGroup>().sortingOrder = i;
            //保存下卡牌的位置和旋转角度，方便选中后有个上移的提牌效果可以返回——但具体是用卡牌的animator部分实现的，还原的话也只用归零就好
            currentCard.UpdatePositionRotation(cardTransform.pos, cardTransform.rotation);
        }
    }

    /// <summary>
    /// 洗牌
    /// </summary>
    private void ShuffleDeck()
    {
        //在这个游戏里就是每次洗牌都要确保弃牌堆是空的
        discardDeck.Clear();

        // 更新 UI 显示数量——抽牌堆和弃牌堆的数量的变化也属于UI更新部分，通过事件广播来也许可以统一交给UI更新的部分管理
        drawCountEvent.RaiseEvent(drawDeck.Count, this);
        discardCountEvent.RaiseEvent(discardDeck.Count, this);

        //这里的洗牌算法是每次随机交换两张牌的位置，但是在别的游戏里可能会有考虑概率的洗牌算法
        for (int i = 0; i < drawDeck.Count; i++)
        {
            CardDataSO temp = drawDeck[i];
            int randomIndex = Random.Range(i, drawDeck.Count);
            drawDeck[i] = drawDeck[randomIndex];
            drawDeck[randomIndex] = temp;
        }
    }

    /// <summary>
    /// 弃牌逻辑，在打出牌的事件中调用，会接受一个卡牌对象，将其放入弃牌堆中
    /// </summary>
    /// <param name="obj"></param>
    public void DiscardCard(object obj)
    {
        Card card = obj as Card;

        discardDeck.Add(card.GetOriginalCardData());//把初始数据放入弃牌堆——保持每次抽牌都是用的初始数据
        handCardObjectList.Remove(card);

        cardManager.DiscardCard(card.gameObject);//卡牌池回收

        // 更新弃牌堆 UI
        discardCountEvent.RaiseEvent(discardDeck.Count, this);

        SetCardLayout(0);
    }

    //随机x张弃牌
    public void DiscardRandomCard(int value)
    {
        for (int i = 0; i < value; i++)
        {
            if (handCardObjectList.Count > 0)
            {
                int randomIndex = Random.Range(0, handCardObjectList.Count);
                Card cardToDiscard = handCardObjectList[randomIndex];

                DiscardCard(cardToDiscard);
            }
        }
    }

    /// <summary>
    /// 事件监听函数
    /// </summary>
    public void OnPlayerTurnEnd()
    {
        for (int i = 0; i < handCardObjectList.Count; i++)
        {
            discardDeck.Add(handCardObjectList[i].GetOriginalCardData());//把初始数据放入弃牌堆——保持每次抽牌都是用的初始数据
            cardManager.DiscardCard(handCardObjectList[i].gameObject);
        }

        handCardObjectList.Clear();
        discardCountEvent.RaiseEvent(discardDeck.Count, this);
    }

    //回收所有卡牌——当游戏结束时调用——玩家或者怪物死亡时
    public void ReleaseAllCards(object obj)
    {
        foreach (var card in handCardObjectList)
        {
            cardManager.DiscardCard(card.gameObject);
        }

        handCardObjectList.Clear();
        //InitializeDeck();//在结束时重新初始化卡牌堆，防止下一轮粗线错误——必须要在解锁新卡牌之后再初始化卡牌堆
    }

    // 根据卡牌名字从 drawDeck 或 discardDeck 中获取所有 CardDataSO——获得的是卡牌数据的副本
    public List<CardDataSO> GetAllCardDataByName(string cardName)
    {
        return cardManager.GetAllCardDataClonesByName(cardName);
    }

    // 根据卡牌名字从 handCardObjectList 中获取所有 CardDataSO——获得的是卡牌数据的副本
    public List<CardDataSO> GetHandCardDataByName(string cardName)
    {
        List<CardDataSO> result = new List<CardDataSO>();
        foreach (var cardData in handCardObjectList)
        {
            if (cardData.cardData.cardName == cardName)
            {
                result.Add(cardData.cardData);
            }
        }
        return result;
    }

    // 还原所有卡牌数据
    public void RestoreAllCards()
    {
        foreach (var card in handCardObjectList)
        {
            card.RestoreOriginalCardData();
        }
    }

    //TOOD:以后要完善成根据卡牌类型获取对应卡牌数量，要有两个参数，一个是卡牌类型，一个是卡牌堆类型
    public int GetDrawDeckCountByType()
    {
        // 获取抽牌堆中的攻击牌数量
        int attackCardCount = 0;
        foreach (var cardData in drawDeck)
        {
            if (cardData.cardType == CardType.Attack)
            {
                attackCardCount++;
            }
        }
        return attackCardCount;
    }

    public List<CardDataSO> GetCurrentCardDatas()
    {
        var result = new List<CardDataSO>();
        foreach (var entry in cardManager.currentLibrary.entryList)
        {
            for (int i = 0; i < entry.amount; i++)
            {
                result.Add(entry.cardData);
            }
        }
        return result;
    }
}
