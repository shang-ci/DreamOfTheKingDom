using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "MapConfigSO", menuName = "Map/MapConfigSO")]
public class MapConfigSO : ScriptableObject
{
    public List<RoomBlueprint> roomBlueprints;

    public void Initialize(List<RoomBlueprint> blueprints)
    {
        roomBlueprints = blueprints;
    }
}

[System.Serializable]
public class RoomBlueprint
{
    public int min;
    public int max;
    public RoomType roomType;
}
