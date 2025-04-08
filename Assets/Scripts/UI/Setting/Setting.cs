using UnityEngine;

public class Setting : MonoBehaviour
{
    public void ActiveSetting()
    {
        gameObject.SetActive(true);
    }

    public void DeactiveSetting()
    {
        gameObject.SetActive(false);
    }
}
