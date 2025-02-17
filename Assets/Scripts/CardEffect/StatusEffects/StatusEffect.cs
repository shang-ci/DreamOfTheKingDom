using UnityEngine;


public abstract class StatusEffect : ScriptableObject
{
    public string effectName;
    public int value, baseValue;
    public EffectTiming timing;

    public void SetBaseValue()
    {
        baseValue = value;
    }

    public abstract void ChangeTime(CharacterBase character);
    public abstract void ExecuteEffect(CharacterBase character);
    public abstract void RemoveEffect(CharacterBase character);
}
