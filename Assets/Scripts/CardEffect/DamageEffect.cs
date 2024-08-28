using Unity.Mathematics;
using UnityEngine;

[CreateAssetMenu(fileName = "DamageEffect", menuName = "CardEffect/DamageEffect")]
public class DamageEffect : Effect
{
    public override void Execute(CharacterBase from, CharacterBase target)
    {
        if (target == null)
        {
            return;
        }
        switch (targetType)
        {
            case EffectTargetType.Target:
                var damage = (int) math.round(value * from.baseStrength);
                target.TakeDamage(damage);
                Debug.Log($"执行了{damage}点伤害!");
                break;
            case EffectTargetType.ALL:
                foreach (var enemy in GameObject.FindGameObjectsWithTag("Enemy"))
                {
                    enemy.GetComponent<CharacterBase>().TakeDamage(value);
                }
                break;
        }
    }
}