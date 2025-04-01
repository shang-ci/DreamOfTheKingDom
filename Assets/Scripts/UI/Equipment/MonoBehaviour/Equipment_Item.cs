using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//����װ���ĸ���
public class Equipment_Item : MonoBehaviour
{
    [Header("��������")]
    public Image icon;
    public Equipment_ItemData item;
    private int id;


    [Header("Ч��Ŀ��")]
    public CharacterBase target;
    public List<CharacterBase> targets;


    public void SetEquipmentItem(Equipment_ItemData item)
    {
        this.item = item;
        this.id = item.id;
        icon.sprite = item.icon;
    }


    #region ִ��Ч��
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
