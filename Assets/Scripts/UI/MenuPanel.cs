using System;
using UnityEngine;
using UnityEngine.UIElements;

public class MenuPanel : MonoBehaviour
{
    private VisualElement rootElement;
    private Button newGameButton;
    private Button quitButton;

    [Header("newgame广播")]
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

    //点击新游戏按钮时，广播newgame事件——加载地图
    private void OnNewGameButtonClicked()
    {
        newGameEvent.RaiseEvent(null, this);
    }
}
