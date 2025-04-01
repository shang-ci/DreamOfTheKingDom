using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CharacterInfoPanel : MonoBehaviour
{
    public TextMeshProUGUI characterNameText;
    public Transform cardListContent;
    public GameObject cardItemPrefab;

    public void ShowCharacterInfo(CharacterBase character)
    {
        // 显示角色的牌库
        if (character is Player player)
        {
            foreach (var cardLibraryEntry in CardManager.instance.currentLibrary.entryList)
            {
                for(int i = 0;i < cardLibraryEntry.amount; i++)
                {
                    CardManager.instance.CreateCard(cardLibraryEntry.cardData);//实例化卡牌
                }
            }
        }
        //else if (character is Enemy enemy)
        //{
        //    foreach (var cardLibraryEntry in enemy.actionDataSO.actions)
        //    {
        //        for (int i = 0; i < cardLibraryEntry.amount; i++)
        //        {
        //            CardManager.instance.CreateCard(cardLibraryEntry.cardData);//实例化卡牌
        //        }
        //    }
        //}

    }
}