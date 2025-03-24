using UnityEngine;
using UnityEngine.Events;

//����װ�����ݵ�
[CreateAssetMenu(fileName = "Equipment_ItemData", menuName = "Item/Equipment_ItemData")]
public class Equipment_ItemData : Item
{
    //ͨ���¼������Ч�����������е��¼�ί�ж�������execute�Ĳ����б�
    [SerializeField] private ObjectEventSO eventSO;
    public UnityAction<CharacterBase, CharacterBase> eventSO3;
    //public BaseEventSO<CharacterBase, CharacterBase> eventSO2;
    //�������ݿ��ܶ���������
    [SerializeField]private Effect effect;
    [SerializeField]private StatusEffect statusEffect;


    public ObjectEventSO Event 
    {  get { return eventSO; } 
       set { eventSO = value; }
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
}
