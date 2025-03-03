using UnityEngine;

[CreateAssetMenu(fileName = "FallingMeteorEffect", menuName = "CardEffect/FallingMeteorEffect")]
public class FallingMeteorEffect : Effect
{
    public override void Execute(CharacterBase from, CharacterBase target)
    {
        // 连续造成1点伤害10次
        for (int i = 0; i < value; i++)
        {
            target.TakeDamage(1);
        }
    }
}