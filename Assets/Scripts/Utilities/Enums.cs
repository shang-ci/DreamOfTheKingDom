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
    Any,//����Ŀ��
    SingleEnemy,//��������
    AllEnemies,//���е���
    All,//���н�ɫ
    Self//�Լ�
}

public enum EffectType
{
    Purification,//����
    Corruption,//����
    Strengthen,//ǿ��
    Reset,//����
    Transform,//ת��
    Extract,//��ȡ
    Shield//����
}


public enum EffectTargetType
{
    Self,
    Target,
    ALL,
}

//����ʱ��
public enum EffectTiming
{
    Reseach,//�о����ƴ�������о���һʱ��
    Strengthen,//ǿ�����ƴ������ǿ����һʱ��
    OnTurnStart,
    OnTurnEnd,
    OnDamageTaken,
    OnDamageDealt,
    Thorn,//����
    None
    // �������ִ��ʱ��
}