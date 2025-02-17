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

    //������һغ�ʱ��������з�����˯����������һغϿ�ʼʱ����������һغϿ�ʼ�¼�
    public void PlayerTurnBeginAnimation()
    {
        animator.SetBool("isSleep", false);
        animator.SetBool("isParry", false);
    }

    //�������˯��/����������������һغϽ���ʱ�����ڵ��˻غϣ���������һغϽ����¼�
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

    //����skill������attack�������������ʹ�ÿ���ʱ�����������ƻ����¼�
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

    //˯������������������Ϣ��ʱ�����жϷ������͵Ľű�����
    public void SetSleepAnimation()
    {
        animator.Play("death");
    }
}
