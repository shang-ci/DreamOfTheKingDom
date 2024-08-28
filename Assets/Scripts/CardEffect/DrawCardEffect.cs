using UnityEngine;

[CreateAssetMenu(fileName = "DrawCardEffect", menuName = "CardEffect/DrawCardEffect", order = 0)]
public class DrawCardEffect : Effect
{
    public IntEventSO drawCountEvent;
    public override void Execute(CharacterBase from, CharacterBase target)
    {
        drawCountEvent?.RaiseEvent(value, this);
    }
}