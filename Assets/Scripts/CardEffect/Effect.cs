using UnityEngine;

public abstract class Effect : ScriptableObject 
{
    public int value;    
    public EffectTargetType targetType;

    // Ö´ÐÐÐ§¹û
    public abstract void Execute(CharacterBase from, CharacterBase target);
}