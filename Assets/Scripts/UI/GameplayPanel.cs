using System;
using UnityEngine;
using UnityEngine.UIElements;

//战斗的 UI 界面管理——状态管理
public class GameplayPanel : MonoBehaviour
{
    [Header("事件广播")]
    public ObjectEventSO playerTurnEndEvent;//玩家回合结束事件——绑定在回合转换按钮上

    //UI 元素
    private VisualElement rootElement;
    private Label energyAmountLabel;
    private Label drawAmountLabel;
    private Label discardAmountLabel;
    private Label turnLabel;
    private Button endTurnButton;

    private void OnEnable() 
    {
        rootElement = GetComponent<UIDocument>().rootVisualElement;

        // 获取 UI 元素
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

    //回合结束按钮点击事件
    private void OnEndTurnButtonClicked()
    {
        playerTurnEndEvent.RaiseEvent(null, this);
    }

    // 更新抽牌堆数量
    public void UpdateDrawDeckAmount(int amount)
    {
        drawAmountLabel.text = amount.ToString();
    }

    // 更新弃牌堆数量
    public void UpdateDiscardDeckAmount(int amount)
    {
        discardAmountLabel.text = amount.ToString();
    }

    // 更新能量数量
    public void UpdateEnergyAmount(int amount)
    {
        energyAmountLabel.text = amount.ToString();
    }

    // 敌方回合开始——禁用回合转换按钮
    public void OnEnemyTurnBegin()
    {
        endTurnButton.SetEnabled(false);
        turnLabel.text = "敌方回合";
        turnLabel.style.color = new StyleColor(Color.red);
    }

    // 玩家回合开始——启用回合转换按钮
    public void OnPlayerTurnBegin()
    {
        endTurnButton.SetEnabled(true);
        turnLabel.text = "玩家回合";
        turnLabel.style.color = new StyleColor(Color.white);
    }
}
