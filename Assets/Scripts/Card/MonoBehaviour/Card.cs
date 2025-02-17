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
    // 卡牌原始位置——鼠标选中时，卡牌会上移，所以需要记录原始位置
    public Vector3 originalPosition;
    public Quaternion originalRotation;
    public int originalLayerOrder;// 卡牌原始层级

    public bool isAnimating;//是否正在移动中/抽卡的动画中——若是在移动就不能让它有上移效果
    public bool isAvailiable;//判断player的当前能量是否足够使用这张卡

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

    //保存卡牌的原始位置和旋转、层级
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
        transform.position = originalPosition + Vector3.up;//这里用新变量保存，防止多次经过卡牌时，transform.position.y 不断增加
        transform.rotation = Quaternion.identity;
        GetComponent<SortingGroup>().sortingOrder = 20;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (isAnimating)
            return;
        ResetCardTransform();
    }

    // 重置卡牌位置——鼠标离开时调用
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

        //防止卡牌的两种效果为空
        if (cardData.effects != null)
        {
            foreach (var effect in cardData.effects)
            {
                effect.Execute(from, target);
            }
        }

        if(cardData.statusEffects != null)
        {
            foreach (var statusEffect in cardData.statusEffects)
            {
                //注意这里是给from添加状态效果，而不是target
                from.AddStatusEffect(statusEffect);
            }
        }
    }

    //更新卡牌状态颜色标识玩家的能量是否够用——判断是否能使用
    public void UpdateCardState()
    {
        isAvailiable = cardData.cost <= player.CurrentMana;
        costText.color = isAvailiable ? Color.green : Color.red;
    }


    // 更新显示的UI
    public void UpdateCardDisplay()
    {
        costText.text = cardData.cost.ToString();
        descriptionText.text = cardData.description;
        // 更新效果描述
        foreach (var effect in cardData.effects)
        {
            if (effect is DamageEffect)
            {
                descriptionText.text += $"\n伤害: {effect.value}";
            }
        }
    }
}
