using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public class CardManager : MonoBehaviour
{
    public static CardManager Instance { get; private set; }

    //卡牌池子
    public PoolTool poolTool;

    // 游戏中所有可能出现的卡牌
    public List<CardDataSO> cardDataList;

    [Header("卡牌库")]
    // 新游戏时初始化的卡牌库
    public CardLibrarySO newGameCardLibrary;
    // 当前玩家卡牌库
    public CardLibrarySO currentLibrary;

    private int previousIndex = 0;

    private void Awake() 
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        //从unity的addressable group中获得所有卡牌数据
        //InitializeCardDataList();


        // 从ExcelDataLoader中获取卡牌数据
        cardDataList = ExcelDataLoader.Instance.cardDataList;

        //根据Excel表中的卡牌数据初始化新游戏卡牌库
        InitializeNewGameCardLibrary();

        // 根据策划游戏的内容自行调整——将初始卡牌添加到当前卡牌库，这样在游戏结束时回到主菜单再次开始游戏时，卡牌库不会被清空，若想要重新打关卡，可以写个newgame函数重新赋值
        foreach (var item in newGameCardLibrary.entryList)
        {
            currentLibrary.entryList.Add(item);
        }
    }

    // 游戏结束时清空卡牌库
    private void OnDisable() 
    {
        currentLibrary.entryList.Clear();    
    }

    #region 获取项目卡牌，从addressable group中读取card data

    private void InitializeCardDataList()
    {
        Addressables.LoadAssetsAsync<CardDataSO>("CardData", null).Completed += OnCardDataLoaded;
    }

    private void OnCardDataLoaded(AsyncOperationHandle<IList<CardDataSO>> handle)
    {
        if (handle.Status == AsyncOperationStatus.Succeeded)
        {
            cardDataList = new List<CardDataSO>(handle.Result);
        }
        else
        {
            Debug.LogError("No CardData Found!");
        }
    }
    #endregion



    // 初始化新游戏卡牌库——让其保持为Excel表的前三个卡牌
    private void InitializeNewGameCardLibrary()
    {
        // 从 ExcelDataLoader 中获取卡牌数据
        var excelCardDataList = ExcelDataLoader.Instance.cardDataList;

        // 确保有足够的卡牌数据
        if (excelCardDataList.Count < 3)
        {
            Debug.LogError("Not enough card data in ExcelDataLoader");
            return;
        }

        // 添加前三个卡牌到 newGameCardLibrary
        newGameCardLibrary.entryList.Clear();
        newGameCardLibrary.entryList.Add(new CardLibraryEntry { cardData = excelCardDataList[0], amount = 1 });
        newGameCardLibrary.entryList.Add(new CardLibraryEntry { cardData = excelCardDataList[8], amount = 1 });
        newGameCardLibrary.entryList.Add(new CardLibraryEntry { cardData = excelCardDataList[6], amount = 1 });
        newGameCardLibrary.entryList.Add(new CardLibraryEntry { cardData = excelCardDataList[7], amount = 1 });
    }



    /// <summary>
    /// 抽卡时调用的函数获得卡牌 GameObject——这里的是从对象池中获取卡牌，对象池里有7个卡牌，也就是说最多同时存在7个卡牌在手牌中
    /// </summary>
    /// <returns></returns>
    public GameObject GetCardObject()
    {
        var cardObj = poolTool.GetObjectFromPool();
        cardObj.transform.localScale = Vector3.zero;//将卡牌缩放为0,有一个放大动画
        return cardObj;
    }

    //回收卡牌
    public void DiscardCard(GameObject cardObj)
    {
        poolTool.ReleaseObjectToPool(cardObj);
    }

    //返回一个新的卡牌数据——制作卡牌用
    public CardDataSO GetNewCardData()
    {
        var randomIndex = 0;
        do
        {
            randomIndex = UnityEngine.Random.Range(0, cardDataList.Count);
        } while (previousIndex == randomIndex);

        previousIndex = randomIndex;
        return cardDataList[randomIndex];//这里就是从Excel表中随机返回一张卡牌数据了——cardDataList就是读到的
    }

    //解锁一张卡牌
    public void UnlockCard(CardDataSO newCardData)
    {
        var newCard = new CardLibraryEntry
        {
            cardData = newCardData,
            amount = 1
        };

        // 如果卡牌库中已经有这张卡牌，那么只需要将数量加1
        if (currentLibrary.entryList.Contains(newCard))
        {
            var target = currentLibrary.entryList.Find(t => t.cardData == newCardData);
            target.amount++;
        }
        else
        {
            currentLibrary.entryList.Add(newCard);
        }
    }


    //todo:应该从抽牌堆获得卡牌，而不是直接从卡牌库中获得
    // 获取当前卡牌库中的所有攻击卡牌——强化/研究卡牌时调用
    public List<CardDataSO> GetAttackCards()
    {
        List<CardDataSO> attackCards = new List<CardDataSO>();
        foreach (var entry in currentLibrary.entryList)
        {
            if (entry.cardData.cardType == CardType.Attack)
            {
                attackCards.Add(entry.cardData);
            }
        }
        return attackCards;
    }
}
