using UnityEngine;

[CreateAssetMenu(fileName = "HealEffect", menuName = "CardEffect/HealEffect")]
public class HealEffect : Effect
{
    public override void Execute(CharacterBase from, CharacterBase target)
    {
        target.HealHealth(value);
    }
}
