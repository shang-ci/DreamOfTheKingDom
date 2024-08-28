using System.Collections.Generic;
using UnityEngine;

public class Room : MonoBehaviour
{
    public int column;
    public int line;
    private SpriteRenderer spriteRenderer;
    public RoomDataSO roomData;
    public RoomState roomState;
    public List<Vector2Int> linkTo = new List<Vector2Int>();

    [Header("广播")]
    public ObjectEventSO loadRoomEvent;

    private void Awake()
    {
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
    }

    // private void Start()
    // {
    //     SetupRoom(0, 0, roomData);
    // }

    private void OnMouseDown()
    {
        // 处理点击事件
        Debug.Log($"点击了房间：{roomData.roomType}");
        if (roomState == RoomState.Attainable)
        {
            loadRoomEvent.RaiseEvent(this, this);
        }
    }

    /// <summary>
    /// 外部创建房间时调用配置房间
    /// </summary>
    /// <param name="colume"></param>
    /// <param name="line"></param>
    /// <param name="roomData"></param>
    public void SetupRoom(int colume, int line, RoomDataSO roomData)
    {
        this.column = colume;
        this.line = line;
        this.roomData = roomData;

        spriteRenderer.sprite = roomData.roomIcon;

        spriteRenderer.color = roomState switch
        {
            RoomState.Locked => new Color(0.5f, 0.5f, 0.5f, 1f),
            RoomState.Visited => new Color(0.5f, 0.8f, 0.5f, 0.5f),
            RoomState.Attainable => new Color(1f, 1f, 1f, 1f),
            _ => throw new System.NotImplementedException(),
        };
    }
}
