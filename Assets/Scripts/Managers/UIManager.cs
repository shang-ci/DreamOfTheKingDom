using UnityEngine;

public class UIManager : MonoBehaviour
{
    [Header("面板")]
    public GameObject gameplayPanel;
    public GameObject gameWinPanel;
    public GameObject gameOverPanel;
    public GameObject pickCardPanel;
    public GameObject restRoomPanel;

    //选择激活对应面板——再点击room事件时根据房间类型，激活不同面板
    public void OnLoadRoomEvent(object data)
    {
        Room currentRoom = (Room) data;

        switch (currentRoom.roomData.roomType)
        {
            case RoomType.MinorEnemy:
            case RoomType.EliteEnemy:
            case RoomType.Boss:
                gameplayPanel.SetActive(true);
                break;
            case RoomType.Shop:
                break;
            case RoomType.Treasure:
                break;
            case RoomType.RestRoom:
                restRoomPanel.SetActive(true);
                break;
        }
    }

    //隐藏所有面板——返回地图时隐藏所有面板，loadmap事件、加载menu时调用
    public void HideAllPanels()
    {
        gameplayPanel.SetActive(false);
        gameWinPanel.SetActive(false);
        gameOverPanel.SetActive(false);
        restRoomPanel.SetActive(false);
    }

    //游戏胜利事件——游戏胜利时激活胜利面板
    public void OnGameWinEvent()
    {
        gameplayPanel.SetActive(false);
        gameWinPanel.SetActive(true);
    }

    //游戏失败事件——游戏失败时激活失败面板
    public void OnGameOverEvent()
    {
        Debug.Log("OnGameOverEvent");
        gameplayPanel.SetActive(false);
        gameOverPanel.SetActive(true);
    }

    //激活选择卡牌面板——winUI中点击选择卡牌时触发
    public void OnPickCardEvent()
    {
        pickCardPanel.SetActive(true);
    }

    //隐藏选择卡牌面板——选择卡牌完成后触发事件
    public void OnFinishPickCardEvent()
    {
        pickCardPanel.SetActive(false);
    }
}
