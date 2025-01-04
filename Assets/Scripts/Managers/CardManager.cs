using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public class CardManager : MonoBehaviour
{
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
        InitializeCardDataList();

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
        return cardDataList[randomIndex];
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
}
