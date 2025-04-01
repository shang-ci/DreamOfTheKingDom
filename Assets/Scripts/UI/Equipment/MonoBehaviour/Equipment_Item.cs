using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//管理装备的格子
public class Equipment_Item : MonoBehaviour
{
    [Header("基本属性")]
    public Image icon;
    public Equipment_ItemData item;
    private int id;


    [Header("效果目标")]
    public CharacterBase target;
    public List<CharacterBase> targets;


    public void SetEquipmentItem(Equipment_ItemData item)
    {
        this.item = item;
        this.id = item.id;
        icon.sprite = item.icon;
    }


    #region 执行效果
    public void Execute(CharacterBase from, CharacterBase target)
    {
        if (item.Effect != null)
            item.Effect.Execute(from, target);
        else if (item.StatusEffect != null)
            item.StatusEffect.ExecuteEffect(from, target);
    }

    public void Execute(CharacterBase from, List<CharacterBase> targets)
    {
        if (item.Effect != null)
            item.Effect.Execute(from, targets);
        else if (item.StatusEffect != null)
            item.StatusEffect.ExecuteEffect(from, targets);
    }

    public void Execute(CharacterBase target)
    {
        if (item.Effect != null)
            item.Effect.Execute(target);
        else if (item.StatusEffect != null)
            item.StatusEffect.ExecuteEffect(target);
    }
    #endregion
}
