using UnityEngine;
using UnityEngine.EventSystems;

// ������ק������
public class CardDragHandler : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    [Header("��ק��ͷ")]
    public GameObject arrowPrefab;
    private GameObject currentArrow;

    private Card currentCard;
    private bool canMove;//�Ƿ������ק����������קʱ����������ǹ������ͣ��ͻ����ɼ�ͷ������Ǽ��ܻ��������ͣ��Ϳ�����ק
    private bool canExecute;//�Ƿ����ִ�У�����ק����ʱ���������ڵ����ϣ��Ϳ���ִ�У�������������Լ��ļ��ܻ�������ֻҪ����1f�Ϳ���ִ�У�����˵ֻҪ��ק����Ļ�Ϸ�y�����1f�Ϳ���ִ��
    private CharacterBase targetCharacter;

    private void Awake() 
    {   
        currentCard = GetComponent<Card>();
    }

    //�����Ʊ�����ʱ���Ͱ�canMove��canExecute����Ϊfalse������ֹ������һ�ֱ����¸���dataʱ����������һ�ֵ�״̬
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
            // ����ǹ������ͣ������ɼ�ͷ
            case CardType.Attack:
                currentArrow = Instantiate(arrowPrefab, transform.position, Quaternion.identity);
                break;
            
            //����Ǽ��ܻ��������ͣ��Ϳ�����ק
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

        //��Լ��ܻ��������͵Ŀ��ƣ�Ҳ���ǿ��������Ƕ������Ч���ģ�������ק
        if (canMove)
        {
            currentCard.isAnimating = true;//��ֹ��������קʱ������Ķ���Ч��

            // �������Ļ����ת��Ϊ�������ꡪ��ʵ�ֿ��Ƹ�������ƶ���קЧ��
            Vector3 screenPos = new Vector3(Input.mousePosition.x, Input.mousePosition.y, 10.0f);
            Vector3 worldPos = Camera.main.ScreenToWorldPoint(screenPos);
            currentCard.transform.position = worldPos;

            //����ק����Ļ�Ϸ�y����1f���Ϳ���ִ��
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
            currentCard.isAnimating = false;//��ק���������ǻص�ԭλ�����Ծɿ�������
        }
    }
}
