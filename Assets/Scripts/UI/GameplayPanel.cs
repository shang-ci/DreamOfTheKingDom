using System;
using UnityEngine;
using UnityEngine.UIElements;

public class GameplayPanel : MonoBehaviour
{
    [Header("事件广播")]
    public ObjectEventSO playerTurnEndEvent;
    private VisualElement rootElement;
    private Label energyAmountLabel;
    private Label drawAmountLabel;
    private Label discardAmountLabel;
    private Label turnLabel;
    private Button endTurnButton;

    private void OnEnable() 
    {
        rootElement = GetComponent<UIDocument>().rootVisualElement;

        // 在这里添加你的 UI 元素和事件处理程序
        energyAmountLabel = rootElement.Q<Label>("EnergyAmount");
        drawAmountLabel = rootElement.Q<Label>("DrawAmount");
        discardAmountLabel = rootElement.Q<Label>("DiscardAmount");
        turnLabel = rootElement.Q<Label>("TurnLabel");
        endTurnButton = rootElement.Q<Button>("EndTurn");

        endTurnButton.clicked += OnEndTurnButtonClicked;

        drawAmountLabel.text = "0";
        discardAmountLabel.text = "0";
        energyAmountLabel.text = "0";
        turnLabel.text = "游戏开始";
    }

    private void OnEndTurnButtonClicked()
    {
        playerTurnEndEvent.RaiseEvent(null, this);
    }

    public void UpdateDrawDeckAmount(int amount)
    {
        drawAmountLabel.text = amount.ToString();
    }

    public void UpdateDiscardDeckAmount(int amount)
    {
        discardAmountLabel.text = amount.ToString();
    }

    public void UpdateEnergyAmount(int amount)
    {
        energyAmountLabel.text = amount.ToString();
    }

    public void OnEnemyTurnBegin()
    {
        endTurnButton.SetEnabled(false);
        turnLabel.text = "敌方回合";
        turnLabel.style.color = new StyleColor(Color.red);
    }

    public void OnPlayerTurnBegin()
    {
        endTurnButton.SetEnabled(true);
        turnLabel.text = "玩家回合";
        turnLabel.style.color = new StyleColor(Color.white);
    }
}
