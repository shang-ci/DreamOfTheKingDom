using System.Collections.Generic;
using UnityEngine;

//����
[CreateAssetMenu(fileName = "ThornStatusEffect", menuName = "StatusEffect/ThornStatusEffect")]
public class ThornStatusEffect : StatusEffect
{
    public override void ChangeTime(CharacterBase character)
    {
        // ����Ч������Ҫ���ض�ʱ��㴥��
    }

    //���ܺ�Ƥ�ȵ�Ӱ�졪��ֱ������˺�
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
        List<CardDataSO> strengthStatusCards = CardDeck.instance.GetAllCardDataByName("����");


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
        // �Ƴ�����Ч��
        character.RemoveStatusEffect(effectName);
    }
}