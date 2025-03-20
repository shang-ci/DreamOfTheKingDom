using System.Collections.Generic;
using UnityEngine;


//猩红印记：对任意目标施加一点猩红印记，每当打出一张攻击卡时，对带有猩红印记的生物造成2x点伤害
[CreateAssetMenu(fileName = "CrimsonMarkStatusEffect", menuName = "StatusEffect/CrimsonMarkStatusEffect")]
public class CrimsonMarkStatusEffect : StatusEffect
{
    public override void ChangeTime(CharacterBase character)
    {
        // 猩红印记效果不在打出时触发
    }

    public override void ExecuteEffect(CharacterBase from, CharacterBase target)
    {
        from.TakeDamage(from.statusEffects["CrimsonMarkStatusEffect"] * 2);
    }

    public override void RemoveEffect(CharacterBase character)
    {
        List<CardDataSO> StatusCards = CardDeck.instance.GetAllCardDataByName("猩红印记");

        foreach (var cardData in StatusCards)
        {
            var originalStatusEffect = CardManager.instance.GetOriginalCardDataByClone(cardData);
            foreach (var effect in cardData.statusEffects)
            {
                if (effect is CrimsonMarkStatusEffect)
                {
                    effect.round = originalStatusEffect.statusEffects[0].round;
                }
            }
        }

        // 移除猩红印记效果
        character.RemoveStatusEffect(effectName);
    }
}