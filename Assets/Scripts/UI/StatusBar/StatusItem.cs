using TMPro;
using UnityEngine;

public class StatusItem : MonoBehaviour
{
    public TextMeshProUGUI effectNameText;
    public TextMeshProUGUI valueText;

    public void Initialize(string effectName, int value)
    {
        effectNameText.text = effectName;
        valueText.text = value.ToString();
    }
}