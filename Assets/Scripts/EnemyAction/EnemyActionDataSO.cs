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
}