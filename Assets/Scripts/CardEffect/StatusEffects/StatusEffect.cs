using System.Collections.Generic;
using UnityEngine;


public abstract class StatusEffect : ScriptableObject
{
    public string effectName;
    public int value, baseValue;
    public StatusEffectTargetType targetType;
    public int round;//持续回合数
    public EffectTiming timing;//目前来说大概所有的效果一个时机就可以了

    public void SetBaseValue()
    {
        baseValue = value;
    }

    //改变时机——在打出卡牌时触发状态效果的状态卡可以在此处改变时机
    public abstract void ChangeTime(CharacterBase character);
    public abstract void ExecuteEffect(CharacterBase from, CharacterBase target);
    public abstract void ExecuteEffect(CharacterBase from, List<CharacterBase> targets);
    public abstract void ExecuteEffect(CharacterBase target);
    public abstract void RemoveEffect(CharacterBase character);
}
