using System;
using UnityEngine;
using UnityEngine.UIElements;

public class MenuPanel : MonoBehaviour
{
    private VisualElement rootElement;
    private Button newGameButton;
    private Button quitButton;

    public ObjectEventSO newGameEvent;

    private void OnEnable() 
    {
        rootElement = GetComponent<UIDocument>().rootVisualElement;
        newGameButton = rootElement.Q<Button>("NewGameButton");    
        quitButton = rootElement.Q<Button>("QuitGameButton");    

        newGameButton.clicked += OnNewGameButtonClicked;
        quitButton.clicked += OnQuitGameButtonClicked;
    }

    private void OnQuitGameButtonClicked()
    {
        Application.Quit();
    }

    private void OnNewGameButtonClicked()
    {
        newGameEvent.RaiseEvent(null, this);
    }
}
