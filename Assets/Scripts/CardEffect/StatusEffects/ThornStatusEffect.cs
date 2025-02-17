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

    public override void ExecuteEffect(CharacterBase character)
    {
        // ����Ч�����ܵ��˺�ʱ����
        //character.TakeDamage(value);
    }

    public override void RemoveEffect(CharacterBase character)
    {
        // �Ƴ�����Ч��
        character.RemoveStatusEffect(effectName);
    }
}