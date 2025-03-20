using UnityEngine;
using UnityEngine.UI;

public class Equipment_Item : MonoBehaviour
{
    public Image icon;
    public Item item;
    private int id;


    public void SetEquipmentItem(Item item)
    {
        this.item = item;
        this.id = item.id;
        icon.sprite = item.icon;
    }
}
