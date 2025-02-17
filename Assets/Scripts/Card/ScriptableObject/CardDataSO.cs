using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CardDataSO", menuName = "Card/CardDataSO")]
public class CardDataSO : ScriptableObject
{
    public string cardName;
    public Sprite cardImage;
    public int cost;
    public CardType cardType;
    //public CardTargetType cardTargetType;
    [TextArea]
    public string description;

    // 执行的实际效果
    public List<Effect> effects;

    // 状态效果
    public List<StatusEffect> statusEffects;

    public void Initialize(string name, Sprite image, int cost, CardType type, string description, List<Effect> effects, List<StatusEffect> statusEffects)
    {
        this.cardName = name;
        this.cardImage = image;
        this.cost = cost;
        this.cardType = type;
        //this.cardTargetType = targetType;
        this.description = description;
        this.effects = effects;
        this.statusEffects = statusEffects;
    }
}
