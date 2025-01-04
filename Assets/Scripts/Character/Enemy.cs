using System.Collections;
using UnityEngine;

public class Enemy : CharacterBase
{
    public EnemyActionDataSO actionDataSO;
    public EnemyAction currentAction;

    protected Player player;

    //产生意图——不过这里只能有一个意图，想实现多个效果，可以在这里加一个数组/制作复合的effect
    public virtual void OnPlayerTurnBegin()
    {
        var randomIndex = Random.Range(0, actionDataSO.actions.Count);
        currentAction = actionDataSO.actions[randomIndex];
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();//放在这里获得是因为在Start中可能还没有player，这样就可以保证player不为空
    }

    //执行意图——在敌人回合开始时执行
    public virtual void OnEnemyTurnBegin()
    {
        ResetDefense();//每回合开始重置防御

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

    //处理延迟动作——在动画播放到一定程度时执行——避免UI更新和动画播放不同步；其实也可以在动画片段中加入事件
    IEnumerator ProcessDelayAction(string actionName)
    {
        animator.SetTrigger(actionName);
        // 等到动画播放了 60% 并且不是在 layer0 的转换过程中 并且动画名是 actionNmae，才往后执行
        yield return new WaitUntil(() => (animator.GetCurrentAnimatorStateInfo(0).normalizedTime % 1.0f > 0.6f) && !animator.IsInTransition(0) && animator.GetCurrentAnimatorStateInfo(0).IsName(actionName));
        
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
