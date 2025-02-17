using UnityEngine;

public class FinishRoom : MonoBehaviour
{
    public ObjectEventSO loadMapEvent;

    private void OnMouseDown()
    {
        // 返回地图
        loadMapEvent.RaiseEvent(null, this);
    }
}
