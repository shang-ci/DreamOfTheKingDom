using System;
using UnityEngine;
using UnityEngine.UIElements;

public class GameWinPanel : MonoBehaviour
{
    private VisualElement rootElement;
    private Button pickCardButton;
    private Button backToMapButton;

    [Header("事件广播")]
    public ObjectEventSO loadMapEvent;
    public ObjectEventSO pickCardEvent;

    private void OnEnable() 
    {
        rootElement = GetComponent<UIDocument>().rootVisualElement;
        pickCardButton = rootElement.Q<Button>("PickCardButton");
        backToMapButton = rootElement.Q<Button>("BackToMapButton");   
        backToMapButton.clicked += OnBackToMapButtonClicked;
        pickCardButton.clicked += OnPickCardButtonClicked;
    }

    private void OnDisable()
    {
        backToMapButton.clicked -= OnBackToMapButtonClicked;
        pickCardButton.clicked -= OnPickCardButtonClicked;
    }

    //点击抽卡按钮时，抽卡广播事件显示抽卡面板
    private void OnPickCardButtonClicked()
    {
        pickCardEvent.RaiseEvent(null, this);
    }

    //点击back按钮时，返回地图——
    private void OnBackToMapButtonClicked()
    {
        loadMapEvent.RaiseEvent(null, this);
    }

    //隐藏抽卡按钮——当抽卡完成时触发事件
    public void OnFinishPickCardEvent()
    {
        pickCardButton.style.display = DisplayStyle.None;
    }

}
