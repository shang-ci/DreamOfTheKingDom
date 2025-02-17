using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ResearchEffect", menuName = "StatusEffect/ResearchEffect")]
public class ResearchStatusEffect : StatusEffect
{

    public override void ChangeTime(CharacterBase character)
    {
        SetBaseValue();

        // 移除研究效果的逻辑
        //character.RemoveStatusEffect(effectName);
        EffectTimingManager.Instance.ChangeEffectTiming(EffectTiming.Reseach);
    }

    public override void ExecuteEffect(CharacterBase character)
    {
        // 获取当前卡牌库中的所有强化卡
        List<CardDataSO> strengthenCards = CardDeck.instance.GetAllCardDataByName("强化");

        // 获取角色身上的研究状态的点数
        int researchValue = character.GetStatusEffectValue(effectName);

        // 将研究状态的点数加到每张强化卡的 value 上
        foreach (var cardData in strengthenCards)
        {
            foreach (var effect in cardData.statusEffects)
            {
                if (effect is StrengthenStatusEffect)
                {
                    var t = 2 + researchValue;//这里的应该是用baseValue来计算，不然会暴增
                    effect.value = t;

                    cardData.description = "增加攻击" + t + "点";
                }
            }

            // 只需要更新手牌中的卡牌UI
            var card = CardDeck.instance.handCardObjectList.Find(c => c.cardData == cardData);

            //防止报空异常
            if (card != null)
            {
                card.Init(cardData);
            }
        }
    }

    public override void RemoveEffect(CharacterBase character)
    {
        // 获取当前卡牌库中的所有强化卡
        List<CardDataSO> strengthenCards = CardDeck.instance.GetAllCardDataByName("强化");

        // 还原强化卡的伤害值
        foreach (var cardData in strengthenCards)
        {
            foreach (var effect in cardData.statusEffects)
            {
                if (effect is StrengthenStatusEffect)
                {
                    effect.value = 2;

                    cardData.description = "增加攻击" + effect.value + "点";
                }
            }

            // 只需要更新手牌中的卡牌UI
            var card = CardDeck.instance.handCardObjectList.Find(c => c.cardData == cardData);

            //防止报空异常
            if (card != null)
            {
                card.Init(cardData);
            }
        }

        // 移除玩家身上的状态效果
        character.RemoveStatusEffect(effectName);
    }
}