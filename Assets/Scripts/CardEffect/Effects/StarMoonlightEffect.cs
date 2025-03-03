using UnityEngine;

//星月之光:抽卡池每有一个攻击类型的（Attack）牌就造成value倍伤害
[CreateAssetMenu(fileName = "StarMoonlightEffect", menuName = "CardEffect/StarMoonlightEffect", order = 0)]
public class StarMoonlightEffect : Effect
{
    public override void Execute(CharacterBase from, CharacterBase target)
    { 
        // 计算伤害
        int damage = value * CardDeck.instance.GetDrawDeckCountByType();

        // 对目标造成伤害
        target.TakeDamage(damage);
    }
}