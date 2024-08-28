using UnityEngine;
using UnityEngine.UIElements;
using DG.Tweening;

public class FadePanel : MonoBehaviour
{
    private VisualElement background;

    private void Awake() 
    {
        background = GetComponent<UIDocument>().rootVisualElement.Q<VisualElement>("Background");
    }

    public void FadeIn(float duration)
    {
        DOVirtual.Float(0, 1, duration, value => 
        {
            background.style.opacity = value;
        }).SetEase(Ease.InQuad);
    }

    public void FadeOut(float duration)
    {
        DOVirtual.Float(1, 0, duration, value => 
        {
            background.style.opacity = value;
        }).SetEase(Ease.InQuad);
    }
}
