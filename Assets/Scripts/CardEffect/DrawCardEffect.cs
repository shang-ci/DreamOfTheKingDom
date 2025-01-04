using UnityEngine;

//实现抽2张卡效果
[CreateAssetMenu(fileName = "DrawCardEffect", menuName = "CardEffect/DrawCardEffect", order = 0)]
public class DrawCardEffect : Effect
{
    [Header("广播抽卡")]
    public IntEventSO drawCountEvent;
    public override void Execute(CharacterBase from, CharacterBase target)
    {
        drawCountEvent?.RaiseEvent(value, this);
    }
}