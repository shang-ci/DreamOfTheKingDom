using UnityEngine;

//激励：随机弃掷一张卡，对敌人造成10点伤害
[CreateAssetMenu(fileName = "InspireEffect", menuName = "CardEffect/InspireEffect", order = 0)]
public class InspireEffect : Effect
{
    [Header("广播弃牌")]
    public IntEventSO discardRandomCardEvent;

    public override void Execute(CharacterBase from, CharacterBase target)
    {
        // 随机弃掷x张卡
        discardRandomCardEvent?.RaiseEvent(value, this);
        //GameManager.Instance.aliveEnemyList[0].TakeDamage(10 * value);
        target.TakeDamage(10 * value);
    }
}