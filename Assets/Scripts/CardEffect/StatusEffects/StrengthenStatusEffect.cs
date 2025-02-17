using System.Collections.Generic;
using UnityEngine;

//ǿ������Ч��
[CreateAssetMenu(fileName = "StrengthenStatusEffect", menuName = "StatusEffect/StrengthenStatusEffect")]
public class StrengthenStatusEffect : StatusEffect
{
    public override void ChangeTime(CharacterBase character)
    {
        //SetBaseValue();

        EffectTimingManager.Instance.ChangeEffectTiming(EffectTiming.Strengthen);
    }

    public override void ExecuteEffect(CharacterBase character)
    {
        // ��ȡ��ǰ���ƿ��е����й�������
        List<CardDataSO> attackCards = CardDeck.instance.GetAllCardDataByName("����");

        // ���ӹ������Ƶ��˺�ֵ
        foreach (var cardData in attackCards)
        {
            foreach (var effect in cardData.effects)
            {
                if (effect is DamageEffect)
                {
                    var t = effect.value + value;
                    effect.value = t;

                    cardData.description = "��Ŀ�����" + t + "���˺�";
                }
            }
            //ֻ��Ҫ���������еĿ���UI
            CardDeck.instance.handCardObjectList.Find(card => card.cardData == cardData).Init(cardData);
        }
    }


    //todo:����Ϸ�ؿ�/ս������ʱ����״̬��Ч��������Ȼǿ������valueһֱ����

    //�����и����⣬����ǿ������baseValue����ChangeTime���ʼ���ģ����������������ſ�ʱ�ͳ�ʼ���ģ������ڴ��ǿ����ʱ��ʼ���������ǰ����о����ı���value�ͻ�Ӱ��baseValue�������ͻᵼ��ǿ������basevalueһֱ���Ӹı�

    public override void RemoveEffect(CharacterBase character)
    {
        List<CardDataSO> attackCards = CardDeck.instance.GetAllCardDataByName("����");

        // ��ԭ���������˺�ֵ
        foreach (var cardData in attackCards)
        {
            foreach (var effect in cardData.effects)
            {
                if (effect is DamageEffect)
                {
                    effect.value = 6;

                    cardData.description = "��Ŀ�����6���˺�";
                }
            }

            var card = CardDeck.instance.handCardObjectList.Find(c => c.cardData == cardData);
            if (card != null)
            {
                card.Init(cardData);
            }
        }

        // �Ƴ�������ϵ�״̬Ч��
        character.RemoveStatusEffect(effectName);
    }
}
