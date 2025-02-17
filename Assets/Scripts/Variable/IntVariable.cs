using UnityEngine;

[CreateAssetMenu(fileName = "IntVariable", menuName = "Variable/IntVariable")]
public class IntVariable : ScriptableObject
{
    public int maxValue;
    public int currentValue;
    public IntEventSO ValueChangeEvent;

    [TextArea]
    [SerializeField]
    private string description;

    public void SetValue(int value) 
    {
        currentValue = value;
        ValueChangeEvent?.RaiseEvent(value, this);
    }
}
