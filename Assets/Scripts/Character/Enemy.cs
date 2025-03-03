using System.Collections;
using UnityEngine;

public class Enemy : CharacterBase
{
    public EnemyActionDataSO actionDataSO;
    public EnemyAction currentAction;

    protected Player player;
    //private Aptitude aptitude;//资质
    private Attribute attribute;//属性
    private Seal seal;//道印

    protected override void Start()
    {
        base.Start();

        attribute = Attribute.gold;
        seal = new Seal(Attribute.gold);
    }

    //产生意图——从actions里根据触发率选择意图——不过这里只能有一个意图，想实现多个效果，可以在这里加一个数组/制作复合的effect
    public virtual void OnPlayerTurnBegin()
    {
        //计算所有动作的总概率
        float totalProbability = 0f;
        foreach (var action in actionDataSO.actions)
        {
            totalProbability += action.probability;//这样就不必保证概率总和为1
        }

        //0到totalProbability的随机数
        float randomPoint = Random.value * totalProbability;

        //选择与randomPoint相匹配的action
        float cumulativeProbability = 0f; // 累积概率
        foreach (var action in actionDataSO.actions)
        {
            cumulativeProbability += action.probability;
            if (randomPoint < cumulativeProbability)
            {
                // 如果randomPoint在当前动作的概率区间内，选择当前动作
                currentAction = action;
                break;
            }
        }

        // 保存原始值
        //currentAction.originalValue = currentAction.effect.value;

        //精准&&暴击判定
        if (Random.value <= currentAction.accuracy)
        {
            currentAction.effect.value = (int)(currentAction.effect.value * currentAction.criticalRate); //暴击
        }
        else
        {
            Debug.Log(" 不暴击");
        }

        //放在这里获得是因为在Start中可能还没有player，这样就可以保证player不为空
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
    }


    //执行意图——在敌人回合开始时执行——点击回合转换按钮时敌人回合开始
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

    //执行行为的暴击部分——处理延迟动作——在动画播放到一定程度时执行——避免UI更新和动画播放不同步；其实也可以在动画片段中加入事件
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

    public override void ExecuteStatusEffects(EffectTiming timing)
    {
        base.ExecuteStatusEffects(timing);
        foreach (var effect in activeEffects)
        {
            if (effect.timing == timing)
            {
                effect.ExecuteEffect(this, player);
            }
        }
    }


    //集合遍历时被修改会触发InvalidOperationException异常，所以这里使用for循环/先记录要删除的元素后面再foreach删除
    //敌人的状态效果回合数更新——这里就不用事件来调用了，要是敌人数量很多那岂不是要给每个敌人都添加一个事件监听太费事了
    //public override void UpdateStatusEffectRounds()
    //{
    //    base.UpdateStatusEffectRounds();
    //    foreach (var statusEffect in activeEffects)
    //    {
    //        statusEffect.round--;
    //        if (statusEffect.round <= 0)
    //        {
    //            statusEffect.RemoveEffect(this);
    //        }
    //    }
    //}
    public override void UpdateStatusEffectRounds()
    {
        base.UpdateStatusEffectRounds();
        for (int i = activeEffects.Count - 1; i >= 0; i--)
        {
            StatusEffect statusEffect = activeEffects[i];
            statusEffect.round--;
            if (statusEffect.round <= 0)
            {
                statusEffect.RemoveEffect(this);
            }
        }
    }
}
