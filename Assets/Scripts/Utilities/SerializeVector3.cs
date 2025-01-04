using UnityEngine;

//将Vector3序列化为可存储的数据
public class SerializeVector3
{
    public float x, y, z;

    public SerializeVector3()
    { }

    public SerializeVector3(float x, float y, float z)
    {
        this.x = x;
        this.y = y;
        this.z = z;
    }

    public SerializeVector3(Vector3 pos)
    {
        this.x = pos.x;
        this.y = pos.y;
        this.z = pos.z;
    }

    public Vector3 ToVector3()
    {
        return new Vector3(x, y, z);
    }
}
