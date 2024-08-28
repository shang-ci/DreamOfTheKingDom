using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Rendering;

public class Card : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [Header("组件")]
    public SpriteRenderer cardSprite;
    public TextMeshPro costText;
    public TextMeshPro descriptionText;
    public TextMeshPro typeText;
    public TextMeshPro cardName;
    public CardDataSO cardData;

    [Header("原始数据")]
    public Vector3 originalPosition;
    public Quaternion originalRotation;
    public int originalLayerOrder;

    public bool isAnimating;
    public bool isAvailiable;

    public Player player;

    [Header("广播事件")]
    public ObjectEventSO discardCardEvent;
    public IntEventSO costEvent;

    private void Start() 
    {
        Init(cardData);
    }

    public void Init(CardDataSO data)
    {
        cardData = data;
        cardSprite.sprite = data.cardImage;
        costText.text = data.cost.ToString();
        descriptionText.text = data.description;
        typeText.text = data.cardType switch
        {
            CardType.Attack => "攻击",
            CardType.Defense => "技能",
            CardType.Abilities => "能力",
            _ => throw new System.NotImplementedException(),
        };
        cardName.text = data.cardName;

        player = GameObject.FindWithTag("Player").GetComponent<Player>();
    }

    public void UpdatePositionRotation(Vector3 position, Quaternion rotation)
    {
        this.originalPosition = position;
        this.originalRotation = rotation;
        this.originalLayerOrder = GetComponent<SortingGroup>().sortingOrder;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (isAnimating)
        {
            return;
        }
        // todo 如果当前是扇形布局的话，可以将 transform.position.y 修改为一个固定值，比如 3.5
        transform.position = originalPosition + Vector3.up;
        transform.rotation = Quaternion.identity;
        GetComponent<SortingGroup>().sortingOrder = 20;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (isAnimating)
        {
            return;
        }
        ResetCardTransform();
    }

    public void ResetCardTransform()
    {
        transform.SetPositionAndRotation(originalPosition, originalRotation);
        GetComponent<SortingGroup>().sortingOrder = originalLayerOrder;
    }

    public void ExecuteCardEffects(CharacterBase from, CharacterBase target)
    {
        // 减少对应能量，通知回收卡牌
        costEvent.RaiseEvent(cardData.cost, this);
        discardCardEvent.RaiseEvent(this, this);
        foreach (var effect in cardData.effects)
        {
            effect.Execute(from, target);
        }
    }

    public void UpdateCardState()
    {
        isAvailiable = cardData.cost <= player.CurrentMana;
        costText.color = isAvailiable ? Color.green : Color.red;
    }
}
