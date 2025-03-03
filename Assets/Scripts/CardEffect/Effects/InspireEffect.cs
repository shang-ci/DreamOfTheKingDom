using UnityEngine;

//�������������һ�ſ����Ե������10���˺�
[CreateAssetMenu(fileName = "InspireEffect", menuName = "CardEffect/InspireEffect", order = 0)]
public class InspireEffect : Effect
{
    [Header("�㲥����")]
    public IntEventSO discardRandomCardEvent;

    public override void Execute(CharacterBase from, CharacterBase target)
    {
        // �������x�ſ�
        discardRandomCardEvent?.RaiseEvent(value, this);
        //GameManager.Instance.aliveEnemyList[0].TakeDamage(10 * value);
        target.TakeDamage(10 * value);
    }
}