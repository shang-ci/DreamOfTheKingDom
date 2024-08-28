using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [Header("地图布局")]
    public MapLayoutSO mapLayout;

    public List<Enemy> aliveEnemyList = new List<Enemy>();

    [Header("事件通知")]
    public ObjectEventSO gameWinEvent;
    public ObjectEventSO gameOverEvent;

    /// <summary>
    /// 更新房间的事件监听函数，加载地图
    /// </summary>
    /// <param name="roomVector"></param>
    public void UpdateMapLayoutData(object value)
    {
        Vector2Int roomVector = (Vector2Int)value;
        if (mapLayout.mapRoomDataList.Count == 0)
        {
            return;
        }

        // 设置当前房间已访问
        var currentRoom = mapLayout.mapRoomDataList.Find(r => r.column == roomVector.x && r.line == roomVector.y);
        currentRoom.roomState = RoomState.Visited;

        // 设定相邻房间为锁定状态
        var sameColumnRooms = mapLayout.mapRoomDataList.FindAll(r => r.column == currentRoom.column);
        foreach (var room in sameColumnRooms)
        {
            if (room.line == roomVector.y)
            {
                // 需要排除掉当前房间
                continue;
            }
            room.roomState = RoomState.Locked;
        }

        // 设置当前房间后面的房间为可访问状态
        foreach (var link in currentRoom.linkTo)
        {
            var linkedRoom = mapLayout.mapRoomDataList.Find(r => r.column == link.x && r.line == link.y);
            linkedRoom.roomState = RoomState.Attainable;
        }

        // 房间信息发生了改变，需要写入文件
        DataManager.instance.SaveToFile(mapLayout);

        aliveEnemyList.Clear();
    }

    public void OnRoomLoadedEvent(object obj)
    {
        var enemies = FindObjectsByType<Enemy>(FindObjectsInactive.Include, FindObjectsSortMode.None);
        foreach (var enemy in enemies)
        {
            aliveEnemyList.Add(enemy);
        }
    }

    public void OnCharacterDeadEvent(object character)
    {
        if (character is Player)
        {
            // 发出失败的通知
            Debug.Log("OnCharacterDeadEvent player");
            StartCoroutine(EventDelayAction(gameOverEvent));
        }

        if (character is Boss)
        {
            StartCoroutine(EventDelayAction(gameOverEvent));
        }
        else if (character is Enemy)
        {
            aliveEnemyList.Remove(character as Enemy);

            if (aliveEnemyList.Count == 0)
            {
                // 发出获胜通知
                StartCoroutine(EventDelayAction(gameWinEvent));
            }
        }
    }

    public IEnumerator EventDelayAction(ObjectEventSO eventSO)
    {
        yield return new WaitForSeconds(1.5f);
        eventSO.RaiseEvent(null, this);
    }

    public void OnNewGameEvent()
    {
        mapLayout.mapRoomDataList.Clear();
        mapLayout.linePositionList.Clear();
    }
}
