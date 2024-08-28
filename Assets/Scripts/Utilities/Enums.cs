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

public enum EffectTargetType
{
    Self,
    Target,
    ALL,
}