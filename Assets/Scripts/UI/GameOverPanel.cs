using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class GameOverPanel : MonoBehaviour
{
    private UnityEngine.UIElements.Button backToStartButton;
    public ObjectEventSO loadMenuEvent;

    private void OnEnable() 
    {
        GetComponent<UIDocument>().rootVisualElement.Q<UnityEngine.UIElements.Button>("BackToStartButton").clicked += BackToStart;
    }

    private void BackToStart()
    {
        loadMenuEvent.RaiseEvent(null, this);
    }
}
