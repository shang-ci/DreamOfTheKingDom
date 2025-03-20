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

    public override void ExecuteEffect(CharacterBase from, CharacterBase target)
    {
        // ��ȡ��ǰ���ƿ��е�����ǿ����
        List<CardDataSO> strengthenCards = CardDeck.instance.GetAllCardDataByName("ǿ��");

        // ��ȡ��ɫ���ϵ��о�״̬�ĵ���
        int researchValue = from.GetStatusEffectValue(effectName);

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
            Debug.Log("�о������ҵ�ǿ������");
            //��ֹ�����쳣
            if (card != null)
            {
                card.UpdateCardDataUI(cardData);
                Debug.Log("�о���������UI����");
            }
        }
    }

    public override void RemoveEffect(CharacterBase character)
    {
        List<CardDataSO> strengthStatusCards = CardDeck.instance.GetAllCardDataByName("�о�");


        foreach (var cardData in strengthStatusCards)
        {
            var originalStatusEffect = CardManager.instance.GetOriginalCardDataByClone(cardData);
            foreach (var effect in cardData.statusEffects)
            {
                if (effect is ResearchStatusEffect)
                {
                    effect.round = originalStatusEffect.statusEffects[0].round;
                }
            }
        }

        // ��ȡ��ǰ���ƿ��е�����ǿ����
        List<CardDataSO> strengthenCards = CardDeck.instance.GetAllCardDataByName("ǿ��");

        // ��ԭǿ�������˺�ֵ
        foreach (var cardData in strengthenCards)
        {
            var originalCardData = CardManager.instance.GetOriginalCardDataByClone(cardData);

            foreach (var effect in cardData.statusEffects)
            {
                if (effect is StrengthenStatusEffect)
                {
                    effect.value = originalCardData.statusEffects[0].value;

                    cardData.description = originalCardData.description;
                }
            }

            // ֻ��Ҫ���������еĿ���UI
            var card = CardDeck.instance.handCardObjectList.Find(c => c.Equals(originalCardData));

            //��ֹ�����쳣
            if (card != null)
            {
                card.UpdateCardDataUI(cardData);
            }
        }

        // �Ƴ�������ϵ�״̬Ч��
        character.RemoveStatusEffect(effectName);
    }
}