using System.Collections.Generic;
using UnityEngine;

//��Ƥ
[CreateAssetMenu(fileName = "ThickSkinStatusEffect", menuName = "StatusEffect/ThickSkinStatusEffect")]
public class ThickSkinStatusEffect : StatusEffect
{
    public override void ChangeTime(CharacterBase character)
    {
        // ʵ��Ч������ʱ��ı仯�߼�
    }

    public override void ExecuteEffect(CharacterBase from, CharacterBase target)
    {
        from.currentDamage -= from.statusEffects["ThickSkinStatusEffect"];
    }

    public override void RemoveEffect(CharacterBase character)
    {
        List<CardDataSO> strengthStatusCards = CardDeck.instance.GetAllCardDataByName("��Ƥ");


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

        // �Ƴ���ƤЧ��
        character.RemoveStatusEffect(effectName);
    }
}