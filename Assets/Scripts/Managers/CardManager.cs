using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public class CardManager : MonoBehaviour
{
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

        // 根据策划游戏的内容自行调整
        foreach (var item in newGameCardLibrary.entryList)
        {
            currentLibrary.entryList.Add(item);
        }
    }

    private void OnDisable() 
    {
        currentLibrary.entryList.Clear();    
    }

    #region 获取项目卡牌

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
    /// 抽卡时调用的函数获得卡牌 GameObject
    /// </summary>
    /// <returns></returns>
    public GameObject GetCardObject()
    {
        var cardObj = poolTool.GetObjectFromPool();
        cardObj.transform.localScale = Vector3.zero;
        return cardObj;
    }

    public void DiscardCard(GameObject cardObj)
    {
        poolTool.ReleaseObjectToPool(cardObj);
    }

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

    /// <summary>
    /// 解锁添加新卡牌
    /// </summary>
    /// <param name="newCardData"></param>
    public void UnlockCard(CardDataSO newCardData)
    {
        var newCard = new CardLibraryEntry
        {
            cardData = newCardData,
            amount = 1
        };

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
