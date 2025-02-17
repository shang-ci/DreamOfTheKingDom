using UnityEngine;
using UnityEngine.EventSystems;

// 卡牌拖拽处理器
public class CardDragHandler : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    [Header("拖拽箭头")]
    public GameObject arrowPrefab;
    private GameObject currentArrow;

    private Card currentCard;
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
}
