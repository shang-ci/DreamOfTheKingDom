using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "MapConfigSO", menuName = "Map/MapConfigSO")]
public class MapConfigSO : ScriptableObject
{
    public List<RoomBlueprint> roomBlueprints;
}

[System.Serializable]
public class RoomBlueprint
{
    public int min;
    public int max;
    public RoomType roomType;
}
