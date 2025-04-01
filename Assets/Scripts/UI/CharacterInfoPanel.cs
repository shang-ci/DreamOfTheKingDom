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
        // ��ʾ��ɫ���ƿ�
        if (character is Player player)
        {
            foreach (var cardLibraryEntry in CardManager.instance.currentLibrary.entryList)
            {
                for(int i = 0;i < cardLibraryEntry.amount; i++)
                {
                    CardManager.instance.CreateCard(cardLibraryEntry.cardData);//ʵ��������
                }
            }
        }
        //else if (character is Enemy enemy)
        //{
        //    foreach (var cardLibraryEntry in enemy.actionDataSO.actions)
        //    {
        //        for (int i = 0; i < cardLibraryEntry.amount; i++)
        //        {
        //            CardManager.instance.CreateCard(cardLibraryEntry.cardData);//ʵ��������
        //        }
        //    }
        //}

    }
}