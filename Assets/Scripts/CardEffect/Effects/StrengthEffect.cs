using UnityEngine;

[CreateAssetMenu(fileName = "StrengthEffect", menuName = "CardEffect/StrengthEffect")]
public class StrengthEffect : Effect
{
    public override void Execute(CharacterBase from, CharacterBase target)
    {
        switch (targetType)
        {
            case EffectTargetType.Self:
                target.SetupStrength(value, true);
                break;
            case EffectTargetType.Target:
                target.SetupStrength(value, false);
                break;
            case EffectTargetType.ALL:
                break;
        }
    }
}
