using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class Player : CharacterBase
{
    public IntVariable playerMana;
    public int maxMana;
    public int CurrentMana {get => playerMana.currentValue;set => playerMana.SetValue(value); }//这个current类的数值就是为了方便事件广播，更新UI状态，和currenthap类似
    
    [Header("成长系统")]
    public GrowthSystem growthSystem;

    protected override void Awake()
    {
        growthSystem = new GrowthSystem();
    }

    protected override void Start()
    {
        base.Start();
    }

    private void OnEnable() 
    {
        playerMana.maxValue = maxMana;
        CurrentMana = playerMana.maxValue; 
    }

    /// <summary>
    /// 监听事件函数
    /// </summary>

    //新回合开始每次都要执行——重置当前回合的法力值
    public void newTurn()
    {
        CurrentMana = maxMana;
    }

    //触发事件，让UI管理器去监听更新UI
    public void UpdateMana(int cost)
    {
        CurrentMana -= cost;
        if (CurrentMana <= 0)
        {
            CurrentMana = 0;
        }
    }

    //初始化玩家数据——当在menu界面点击新游戏按钮时调用
    public void NewGame()
    {
        CurrentHP = MaxHP;
        isDead = false;
        buffRound.currentValue = buffRound.maxValue;//防止上一局的buffer影响到这一局
        newTurn();//重置法力值
    }

    //执行玩家的状态效果
    public override void ExecuteStatusEffects(EffectTiming timing)
    {
        base.ExecuteStatusEffects(timing); 

        foreach (var effect in activeEffects)
        {
            if (effect.timing == timing)
            {
                effect.ExecuteEffect(this, (CharacterBase)GameManager.Instance.GetSingleOrMultipleEnemies());
            }
        }
    }


    //集合遍历时被修改会触发InvalidOperationException异常，所以这里使用for循环/先记录要删除的元素后面再foreach删除
    //玩家状态效果回合数更新——这里就不用事件来调用了，要是敌人数量很多那岂不是要给每个敌人都添加一个事件监听太费事了
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

