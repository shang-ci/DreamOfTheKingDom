using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using static UnityEditor.Progress;

//����װ�����ݵ�
[CreateAssetMenu(fileName = "Equipment_ItemData", menuName = "Item/Equipment_ItemData")]
public class Equipment_ItemData : Item
{
    //ͨ���¼������Ч�����������е��¼�ί�ж�������execute�Ĳ����б�
    //[SerializeField] private ObjectEventSO eventSO;
    //public UnityAction<CharacterBase, CharacterBase> eventSO3;
    //public BaseEventSO<CharacterBase, CharacterBase> eventSO2;
    //�������ݿ��ܶ���������
    [SerializeField]private Effect effect;
    [SerializeField]private StatusEffect statusEffect;
    [SerializeField]private EquipmentTargetType targetType;
    [SerializeField]private EffectTiming timing;

    public EquipmentTargetType TargetType
    {
        get { return targetType; }
        set { targetType = value; }
    }

    public EffectTiming Timing
    {
        get { return timing; }
        set { timing = value; }
    }

    public Effect Effect
    {
        get { return effect; }
        set { effect = value; }
    }
    public StatusEffect StatusEffect
    {
        get { return statusEffect; }
        set { statusEffect = value; }
    }


    #region ִ��Ч��
    public void Execute(CharacterBase from, CharacterBase target)
    {
        Effect.Execute(from, target);
    }

    public void Execute(CharacterBase from, List<CharacterBase> targets)
    {
        Effect.Execute(from, targets);
    }

    public void Execute(CharacterBase target)
    {
        Effect.Execute(target);
    }
    #endregion
}
