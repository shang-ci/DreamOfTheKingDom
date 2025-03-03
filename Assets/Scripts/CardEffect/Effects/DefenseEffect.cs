using UnityEngine;

[CreateAssetMenu(fileName = "DefenseEffect", menuName = "CardEffect/DefenseEffect")]
public class DefenseEffect : Effect
{
    public override void Execute(CharacterBase from, CharacterBase target)
    {
        target.UpdateDefense(value);
    }
}
