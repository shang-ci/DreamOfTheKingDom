using System.Collections.Generic;
using UnityEngine;

//���������ͼ����
[CreateAssetMenu(fileName = "MapLayoutSO", menuName = "Map/MapLayoutSO")]
public class MapLayoutSO : ScriptableObject 
{
    public List<MapRoomData> mapRoomDataList = new List<MapRoomData>();
    public List<LinePosition> linePositionList = new List<LinePosition>();
}

//�������ݣ����淿���λ�ã����ͣ�״̬���Լ���֮�����ķ���
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

//��Vector3���л����������ߵ���ʼ����յ�
[System.Serializable]
public class LinePosition
{
    public SerializeVector3 startPos;
    public SerializeVector3 endPos;
}