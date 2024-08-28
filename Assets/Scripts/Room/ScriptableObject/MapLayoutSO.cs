using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "MapLayoutSO", menuName = "Map/MapLayoutSO")]
public class MapLayoutSO : ScriptableObject 
{
    public List<MapRoomData> mapRoomDataList = new List<MapRoomData>();
    public List<LinePosition> linePositionList = new List<LinePosition>();
}

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

[System.Serializable]
public class LinePosition
{
    public SerializeVector3 startPos;
    public SerializeVector3 endPos;
}