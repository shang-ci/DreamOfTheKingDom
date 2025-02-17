using System.Collections.Generic;
using UnityEngine;

//用来保存地图数据
[CreateAssetMenu(fileName = "MapLayoutSO", menuName = "Map/MapLayoutSO")]
public class MapLayoutSO : ScriptableObject 
{
    public List<MapRoomData> mapRoomDataList = new List<MapRoomData>();
    public List<LinePosition> linePositionList = new List<LinePosition>();
}

//房间数据，保存房间的位置，类型，状态，以及与之相连的房间
[System.Serializable]
public class MapRoomData 
{
    public float posX;
    public float posY;
    public int column;
    public int line;
    public RoomType roomType;
    public RoomState roomState;
    public List<Vector2Int> linkTo;
}

//将Vector3序列化，保存连线的起始点和终点
[System.Serializable]
public class LinePosition
{
    public SerializeVector3 startPos;
    public SerializeVector3 endPos;
}