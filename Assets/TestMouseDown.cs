using UnityEngine;
using UnityEngine.EventSystems;

public class TestMouseDown : MonoBehaviour,IPointerDownHandler
{
    public void OnPointerDown(PointerEventData eventData)
    {
        Debug.Log("Mouse Down");
    }

}
