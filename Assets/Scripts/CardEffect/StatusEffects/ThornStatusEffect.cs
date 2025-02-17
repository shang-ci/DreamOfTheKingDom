using System.Collections.Generic;
using UnityEngine;

//荆棘
[CreateAssetMenu(fileName = "ThornStatusEffect", menuName = "StatusEffect/ThornStatusEffect")]
public class ThornStatusEffect : StatusEffect
{
    public override void ChangeTime(CharacterBase character)
    {
        // 荆棘效果不需要在特定时间点触发
    }

    public override void ExecuteEffect(CharacterBase character)
    {
        // 荆棘效果在受到伤害时触发
        //character.TakeDamage(value);
    }

    public override void RemoveEffect(CharacterBase character)
    {
        // 移除荆棘效果
        character.RemoveStatusEffect(effectName);
    }
}