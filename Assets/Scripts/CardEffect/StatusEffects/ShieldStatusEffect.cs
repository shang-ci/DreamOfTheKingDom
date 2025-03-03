using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ShieldStatusEffect", menuName = "StatusEffect/ShieldStatusEffect")]
public class ShieldStatusEffect : StatusEffect
{
    public override void ChangeTime(CharacterBase character)
    {
        
    }

    public override void ExecuteEffect(CharacterBase from, CharacterBase target)
    {
        if (from.statusEffects["ShieldStatusEffect"] > from.currentDamage)
        {
            from.currentDamage = 0;
        }
        else
        {
            from.currentDamage -= from.statusEffects["ShieldStatusEffect"];
            from.statusEffects["ShieldStatusEffect"] = 0;
            RemoveEffect(from);
        }
    }

    public override void RemoveEffect(CharacterBase character)
    {
        List<CardDataSO> strengthStatusCards = CardDeck.instance.GetAllCardDataByName("护盾");


        foreach (var cardData in strengthStatusCards)
        {
            var originalStatusEffectCardData = CardManager.Instance.GetOriginalCardDataByClone(cardData);
            foreach (var effect in cardData.statusEffects)
            {
                if (effect is ShieldStatusEffect)
                {
                    effect.round = originalStatusEffectCardData.statusEffects[0].round;
                }
            }
        }

        // 移除护盾效果
        character.RemoveStatusEffect(effectName);
    }
}