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

    public override void ExecuteEffect(CharacterBase from, CharacterBase target)
    {
        // 获取当前这次对局里所有攻击卡牌
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

            // 更新手牌中的卡牌数据UI
            //var card = CardDeck.instance.handCardObjectList.Find(c => c.cardData == cardData);//这两个carddata都是同一个data的副本——find比较的是两者的引用地址 
            var card = CardDeck.instance.handCardObjectList.Find(c => c.cardData.Equals(cardData));
            //Debug.Log("强化——找到攻击卡了");
            if (card != null)
            {
                card.UpdateCardDataUI(cardData);
                Debug.Log("强化——副本UI更新");
            }
        }
    }


    //todo:当游戏重开/战斗结束时消除状态的效果——不然强化卡的value一直增加

    //这里有个问题，就是强化卡的baseValue是在ChangeTime里初始化的，并不是在生成这张卡时就初始化的，它是在打出强化卡时初始化，如果提前打出研究卡改变其value就会影响baseValue，这样就会导致强化卡的basevalue一直增加改变

    public override void RemoveEffect(CharacterBase character)
    {
        List<CardDataSO> strengthStatusCards = CardDeck.instance.GetAllCardDataByName("强化");
        

        foreach (var cardData in strengthStatusCards)
        {
            var originalStatusEffect = CardManager.Instance.GetOriginalCardDataByClone(cardData);
            foreach (var effect in cardData.statusEffects)
            {
                if (effect is StrengthenStatusEffect)
                {
                    effect.round = originalStatusEffect.statusEffects[0].round;
                }
            }
        }

        List<CardDataSO> attackCards = CardDeck.instance.GetAllCardDataByName("攻击");

        foreach (var cardData in attackCards)
        {
            var originalCardData = CardManager.Instance.GetOriginalCardDataByClone(cardData);

            foreach (var effect in cardData.effects)
            {
                if (effect is DamageEffect)
                {
                    effect.value = originalCardData.effects[0].value;

                    cardData.description = originalCardData.description;
                }
            }

            // 更新手牌中的卡牌数据UI
            var card = CardDeck.instance.handCardObjectList.Find(c => c.cardData.Equals(originalCardData));
            if (card != null)
            {
                card.UpdateCardDataUI(originalCardData);
            }
        }

        // 移除玩家身上的状态效果
        character.RemoveStatusEffect(effectName);
    }
}
