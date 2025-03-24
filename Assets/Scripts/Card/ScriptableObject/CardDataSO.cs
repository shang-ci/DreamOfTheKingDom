using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CardDataSO", menuName = "Card/CardDataSO")]
public class CardDataSO : Item
{
    public string cardName;
    public Sprite cardImage;
    public int cost;
    public CardType cardType;
    [TextArea]
    public string description;

   // public override ItemType ItemType { get => base.ItemType; set => base.ItemType = value; } = ItemType.Card;

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

    // 创建卡牌数据的副本
    public CardDataSO Clone()
    {
        CardDataSO clone = ScriptableObject.CreateInstance<CardDataSO>();
        clone.cardName = this.cardName;
        clone.cardImage = this.cardImage;
        clone.cost = this.cost;
        clone.cardType = this.cardType;
        clone.description = this.description;
        //clone.effects = new List<Effect>(this.effects);
        //clone.statusEffects = new List<StatusEffect>(this.statusEffects);

        // 深度克隆 effects 列表
        clone.effects = new List<Effect>();
        foreach (var effect in this.effects)
        {
            var effectClone = Instantiate(effect);
            clone.effects.Add(effectClone);
        }

        // 深度克隆 statusEffects 列表
        clone.statusEffects = new List<StatusEffect>();
        foreach (var statusEffect in this.statusEffects)
        {
            var statusEffectClone = Instantiate(statusEffect);
            clone.statusEffects.Add(statusEffectClone);
        }
        return clone;
    }

    // 重写Equals和GetHashCode方法，用于判断两个CardDataSO是否相等
    public override bool Equals(object obj)
    {
        if (obj is CardDataSO other)
        {
            return cardName == other.cardName && cardType == other.cardType;
        }
        return false;
    }

    public override int GetHashCode()
    {
        int hashCardName = cardName == null ? 0 : cardName.GetHashCode();
        int hashCardType = cardType.GetHashCode();
        return hashCardName ^ hashCardType;
    }
}
