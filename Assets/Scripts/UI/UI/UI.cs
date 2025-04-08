using UnityEngine;
using UnityEngine.UI;

//管理几个ui
public class UI : MonoBehaviour
{
    public GameObject Shop;
    public GameObject Status_UI;
    public GameObject Equipment;
    public GameObject Setting;

    public Image Image;


    #region Set Active
    public void SetActiveSetting()
    {
        Setting.SetActive(true);
    }

    public void SetActiveEquipment()
    {
        Equipment.SetActive(true);
    }

    public void SetActiveStatus_UI()
    {
        Status_UI.SetActive(true);
    }

    public void SetActiveShop()
    {
        Shop.SetActive(true);
    }

    public void DisActiveShop()
    {
        Shop.SetActive(false);
    }

    public void DisActiveSetting()
    {
        Setting.SetActive(false);
    }

    public void DisActiveEquipment()
    {
        Equipment.SetActive(false);
    }

    public void DisActiveStatus_UI()
    {
        Status_UI.SetActive(false);
    }
    #endregion
}
