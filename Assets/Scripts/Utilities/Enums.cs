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

//装备的类型
public enum EquipmentTargetType
{
    Self,//自己
    Target,//单个敌人
    ALL,//所有角色
    Our,//己方所有人
    Enemies,//所有敌人
    Random,//随机一个目标
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

//effect的类型
public enum EffectTargetType
{
    Self,
    Target,
    ALL,
    Our,
}

public enum StatusEffectTargetType
{
    Self,//自己
    Target,//单个敌人
    ALL,//所有敌人
    Our,//我们
}



//触发时机
public enum EffectTiming
{
    #region 装备的时机

    NewGame,//进入新战斗

    #endregion

    #region 状态卡牌的时机

    Reseach,//研究卡牌打出触发研究这一时机
    Strengthen,//强化卡牌打出触发强化这一时机
    OnPlayerTurn,//玩家回合
    OnEnemyTurn,//敌人回合
    OnDamagePlayer,//玩家受伤
    OnDamageEnemy,//敌人受伤
    PlayerThorn,//玩家荆棘
    EnemyThorn,//敌人荆棘
    Thorn,//荆棘
    ThickSkin,//厚皮
    PlayerThickSkin,//玩家厚皮
    EnemyThickSkin,//敌人厚皮
    Shield,//护盾
    EnemyShield,//敌人护盾
    PlayerShield,//玩家护盾
    CrimsonMark,//猩红印记
    PlayerCrimsonMark,//玩家猩红印记
    EnemyCrimsonMark,//敌人猩红印记

    #endregion
    None
    // 添加其他执行时机
}

//属性
public enum Attribute
{
    gold,
    wood,
    water,
    fire,
    earth
}


