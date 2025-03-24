using UnityEngine;
using UnityEngine.Events;

//管理装备数据的
[CreateAssetMenu(fileName = "Equipment_ItemData", menuName = "Item/Equipment_ItemData")]
public class Equipment_ItemData : Item
{
    //通过事件来完成效果——但现有的事件委托都不符合execute的参数列表
    [SerializeField] private ObjectEventSO eventSO;
    public UnityAction<CharacterBase, CharacterBase> eventSO3;
    //public BaseEventSO<CharacterBase, CharacterBase> eventSO2;
    //所有数据可能都有这两个
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
