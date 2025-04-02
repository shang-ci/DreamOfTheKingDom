using UnityEngine;
using UnityEngine.UI;

//管理几个ui
public class UI : MonoBehaviour
{
    public GameObject Shop;
    public GameObject Status_UI;
    public GameObject Equipment;

    public Image Image;

    public void SetActiveEquipment()
    {
        Equipment.SetActive(true);
    }

    public void SetActiveStatus_UI()
    {
        Status_UI.SetActive(true);
    }

    public void SetShop()
    {
        Shop.SetActive(true);
    }

}
