using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    private Player player;
    private Animator animator;

    private void Awake()
    {
        player = GetComponent<Player>();
        animator = GetComponentInChildren<Animator>();
    }

    private void OnEnable() 
    {
        animator.Play("sleep");
        animator.SetBool("isSleep", true);
    }

    //处于玩家回合时，不会进行防御和睡觉——当玩家回合开始时，即监听玩家回合开始事件
    public void PlayerTurnBeginAnimation()
    {
        animator.SetBool("isSleep", false);
        animator.SetBool("isParry", false);
    }

    //播放玩家睡觉/防御动画——当玩家回合结束时，处于敌人回合，即监听玩家回合结束事件
    public void PlayerTurnEndAnimation()
    {
        if (player.defense.currentValue > 0)
        {
            animator.SetBool("isSleep", false);
            animator.SetBool("isParry", true);
        }
        else
        {
            animator.SetBool("isSleep", true);
            animator.SetBool("isParry", false);
        }
    }

    //播放skill动画和attack动画——当玩家使用卡牌时，即监听卡牌回收事件
    public void OnPlayerCardEvent(object obj)
    {
        Card card = obj as Card;

        switch (card.cardData.cardType)
        {
            case CardType.Attack:
                animator.SetTrigger("attack");
                break;
            case CardType.Defense:
            case CardType.Abilities:
                animator.SetTrigger("skill");
                break;
        }
    }

    //睡觉动画——当进入休息室时，由判断房间类型的脚本调用
    public void SetSleepAnimation()
    {
        animator.Play("death");
    }
}
