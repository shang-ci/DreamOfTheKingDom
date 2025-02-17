using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ResearchEffect", menuName = "StatusEffect/ResearchEffect")]
public class ResearchStatusEffect : StatusEffect
{

    public override void ChangeTime(CharacterBase character)
    {
        SetBaseValue();

        // �Ƴ��о�Ч�����߼�
        //character.RemoveStatusEffect(effectName);
        EffectTimingManager.Instance.ChangeEffectTiming(EffectTiming.Reseach);
    }

    public override void ExecuteEffect(CharacterBase character)
    {
        // ��ȡ��ǰ���ƿ��е�����ǿ����
        List<CardDataSO> strengthenCards = CardDeck.instance.GetAllCardDataByName("ǿ��");

        // ��ȡ��ɫ���ϵ��о�״̬�ĵ���
        int researchValue = character.GetStatusEffectValue(effectName);

        // ���о�״̬�ĵ����ӵ�ÿ��ǿ������ value ��
        foreach (var cardData in strengthenCards)
        {
            foreach (var effect in cardData.statusEffects)
            {
                if (effect is StrengthenStatusEffect)
                {
                    var t = 2 + researchValue;//�����Ӧ������baseValue�����㣬��Ȼ�ᱩ��
                    effect.value = t;

                    cardData.description = "���ӹ���" + t + "��";
                }
            }

            // ֻ��Ҫ���������еĿ���UI
            var card = CardDeck.instance.handCardObjectList.Find(c => c.cardData == cardData);

            //��ֹ�����쳣
            if (card != null)
            {
                card.Init(cardData);
            }
        }
    }

    public override void RemoveEffect(CharacterBase character)
    {
        // ��ȡ��ǰ���ƿ��е�����ǿ����
        List<CardDataSO> strengthenCards = CardDeck.instance.GetAllCardDataByName("ǿ��");

        // ��ԭǿ�������˺�ֵ
        foreach (var cardData in strengthenCards)
        {
            foreach (var effect in cardData.statusEffects)
            {
                if (effect is StrengthenStatusEffect)
                {
                    effect.value = 2;

                    cardData.description = "���ӹ���" + effect.value + "��";
                }
            }

            // ֻ��Ҫ���������еĿ���UI
            var card = CardDeck.instance.handCardObjectList.Find(c => c.cardData == cardData);

            //��ֹ�����쳣
            if (card != null)
            {
                card.Init(cardData);
            }
        }

        // �Ƴ�������ϵ�״̬Ч��
        character.RemoveStatusEffect(effectName);
    }
}