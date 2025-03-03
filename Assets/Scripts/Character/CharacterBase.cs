using System.Collections.Generic;
using UnityEngine;

public class CharacterBase : MonoBehaviour
{
    public int maxHp;
    public IntVariable hp;
    public IntVariable defense;
    public IntVariable buffRound;//buffer持续回合数

    //状态条引用
    public StatusBar statusBar;

    public int CurrentHP {get => hp.currentValue; set => hp.SetValue(value);}
    public int MaxHP {get => hp.maxValue; }
    protected Animator animator;
    public bool isDead;
    public int currentDamage;

    //用于显示buff和debuff——两个特效，在这里就简化了，正面buffer，负面debuffer分别使用他俩
    public GameObject buff;
    public GameObject debuff;

    [Header("状态效果")]
    // 使用字典存储状态及其点数——存储的都是副本，不会影响原始数据
    public Dictionary<string, int> statusEffects = new Dictionary<string, int>();
    public List<StatusEffect> activeEffects = new List<StatusEffect>();

    //强化卡牌有关——增强伤害
    public float baseStrength = 1f;
    public float strengthEffect = 0.5f;

    [SerializeField]public int thornRoundCount = 0; // 荆棘效果的回合计数
    [SerializeField]private bool wasAttackedInThornRound = false; // 荆棘效果期间是否受到攻击

    [Header("广播")]
    public ObjectEventSO characterDeadEvent;//角色死亡事件

    public List<Enemy> aliveEnemyList = new List<Enemy>();//存活的敌人列表


    protected virtual void Awake()
    {
        animator = GetComponentInChildren<Animator>();
    }

    protected virtual void Start() {
        hp.maxValue = maxHp;
        CurrentHP = MaxHP;
        buffRound.currentValue = buffRound.maxValue;//确保每次开始新战斗都是从新开始计算
        ResetDefense();

        // 监听状态效果改变事件
        EffectTimingManager.Instance.OnEffectTimingChanged.AddListener(OnEffectTimingChanged);
    }

    private void OnDestroy()
    {
        if (EffectTimingManager.Instance != null)
        {
            EffectTimingManager.Instance.OnEffectTimingChanged.RemoveListener(OnEffectTimingChanged);
        }
    }


    //监听当前状态改变这一事件，每次改变都会判断执行当前身上能执行的效果
    private void OnEffectTimingChanged(EffectTiming timing)
    {
        ExecuteStatusEffects(timing);
    }

    private void Update() 
    {
        animator.SetBool("isDead", isDead);//玩家和敌人死亡动画
    }

    public virtual void TakeDamage(int damage)
    {
        currentDamage = (damage - defense.currentValue) >= 0 ? (damage - defense.currentValue) : 0;
        var currentDefense = (damage - defense.currentValue) >= 0 ? 0 : (defense.currentValue - damage);
        defense.SetValue(currentDefense);
        if (CurrentHP > currentDamage)
        {
            // 处理护盾效果
            if (statusEffects.ContainsKey("ShieldStatusEffect") && this is Enemy)
            {
                EffectTimingManager.Instance.ChangeEffectTiming(EffectTiming.EnemyShield);
            }
            else if (statusEffects.ContainsKey("ShieldStatusEffect") && this is Player)
            {
                EffectTimingManager.Instance.ChangeEffectTiming(EffectTiming.PlayerShield);
            }

                // 处理厚皮效果
            if (statusEffects.ContainsKey("ThickSkinStatusEffect") && this is Enemy)
            {
                //currentDamage -= statusEffects["ThickSkinStatusEffect"];
                EffectTimingManager.Instance.ChangeEffectTiming(EffectTiming.EnemyThickSkin);
            }
            else if (statusEffects.ContainsKey("ThickSkinStatusEffect") && this is Player)
            {
                EffectTimingManager.Instance.ChangeEffectTiming(EffectTiming.PlayerThickSkin);
            }

            // 当前人物生命值减少,就会自动调用hp的SetValue方法从而触发事件
            CurrentHP -= currentDamage;
            Debug.Log($"CurrentHP: {CurrentHP}");
            animator.SetTrigger("hit");//玩家和敌人受击动画

            // 处理荆棘效果
            if (statusEffects.ContainsKey("ThornStatusEffect") && this is Enemy)
            {
                EffectTimingManager.Instance.ChangeEffectTiming(EffectTiming.EnemyThorn);
                wasAttackedInThornRound = true; // 标记在荆棘效果期间受到攻击
            }
            else if(statusEffects.ContainsKey("ThornStatusEffect") && this is Player)
            {
                EffectTimingManager.Instance.ChangeEffectTiming(EffectTiming.PlayerThorn);
                wasAttackedInThornRound = true;
                Debug.Log("玩家荆棘");
            }
        }
        else
        {
            CurrentHP = 0;
            // 当前人物死亡
            isDead = true;
            characterDeadEvent.RaiseEvent(this, this);//广播死亡事件
        }
    }

    //直接造成伤害，不考虑身上的状态效果
    public virtual void TakeDamage2(int damage)
    {
        if(CurrentHP > damage)
        {
            CurrentHP -= damage;
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



    // 添加状态效果
    public void AddStatusEffect(StatusEffect effect)
    {
        if (statusEffects.ContainsKey(effect.effectName))
        {
            statusEffects[effect.effectName] += effect.value;

            effect.round += 3;//TOOD：偷懒了没有拿到副本对应的原始数据来加回合数

            //提前结算荆棘效果并重置回合数——在有荆棘效果时，再次受到荆棘效果时，会提前结算上一次的荆棘效果
            if (effect is ThornStatusEffect)
            {
                if (wasAttackedInThornRound)
                {
                    HealHealth(statusEffects["ThornStatusEffect"] * (3 - thornRoundCount)); // 受到攻击时加血
                }
                else
                {
                    TakeDamage(statusEffects["ThornStatusEffect"] * (3 - thornRoundCount)); // 没有受到攻击时扣血
                }
                thornRoundCount = 3;
                wasAttackedInThornRound = false;
            }
        }
        else
        {
            statusEffects[effect.effectName] = effect.value;
            activeEffects.Add(effect);//只存储当前角色身上的状态效果

            // 如果是荆棘效果，初始化回合数
            if (effect is ThornStatusEffect)
            {
                thornRoundCount = 3;
                wasAttackedInThornRound = false;
            }
        }

        //改变时机——触发状态效果
        effect.ChangeTime(this);
    }

    // 移除状态效果
    public void RemoveStatusEffect(string effectName)
    {
        if (statusEffects.ContainsKey(effectName))
        {
            statusEffects.Remove(effectName);
            activeEffects.RemoveAll(e => e.effectName == effectName);
        }
    }

    // 清除所有状态效果——在点击房间加载玩家时调用
    public void ClearAllStatusEffects()
    {
        foreach (var effect in new List<StatusEffect>(activeEffects))
        {
            effect.RemoveEffect(this);
        }
        //thornRoundCount = 0;//清除荆棘效果的回合数,防止报空
    }

    // 执行状态效果
    public virtual void ExecuteStatusEffects(EffectTiming timing) { }


    // 更新状态条——不需要了，在血条控制部分已经有了
    private void UpdateStatusBar()
    {
        statusBar.UpdateStatusBar(statusEffects);
    }

    // 获取状态效果
    public Dictionary<string, int> GetStatusEffects()
    {
        return statusEffects;
    }

    // 获取状态效果的点数
    public int GetStatusEffectValue(string effectName)
    {
        if (statusEffects.ContainsKey(effectName))
        {
            return statusEffects[effectName];
        }
        return 0;
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

    //这里是为了在每个回合结束时调用，更新buffer的持续回合数——每当playerturnbegin事件就执行，也就是轮到玩家就执行
    public void UpdateStrengthRound()
    {
        buffRound.SetValue(buffRound.currentValue - 1);
        if (buffRound.currentValue <= 0)
        {
            buffRound.SetValue(0);
            baseStrength = 1;
        }

        //更新荆棘效果的回合数
        UpdateThornRound();
    }

    // 更新荆棘效果的回合数
    public void UpdateThornRound()
    {
        if (thornRoundCount > 0)
        {
            thornRoundCount--;
            if (thornRoundCount == 0 && statusEffects.ContainsKey("ThornStatusEffect"))
            {
                if (wasAttackedInThornRound)
                {
                    HealHealth(statusEffects["ThornStatusEffect"] * 3); // 受到攻击时加血
                }
                else
                {
                    TakeDamage(statusEffects["ThornStatusEffect"] * 3); // 没有受到攻击时扣血
                }
            }
        }
    }

    // 更新状态效果的回合数
    public virtual void UpdateStatusEffectRounds() { }

}
