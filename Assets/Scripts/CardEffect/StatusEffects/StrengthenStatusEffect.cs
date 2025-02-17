using System.Collections.Generic;
using UnityEngine;

//强化卡牌效果
[CreateAssetMenu(fileName = "StrengthenStatusEffect", menuName = "StatusEffect/StrengthenStatusEffect")]
public class StrengthenStatusEffect : StatusEffect
{
    public override void ChangeTime(CharacterBase character)
    {
        //SetBaseValue();

        EffectTimingManager.Instance.ChangeEffectTiming(EffectTiming.Strengthen);
    }

    public override void ExecuteEffect(CharacterBase character)
    {
        // 获取当前卡牌库中的所有攻击卡牌
        List<CardDataSO> attackCards = CardDeck.instance.GetAllCardDataByName("攻击");

        // 增加攻击卡牌的伤害值
        foreach (var cardData in attackCards)
        {
            foreach (var effect in cardData.effects)
            {
                if (effect is DamageEffect)
                {
                    var t = effect.value + value;
                    effect.value = t;

                    cardData.description = "对目标造成" + t + "点伤害";
                }
            }
            //只需要更新手牌中的卡牌UI
            CardDeck.instance.handCardObjectList.Find(card => card.cardData == cardData).Init(cardData);
        }
    }


    //todo:当游戏重开/战斗结束时消除状态的效果——不然强化卡的value一直增加

    //这里有个问题，就是强化卡的baseValue是在ChangeTime里初始化的，并不是在生成这张卡时就初始化的，它是在打出强化卡时初始化，如果提前打出研究卡改变其value就会影响baseValue，这样就会导致强化卡的basevalue一直增加改变

    public override void RemoveEffect(CharacterBase character)
    {
        List<CardDataSO> attackCards = CardDeck.instance.GetAllCardDataByName("攻击");

        // 还原攻击卡的伤害值
        foreach (var cardData in attackCards)
        {
            foreach (var effect in cardData.effects)
            {
                if (effect is DamageEffect)
                {
                    effect.value = 6;

                    cardData.description = "对目标造成6点伤害";
                }
            }

            var card = CardDeck.instance.handCardObjectList.Find(c => c.cardData == cardData);
            if (card != null)
            {
                card.Init(cardData);
            }
        }

        // 移除玩家身上的状态效果
        character.RemoveStatusEffect(effectName);
    }
}
