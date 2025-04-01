using UnityEngine;
using UnityEngine.EventSystems;
using static UnityEngine.GraphicsBuffer;

// 卡牌拖拽处理器
public class CardDragHandler : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    [Header("拖拽箭头")]
    public GameObject arrowPrefab;
    private GameObject currentArrow;

    public Card currentCard;
    private bool canMove;//是否可以拖拽，当尝试拖拽时，如果卡牌是攻击类型，就会生成箭头，如果是技能或能力类型，就可以拖拽
    private bool canExecute;//是否可以执行，当拖拽结束时，如果鼠标在敌人上，就可以执行；如果是针对玩家自己的技能或能力，只要超过1f就可以执行，就是说只要拖拽到屏幕上方y轴大于1f就可以执行
    private CharacterBase targetCharacter;

    private void Awake() 
    {   
        currentCard = GetComponent<Card>();
    }

    //当卡牌被回收时，就把canMove和canExecute设置为false——防止其在下一轮被重新赋予data时，还保留上一轮的状态
    private void OnDisable() 
    {
        canMove = false;
        canExecute = false;    
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (!currentCard.isAvailiable)
        {
            return;
        }
        switch (currentCard.cardData.cardType)
        {
            // 如果是攻击类型，就生成箭头
            case CardType.Attack:
                currentArrow = Instantiate(arrowPrefab, transform.position, Quaternion.identity);
                break;
            
            //如果是技能或能力类型，就可以拖拽
            case CardType.Defense:
            case CardType.Abilities:
                canMove = true;
                break;
        }
    }

    //根据effect/statusEffect的效果类型，来确定effect实施的目标
    public void OnDrag(PointerEventData eventData)
    {
        if (!currentCard.isAvailiable)
        {
            return;
        }

        //针对技能或能力类型的卡牌，也就是卡牌类型是对玩家其效果的，可以拖拽
        if (canMove)
        {
            currentCard.isAnimating = true;//防止卡牌在拖拽时有上提的动画效果

            // 将鼠标屏幕坐标转换为世界坐标——实现卡牌跟随鼠标移动拖拽效果
            Vector3 screenPos = new Vector3(Input.mousePosition.x, Input.mousePosition.y, 10.0f);
            Vector3 worldPos = Camera.main.ScreenToWorldPoint(screenPos);
            currentCard.transform.position = worldPos;

            //根据卡牌的效果类型，来确定effect实施的目标
            if(currentCard.cardData.effects != null)
            {
                foreach (var effect in currentCard.cardData.effects)
                {
                    switch (effect.targetType)
                    {
                        case EffectTargetType.Self:
                            targetCharacter = currentCard.player;
                            break;
                        case EffectTargetType.Target:
                            // 这里可以添加逻辑来选择一个特定的目标，例如通过点击选择敌人
                            targetCharacter = (CharacterBase)GameManager.instance.GetSingleOrMultipleEnemies();
                            break;
                        case EffectTargetType.ALL:
                            // 这里可以添加逻辑来选择所有目标，例如对所有敌人造成伤害
                            //targetCharacter = GameManager.instance.GetAllEnemies();
                            //TOOD:敌人会有多个，接受多个目标
                            break;
                        case EffectTargetType.Our:
                            break;
                    }
                }
            }

            if(currentCard.cardData.statusEffects != null)
            {
                foreach (var statusEffect in currentCard.cardData.statusEffects)
                {
                    switch (statusEffect.targetType)
                    {
                        case StatusEffectTargetType.Self:
                            targetCharacter = currentCard.player;
                            break;
                        case StatusEffectTargetType.Target:
                            // 这里可以添加逻辑来选择一个特定的目标，例如通过点击选择敌人
                            targetCharacter = (CharacterBase)GameManager.instance.GetSingleOrMultipleEnemies();
                            break;
                        case StatusEffectTargetType.ALL:
                            // 这里可以添加逻辑来选择所有目标，例如对所有敌人造成伤害
                            //targetCharacter = GameManager.instance.GetAllEnemies();
                            //TOOD:敌人会有多个，接受多个目标
                            targetCharacter = null;
                            break;
                        case StatusEffectTargetType.Our:
                            break;
                    }
                }
            }

            //当拖拽到屏幕上方y大于1f，就可以执行
            canExecute = worldPos.y > 1f;
        }
        else
        {
            if (eventData.pointerEnter == null)
            {
                canExecute = false;
                targetCharacter = null;
                return;
            }

            if (eventData.pointerEnter.CompareTag("Enemy"))
            {
                canExecute = true;
                targetCharacter = eventData.pointerEnter.GetComponent<CharacterBase>();
                return;
            }
            else
            {
                canExecute = false; 
                targetCharacter = null;
            }
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (!currentCard.isAvailiable)
        {
            return;
        }
        if (currentArrow != null)
        {
            Destroy(currentArrow);
        }
        if (canExecute)
        {
            currentCard.ExecuteCardEffects(currentCard.player, targetCharacter);
        }
        else
        {
            currentCard.ResetCardTransform();
            currentCard.isAnimating = false;//拖拽结束后，若是回到原位卡牌仍旧可以上提
        }
    }

    //获取当前拖拽的卡牌
    public Card GetCurrentDraggedCard()
    {
        return currentCard;
    }
}
