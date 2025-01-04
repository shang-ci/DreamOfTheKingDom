using UnityEngine;

public abstract class Effect : ScriptableObject 
{
    public int value;    
    public EffectTargetType targetType;

    // ִ��Ч��
    public abstract void Execute(CharacterBase from, CharacterBase target);
}