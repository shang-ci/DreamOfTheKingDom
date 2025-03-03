using System.Collections.Generic;
using UnityEngine;

// 敌人行为数据，每个敌人都有自己的行为数据
[CreateAssetMenu(fileName = "EnemyActionDataSO", menuName = "Enemy/EnemyActionDataSO")]
public class EnemyActionDataSO : ScriptableObject 
{
    public List<EnemyAction> actions;
}

//意图的图片加上效果
[System.Serializable]
public struct EnemyAction
{
    public Sprite intentSprite;
    public Effect effect;
    public float probability; //触发率
    public float accuracy; //精准率
    public float criticalRate; //暴击率
    public int originalValue; //原始值
}