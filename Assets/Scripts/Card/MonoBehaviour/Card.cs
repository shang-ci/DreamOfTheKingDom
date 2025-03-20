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
    [SerializeField]private CardDataSO originalCardData; // 保存原始卡牌数据

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

    private void Awake()
    {
        //player = TurnBaseManager.instance.player;
    }

    private void Start() 
    {
        //Init(cardData);这里会影响到卡牌的数据，会把副本又加到卡牌上，所以这里不需要初始化
        //player = TurnBaseManager.instance.player;
    }

    //拿到的data一直是最开始的版本
    public void Init(CardDataSO data)
    {
        originalCardData = data; // 保存原始卡牌数据
        cardData = CardManager.instance.GetCardDataClone(data); // 使用卡牌数据的副本

        //使用carddata的副本初始化卡牌的数据——这样不论是第一次抽取这张卡还是怎样都使用
        cardSprite.sprite = cardData.cardImage;
        costText.text = cardData.cost.ToString();
        descriptionText.text = cardData.description;
        typeText.text = cardData.cardType switch
        {
            CardType.Attack => "攻击",
            CardType.Defense => "技能",
            CardType.Abilities => "能力",
            _ => throw new System.NotImplementedException(),
        };
        cardName.text = cardData.cardName;

        //player = GameObject.FindWithTag("Player").GetComponent<Player>();//这里的player需要在玩家回合开始时就要获得，而抽卡这一行动在加载房间时就会执行，所以不用turnbase的player
        player = TurnBaseManager.instance.player;
    }

    //状态卡更新卡牌的数据
    public void UpdateCardDataUI(CardDataSO data)
    {
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
    }

    //保存卡牌的原始位置和旋转、层级
    public void UpdatePositionRotation(Vector3 position, Quaternion rotation)
    {
        this.originalPosition = position;
        this.originalRotation = rotation;
        this.originalLayerOrder = GetComponent<SortingGroup>().sortingOrder;
    }

    
    public void OnPointerDown(PointerEventData eventData)
    {
        // 开始拖拽
        //CardDragHandler.currentCard = this;
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
        if (cardData.effects != null)//拿副本执行效果
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
                //根据状态效果的类型执行不同的效果
                target.AddStatusEffect(statusEffect);
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
        cardSprite.sprite = cardData.cardImage;
        costText.text = cardData.cost.ToString();
        descriptionText.text = cardData.description;
        typeText.text = cardData.cardType switch
        {
            CardType.Attack => "攻击",
            CardType.Defense => "技能",
            CardType.Abilities => "能力",
            _ => throw new System.NotImplementedException(),
        };
        cardName.text = cardData.cardName;
    }

    // 还原卡牌数据
    public void RestoreOriginalCardData()
    {
        cardData = originalCardData;
        UpdateCardDisplay();
    }

    // 获取原始卡牌数据
    public CardDataSO GetOriginalCardData()
    {
        return originalCardData;
    }
}
