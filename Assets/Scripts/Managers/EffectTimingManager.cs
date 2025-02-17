using UnityEngine;
using UnityEngine.Events;

public class EffectTimingManager : MonoBehaviour
{
    public static EffectTimingManager Instance { get; private set; }

    public UnityEvent<EffectTiming> OnEffectTimingChanged;//广播状态改变事件

    private EffectTiming currentTiming;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        if (OnEffectTimingChanged == null)
        {
            OnEffectTimingChanged = new UnityEvent<EffectTiming>();
        }
    }

    public void ChangeEffectTiming(EffectTiming timing)
    {
        currentTiming = timing;
        OnEffectTimingChanged.Invoke(timing);
    }

    public EffectTiming GetCurrentTiming()
    {
        return currentTiming;
    }
}