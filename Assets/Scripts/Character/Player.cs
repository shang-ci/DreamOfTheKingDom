using UnityEngine;

public class Player : CharacterBase
{
    public IntVariable playerMana;
    public int maxMana;

    //这个current类的数值就是为了方便事件广播，更新UI状态，和currenthap类似
    public int CurrentMana {get => playerMana.currentValue;set => playerMana.SetValue(value);}

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
}
