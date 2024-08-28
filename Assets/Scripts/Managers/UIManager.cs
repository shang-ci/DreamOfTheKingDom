using UnityEngine;

public class UIManager : MonoBehaviour
{
    [Header("面板")]
    public GameObject gameplayPanel;
    public GameObject gameWinPanel;
    public GameObject gameOverPanel;
    public GameObject pickCardPanel;
    public GameObject restRoomPanel;

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

    /// <summary>
    /// loadmap Event / load menu
    /// </summary>
    public void HideAllPanels()
    {
        gameplayPanel.SetActive(false);
        gameWinPanel.SetActive(false);
        gameOverPanel.SetActive(false);
        restRoomPanel.SetActive(false);
    }

    public void OnGameWinEvent()
    {
        gameplayPanel.SetActive(false);
        gameWinPanel.SetActive(true);
    }

    public void OnGameOverEvent()
    {
        Debug.Log("OnGameOverEvent");
        gameplayPanel.SetActive(false);
        gameOverPanel.SetActive(true);
    }

    public void OnPickCardEvent()
    {
        pickCardPanel.SetActive(true);
    }

    public void OnFinishPickCardEvent()
    {
        pickCardPanel.SetActive(false);
    }
}
