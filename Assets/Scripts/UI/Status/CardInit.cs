using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CardInit : MonoBehaviour
{
    public Image cardSprite;
    public Text costText;
    public Text descriptionText;
    public Text typeText;
    public Text cardName;
    public CardDataSO cardData;


    public void Init(CardDataSO data)
    {
        cardSprite.sprite = data.cardImage;
        costText.text = data.cost.ToString();
        descriptionText.text = data.description;
        typeText.text = data.cardType switch
        {
            CardType.Attack => "攻击",
            CardType.Defense => "技能",
            CardType.Abilities => "能力",
            _ => throw new System.NotImplementedException(),
        };
        cardName.text = data.cardName;
    }
}
