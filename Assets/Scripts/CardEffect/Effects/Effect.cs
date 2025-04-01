using System.Collections.Generic;
using UnityEngine;

public abstract class Effect : ScriptableObject 
{
    public int value;    
    public EffectTargetType targetType;

    // Ö´ÐÐÐ§¹û
    public virtual void Execute(CharacterBase from, CharacterBase target) { }
    public virtual void Execute(CharacterBase from , List<CharacterBase> targets) { }
    public virtual void Execute(CharacterBase target) { }


    public virtual void Initialize(int value, EffectTargetType targetType)
    {
        this.value = value;
        this.targetType = targetType;
    }
}