using UnityEngine;

[CreateAssetMenu(fileName = "DefenseEffect", menuName = "CardEffect/DefenseEffect")]
public class DefenseEffect : Effect
{
    public override void Execute(CharacterBase from, CharacterBase target)
    {
        if (targetType == EffectTargetType.Self)
        {
            // 自己给自己加盾
            from.UpdateDefense(value);
        }

        if (targetType == EffectTargetType.Target)
        {
            // 多个敌人时，一个敌人给另一个敌人加盾
            target.UpdateDefense(value);
        }
    }
}
