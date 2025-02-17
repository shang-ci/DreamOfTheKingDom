using System;

[Flags]
public enum RoomType
{
    MinorEnemy = 1 << 0,
    EliteEnemy = 1 << 1,
    Shop = 1 << 2,
    Treasure = 1 << 3,
    RestRoom = 1 << 4,
    Boss = 1 << 5
}

public enum RoomState
{
    Locked,
    Visited,
    Attainable
}

public enum CardType
{
    Attack,
    Defense,
    Abilities
}

public enum CardTargetType
{
    Any,//任意目标
    SingleEnemy,//单个敌人
    AllEnemies,//所有敌人
    All,//所有角色
    Self//自己
}

public enum EffectType
{
    Purification,//净化
    Corruption,//腐化
    Strengthen,//强化
    Reset,//重置
    Transform,//转化
    Extract,//抽取
    Shield//护盾
}


public enum EffectTargetType
{
    Self,
    Target,
    ALL,
}

//触发时机
public enum EffectTiming
{
    Reseach,//研究卡牌打出触发研究这一时机
    Strengthen,//强化卡牌打出触发强化这一时机
    OnTurnStart,
    OnTurnEnd,
    OnDamageTaken,
    OnDamageDealt,
    Thorn,//荆棘
    None
    // 添加其他执行时机
}