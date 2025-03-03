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

                //TOOD:这里的判断不好，因该在卡牌拖动时判断卡牌/carddata的相关类型再触发
                if (target is Enemy && target.statusEffects.ContainsKey("CrimsonMarkStatusEffect"))
                    EffectTimingManager.Instance.ChangeEffectTiming(EffectTiming.EnemyCrimsonMark);

                if (target is Player && target.statusEffects.ContainsKey("CrimsonMarkStatusEffect"))
                    EffectTimingManager.Instance.ChangeEffectTiming(EffectTiming.PlayerCrimsonMark);


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