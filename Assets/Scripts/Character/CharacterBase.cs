using UnityEngine;

public class CharacterBase : MonoBehaviour
{
    public int maxHp;
    public IntVariable hp;
    public IntVariable defense;
    public IntVariable buffRound;//buffer持续回合数
    public int CurrentHP {get => hp.currentValue; set => hp.SetValue(value);}
    public int MaxHP {get => hp.maxValue; }
    protected Animator animator;
    public bool isDead;

    // 用于显示buff和debuff——两个特效，在这里就简化了，正面buffer，负面debuffer分别使用他俩
    public GameObject buff;
    public GameObject debuff;

    // 力量卡牌有关——增强伤害
    public float baseStrength = 1f;
    public float strengthEffect = 0.5f;

    [Header("广播")]
    public ObjectEventSO characterDeadEvent;//角色死亡事件

    protected virtual void Awake()
    {
        animator = GetComponentInChildren<Animator>();
    }

    protected virtual void Start() {
        hp.maxValue = maxHp;
        CurrentHP = MaxHP;
        buffRound.currentValue = buffRound.maxValue;//确保每次开始新战斗都是从新开始计算
        ResetDefense();
    }

    private void Update() 
    {
        animator.SetBool("isDead", isDead);//玩家和敌人死亡动画
    }

    public virtual void TakeDamage(int damage)
    {
        var currentDamage = (damage - defense.currentValue) >= 0 ? (damage - defense.currentValue) : 0;
        var currentDefense = (damage - defense.currentValue) >= 0 ? 0 : (defense.currentValue - damage);
        defense.SetValue(currentDefense);
        if (CurrentHP > currentDamage)
        {
            // 当前人物生命值减少,就会自动调用hp的SetValue方法从而触发事件
            CurrentHP -= currentDamage;
            Debug.Log($"CurrentHP: {CurrentHP}");
            animator.SetTrigger("hit");//玩家和敌人受击动画
        }
        else
        {
            CurrentHP = 0;
            // 当前人物死亡
            isDead = true;
            characterDeadEvent.RaiseEvent(this, this);//广播死亡事件
        }
    }

    public void UpdateDefense(int amount)
    {
        var value = defense.currentValue + amount;
        defense.SetValue(value);
    }

    public void ResetDefense()
    {
        defense.SetValue(0);
    }

    public void HealHealth(int amount)
    {
        CurrentHP += amount;
        CurrentHP = Mathf.Min(CurrentHP, MaxHP);
        buff.SetActive(true);
    }

    //强化/弱化效果——有需要的卡牌可以包含他
    public void SetupStrength(int round, bool isPositive)
    {
        if (isPositive)//增强，对自己用增强，对敌人用减弱，在Astrea里要改变下
        {
            float newStrength = baseStrength + strengthEffect;
            baseStrength = Mathf.Min(newStrength, 1.5f);
            buff.SetActive(true);
        }
        else
        {
            debuff.SetActive(true);
            baseStrength = 1 - strengthEffect;
        }

        var currentRound = buffRound.currentValue + round;//这里记得在战斗结束时要清零，不然开启新战斗时数值会有问题

        //结束buffer——当敌方给你debuffer你又给自己buffer时，可以直接相互抵消
        if (baseStrength == 1)
        {
            buffRound.SetValue(0);
        }
        else
        {
            buffRound.SetValue(currentRound);
        }
    }

    /// <summary>
    /// 回合转换事件函数
    /// </summary>

    //这里是为了在每个回合结束时调用，更新buffer的持续回合数
    public void UpdateStrengthRound()
    {
        buffRound.SetValue(buffRound.currentValue - 1);
        if (buffRound.currentValue <= 0)
        {
            buffRound.SetValue(0);
            baseStrength = 1;
        }
    }
}
