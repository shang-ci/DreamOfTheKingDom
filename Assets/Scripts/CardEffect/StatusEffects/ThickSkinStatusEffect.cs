using System.Collections.Generic;
using UnityEngine;

//厚皮
[CreateAssetMenu(fileName = "ThickSkinStatusEffect", menuName = "StatusEffect/ThickSkinStatusEffect")]
public class ThickSkinStatusEffect : StatusEffect
{
    public override void ChangeTime(CharacterBase character)
    {
        // 实现效果持续时间的变化逻辑
    }

    public override void ExecuteEffect(CharacterBase from, CharacterBase target)
    {
        from.currentDamage -= from.statusEffects["ThickSkinStatusEffect"];
    }

    public override void RemoveEffect(CharacterBase character)
    {
        List<CardDataSO> strengthStatusCards = CardDeck.instance.GetAllCardDataByName("厚皮");


        foreach (var cardData in strengthStatusCards)
        {
            var originalStatusEffect = CardManager.Instance.GetOriginalCardDataByClone(cardData);
            foreach (var effect in cardData.statusEffects)
            {
                if (effect is ThickSkinStatusEffect)
                {
                    effect.round = originalStatusEffect.statusEffects[0].round;
                }
            }
        }

        // 移除厚皮效果
        character.RemoveStatusEffect(effectName);
    }
}