using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [Header("地图布局")]
    public MapLayoutSO mapLayout;//保存的地图布局数据——房间信息

    public List<Enemy> aliveEnemyList = new List<Enemy>();//存活的敌人列表——表示可以有多个敌人——在点击进入时先清空再读取填充

    [Header("事件通知")]//事件通知，当游戏胜利或失败时调用
    public ObjectEventSO gameWinEvent;
    public ObjectEventSO gameOverEvent;

    //加载地图时调用，清空存活的敌人列表，也就是——点击back时调用||newgame时调用
    // 加载地图，把当前房间设置为已访问状态，把相邻房间设置为锁定状态，把当前房间后面的房间设置为可访问状态
    //根据点击的房间的位置，更新地图布局数据以及房间状态
    public void UpdateMapLayoutData(object value)
    {
        Vector2Int roomVector = (Vector2Int)value;

        // 如果地图布局数据为空，直接返回——防止在newgame时地图为空报错
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

        aliveEnemyList.Clear();//清空存活的敌人列表
    }

    //发现场景中所有敌人包括未激活的敌人——当房间加载时调用
    public void OnRoomLoadedEvent(object obj)
    {
        var enemies = FindObjectsByType<Enemy>(FindObjectsInactive.Include, FindObjectsSortMode.None);
        foreach (var enemy in enemies)
        {
            aliveEnemyList.Add(enemy);
        }
    }

    //角色死亡，当有角色死亡时调用，判断是玩家死亡还是敌人死亡，发出对应的通知更改游戏状态
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
            StartCoroutine(EventDelayAction(gameOverEvent));//这里可以做一个通关保存数据的操作
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

    //希望在游戏结束时，延迟一段时间再发出通知
    public IEnumerator EventDelayAction(ObjectEventSO eventSO)
    {
        yield return new WaitForSeconds(1.5f);
        eventSO.RaiseEvent(null, this);
    }

    //清空地图布局数据——当在menu点击新游戏时调用
    public void OnNewGameEvent()
    {
        mapLayout.mapRoomDataList.Clear();
        mapLayout.linePositionList.Clear();
    }
}
