using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class GameOverPanel : MonoBehaviour
{
    private UnityEngine.UIElements.Button backToStartButton;
    [Header("广播返回Menu")]
    public ObjectEventSO loadMenuEvent;

    private void OnEnable() 
    {
        GetComponent<UIDocument>().rootVisualElement.Q<UnityEngine.UIElements.Button>("BackToStartButton").clicked += BackToStart;
    }

    //返回menu界面
    private void BackToStart()
    {
        loadMenuEvent.RaiseEvent(null, this);
    }
}
