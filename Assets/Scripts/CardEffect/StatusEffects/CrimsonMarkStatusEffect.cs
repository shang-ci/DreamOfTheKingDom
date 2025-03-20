using System.Collections.Generic;
using UnityEngine;


//�ɺ�ӡ�ǣ�������Ŀ��ʩ��һ���ɺ�ӡ�ǣ�ÿ�����һ�Ź�����ʱ���Դ����ɺ�ӡ�ǵ��������2x���˺�
[CreateAssetMenu(fileName = "CrimsonMarkStatusEffect", menuName = "StatusEffect/CrimsonMarkStatusEffect")]
public class CrimsonMarkStatusEffect : StatusEffect
{
    public override void ChangeTime(CharacterBase character)
    {
        // �ɺ�ӡ��Ч�����ڴ��ʱ����
    }

    public override void ExecuteEffect(CharacterBase from, CharacterBase target)
    {
        from.TakeDamage(from.statusEffects["CrimsonMarkStatusEffect"] * 2);
    }

    public override void RemoveEffect(CharacterBase character)
    {
        List<CardDataSO> StatusCards = CardDeck.instance.GetAllCardDataByName("�ɺ�ӡ��");

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

        // �Ƴ��ɺ�ӡ��Ч��
        character.RemoveStatusEffect(effectName);
    }
}