using System.Collections.Generic;
using UnityEngine;

//荆棘
[CreateAssetMenu(fileName = "ThornStatusEffect", menuName = "StatusEffect/ThornStatusEffect")]
public class ThornStatusEffect : StatusEffect
{
    public override void ChangeTime(CharacterBase character)
    {
        // 荆棘效果不需要在特定时间点触发
    }

    //不受厚皮等的影响——直接输出伤害
    public override void ExecuteEffect(CharacterBase from, CharacterBase target)
    {
        target.TakeDamage2(from.statusEffects["ThornStatusEffect"]);
    }

    public override void ExecuteEffect(CharacterBase from, List<CharacterBase> targets)
    {
        throw new System.NotImplementedException();
    }

    public override void ExecuteEffect(CharacterBase target)
    {
        throw new System.NotImplementedException();
    }

    public override void RemoveEffect(CharacterBase character)
    {
        List<CardDataSO> strengthStatusCards = CardDeck.instance.GetAllCardDataByName("荆棘");


        foreach (var cardData in strengthStatusCards)
        {
            var originalStatusEffect = CardManager.instance.GetOriginalCardDataByClone(cardData);
            foreach (var effect in cardData.statusEffects)
            {
                if (effect is ThornStatusEffect)
                {
                    effect.round = originalStatusEffect.statusEffects[0].round;
                }
            }
        }
        character.thornRoundCount = 0;
        // 移除荆棘效果
        character.RemoveStatusEffect(effectName);
    }
}