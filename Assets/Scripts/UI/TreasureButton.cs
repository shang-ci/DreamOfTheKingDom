using UnityEngine;
using UnityEngine.EventSystems;

public class TreasureButton : MonoBehaviour, IPointerDownHandler
{
    public ObjectEventSO gameWinEvent;

    public void OnPointerDown(PointerEventData eventData)
    {
        gameWinEvent.RaiseEvent(null, this);
    }
}
