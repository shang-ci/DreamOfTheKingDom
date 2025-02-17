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
                var damage = (int) math.round(value * from.baseStrength);//四舍五入计算伤害
                target.TakeDamage(damage);

                if(target.statusEffects.ContainsKey("ThornStatusEffect"))
                {
                    //荆棘效果
                    //from.TakeDamage((int)math.round(target.statusEffects["ThornStatusEffect"] * from.baseStrength));
                    from.TakeDamage(target.statusEffects["ThornStatusEffect"]);//根据身上的荆棘点数决定反伤的伤害
                }

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