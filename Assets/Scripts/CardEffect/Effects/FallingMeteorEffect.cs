using UnityEngine;

[CreateAssetMenu(fileName = "FallingMeteorEffect", menuName = "CardEffect/FallingMeteorEffect")]
public class FallingMeteorEffect : Effect
{
    public override void Execute(CharacterBase from, CharacterBase target)
    {
        // �������1���˺�10��
        for (int i = 0; i < value; i++)
        {
            target.TakeDamage(1);
        }
    }
}