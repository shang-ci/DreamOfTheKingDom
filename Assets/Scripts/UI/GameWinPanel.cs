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

    private void OnPickCardButtonClicked()
    {
        pickCardEvent.RaiseEvent(null, this);
    }

    private void OnBackToMapButtonClicked()
    {
        loadMapEvent.RaiseEvent(null, this);
    }

    public void OnFinishPickCardEvent()
    {
        pickCardButton.style.display = DisplayStyle.None;
    }

}
