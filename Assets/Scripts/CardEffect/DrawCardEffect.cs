using UnityEngine;

//ʵ�ֳ�2�ſ�Ч��
[CreateAssetMenu(fileName = "DrawCardEffect", menuName = "CardEffect/DrawCardEffect", order = 0)]
public class DrawCardEffect : Effect
{
    [Header("�㲥�鿨")]
    public IntEventSO drawCountEvent;
    public override void Execute(CharacterBase from, CharacterBase target)
    {
        drawCountEvent?.RaiseEvent(value, this);
    }
}