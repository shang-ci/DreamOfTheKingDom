using UnityEngine;

[CreateAssetMenu(fileName = "Equipment", menuName = "Equipment/Item")]
public class Item : ScriptableObject
{
    public int id;
    public string itemName;
    public string des;
    public Sprite icon;
    public ItemType itemType;
    public Effect effect;
}

public enum ItemType
{
    Equipment,
    Card,
    Character
}
