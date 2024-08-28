using System.Collections;
using UnityEngine;

public class Enemy : CharacterBase
{
    public EnemyActionDataSO actionDataSO;
    public EnemyAction currentAction;

    protected Player player;

    public virtual void OnPlayerTurnBegin()
    {
        var randomIndex = Random.Range(0, actionDataSO.actions.Count);
        currentAction = actionDataSO.actions[randomIndex];
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
    }

    public virtual void OnEnemyTurnBegin()
    {
        switch (currentAction.effect.targetType)
        {
            case EffectTargetType.Self:
                Skill();
                break;
            case EffectTargetType.Target:
                Attack();
                break;
            case EffectTargetType.ALL:
                break;
        }
    }

    public virtual void Skill()
    {
        // animator.SetTrigger("skill");
        // currentAction.effect.Execute(this, this);
        StartCoroutine(ProcessDelayAction("skill"));
    }

    public virtual void Attack()
    {
        // animator.SetTrigger("attack");
        // currentAction.effect.Execute(this, player);
        StartCoroutine(ProcessDelayAction("attack"));
    }

    IEnumerator ProcessDelayAction(string actionName)
    {
        animator.SetTrigger(actionName);
        // 等到动画播放了 60% 并且不是在 layer0 的转换过程中 并且动画名是 actionNmae，才往后执行
        yield return new WaitUntil(() => (animator.GetCurrentAnimatorStateInfo(0).normalizedTime % 1.0f > 0.6f) && 
                                        !animator.IsInTransition(0) &&
                                        animator.GetCurrentAnimatorStateInfo(0).IsName(actionName));
        if (actionName == "attack")
        {
            currentAction.effect.Execute(this, player);
        }
        else
        {
            currentAction.effect.Execute(this, this);
        }
    }
}
