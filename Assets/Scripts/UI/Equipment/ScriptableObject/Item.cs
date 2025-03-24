using UnityEngine;

//����Դͷ�������ơ�����
[CreateAssetMenu(fileName = "Equipment", menuName = "Equipment/Item")]
public class Item : ScriptableObject
{
    public int id;
    public string itemName;
    [TextArea(10,15)]
    public string des;
    public Sprite icon;
    public ItemType itemType;//�����ֱ��������͡����̵�����Ҫ

    public virtual ItemType ItemType
    {
        get { return itemType; }
        set { itemType = value; }
    }
}

public enum ItemType
{
    Equipment,
    Card,
    Character
}
