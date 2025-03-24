using UnityEngine;

//数据源头——卡牌、遗物
[CreateAssetMenu(fileName = "Equipment", menuName = "Equipment/Item")]
public class Item : ScriptableObject
{
    public int id;
    public string itemName;
    [TextArea(10,15)]
    public string des;
    public Sprite icon;
    public ItemType itemType;//用来分辨数据类型——商店里需要

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
