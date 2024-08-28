using UnityEngine;
using UnityEngine.Events;

public class BaseEventSO<T> : ScriptableObject
{
    [TextArea]
    public string description;

    public UnityAction<T> OnEventRaised;
    public string lastSender;

    public void RaiseEvent(T value, object sender)
    {
        OnEventRaised?.Invoke(value);
        lastSender = sender.ToString();
    }
}
