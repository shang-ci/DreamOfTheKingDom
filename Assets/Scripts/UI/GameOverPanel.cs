using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class GameOverPanel : MonoBehaviour
{
    private UnityEngine.UIElements.Button backToStartButton;
    [Header("�㲥����Menu")]
    public ObjectEventSO loadMenuEvent;

    private void OnEnable() 
    {
        GetComponent<UIDocument>().rootVisualElement.Q<UnityEngine.UIElements.Button>("BackToStartButton").clicked += BackToStart;
    }

    //����menu����
    private void BackToStart()
    {
        loadMenuEvent.RaiseEvent(null, this);
    }
}
