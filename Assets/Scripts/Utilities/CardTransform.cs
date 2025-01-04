using UnityEngine;

//因为卡牌布局包括位置和旋转，所以我们需要一个结构来存储这两个值，方便传递使用
public struct CardTransform
{
    public Vector3 pos;
    public Quaternion rotation;

    public CardTransform(Vector3 position, Quaternion rotation)
    {
        this.pos = position;
        this.rotation = rotation;
    }
}
