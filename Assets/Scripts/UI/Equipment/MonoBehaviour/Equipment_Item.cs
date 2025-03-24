using UnityEngine;
using UnityEngine.UI;

//管理装备的格子
public class Equipment_Item : MonoBehaviour
{
    public Image icon;
    public Equipment_ItemData item;
    private int id;

    private void OnEnable()
    {
        item.Event.OnEventRaised += Execute;
    }

    private void OnDisable()
    {
        item.Event.OnEventRaised -= Execute;
    }

    public void SetEquipmentItem(Equipment_ItemData item)
    {
        this.item = item;
        this.id = item.id;
        icon.sprite = item.icon;
    }
    
    public void Execute(CharacterBase from, CharacterBase target)
    {
        item.Effect.Execute(from, target);
    }

    public void Execute(object target)
    {
        item.Effect.Execute(target as CharacterBase);
    }
}
