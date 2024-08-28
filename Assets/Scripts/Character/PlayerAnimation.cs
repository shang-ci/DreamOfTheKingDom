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

    public void PlayerTurnBeginAnimation()
    {
        animator.SetBool("isSleep", false);
        animator.SetBool("isParry", false);
    }

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

    public void SetSleepAnimation()
    {
        animator.Play("death");
    }
}
