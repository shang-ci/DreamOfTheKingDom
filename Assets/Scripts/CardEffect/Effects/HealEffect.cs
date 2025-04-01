using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "HealEffect", menuName = "CardEffect/HealEffect")]
public class HealEffect : Effect
{
    public override void Execute(CharacterBase from, CharacterBase target)
    {
        target.HealHealth(value);
    }

    public override void Execute(CharacterBase from, List<CharacterBase> targets)
    {
        base.Execute(from, targets);
        foreach(var target in targets)
        {
            target.HealHealth(value);
        }
    }

    public override void Execute(CharacterBase target)
    {
        target.HealHealth(value);
    }
}
