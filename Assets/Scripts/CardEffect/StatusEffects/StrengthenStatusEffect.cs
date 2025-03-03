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

    public override void ExecuteEffect(CharacterBase from, CharacterBase target)
    {
        // ��ȡ��ǰ��ζԾ������й�������
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

            // ���������еĿ�������UI
            //var card = CardDeck.instance.handCardObjectList.Find(c => c.cardData == cardData);//������carddata����ͬһ��data�ĸ�������find�Ƚϵ������ߵ����õ�ַ 
            var card = CardDeck.instance.handCardObjectList.Find(c => c.cardData.Equals(cardData));
            //Debug.Log("ǿ�������ҵ���������");
            if (card != null)
            {
                card.UpdateCardDataUI(cardData);
                Debug.Log("ǿ����������UI����");
            }
        }
    }


    //todo:����Ϸ�ؿ�/ս������ʱ����״̬��Ч��������Ȼǿ������valueһֱ����

    //�����и����⣬����ǿ������baseValue����ChangeTime���ʼ���ģ����������������ſ�ʱ�ͳ�ʼ���ģ������ڴ��ǿ����ʱ��ʼ���������ǰ����о����ı���value�ͻ�Ӱ��baseValue�������ͻᵼ��ǿ������basevalueһֱ���Ӹı�

    public override void RemoveEffect(CharacterBase character)
    {
        List<CardDataSO> strengthStatusCards = CardDeck.instance.GetAllCardDataByName("ǿ��");
        

        foreach (var cardData in strengthStatusCards)
        {
            var originalStatusEffect = CardManager.Instance.GetOriginalCardDataByClone(cardData);
            foreach (var effect in cardData.statusEffects)
            {
                if (effect is StrengthenStatusEffect)
                {
                    effect.round = originalStatusEffect.statusEffects[0].round;
                }
            }
        }

        List<CardDataSO> attackCards = CardDeck.instance.GetAllCardDataByName("����");

        foreach (var cardData in attackCards)
        {
            var originalCardData = CardManager.Instance.GetOriginalCardDataByClone(cardData);

            foreach (var effect in cardData.effects)
            {
                if (effect is DamageEffect)
                {
                    effect.value = originalCardData.effects[0].value;

                    cardData.description = originalCardData.description;
                }
            }

            // ���������еĿ�������UI
            var card = CardDeck.instance.handCardObjectList.Find(c => c.cardData.Equals(originalCardData));
            if (card != null)
            {
                card.UpdateCardDataUI(originalCardData);
            }
        }

        // �Ƴ�������ϵ�״̬Ч��
        character.RemoveStatusEffect(effectName);
    }
}
