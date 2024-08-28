using System.Collections.Generic;
using UnityEngine;

public class CardLayoutManager : MonoBehaviour
{
    public bool isHorizontal;
    // 摄像机的大小是6，分辨率是1920x1080，也就是说摄像机可以看到(-10.7,-6)~(10.7,6)的物体，这里让手牌的位置从-3.5~3.5
    public float maxWidth = 7f;
    // 卡牌与卡牌之间最大的间隔是2
    public float cardSpacing = 2f;
    [Header("弧形参数")]
    // 已知卡牌最大宽度是7，半径是17，它的最大角度应该是 arcsin(7/2/17)*2，大概是 23.76231234995012
    public float maxAngle = 24f;
    // 卡牌与卡牌之间的最大角度间隔是7度
    public float angleBetweenCards = 7f;
    // 卡牌旋转的半径
    public float radius = 17f;
    // 中心点位置，表示卡牌在屏幕中的位置
    public Vector3 centerPoint;

    [SerializeField]
    private List<Vector3> cardPositions = new List<Vector3>();
    private List<Quaternion> cardRotations = new List<Quaternion>();

    private void Awake() {
        centerPoint = isHorizontal ? Vector3.up * -4.5f : Vector3.up * -21.5f;
    }

    /// <summary>
    /// 当一共有 totalCards 张卡牌的时候，计算出第 index 张卡牌的坐标和旋转
    /// </summary>
    /// <param name="index"></param>
    /// <param name="totalCards"></param>
    /// <returns></returns>
    public CardTransform GetCardTransform(int index, int totalCards)
    {
        CalculatePositoin(totalCards, isHorizontal);

        return new CardTransform(cardPositions[index], cardRotations[index]);
    }

    /// <summary>
    /// 计算卡牌的位置
    /// </summary>
    /// <param name="numberOfCards">当前有多少张卡牌</param>
    /// <param name="horizontal">横向还是扇形</param>
    private void CalculatePositoin(int numberOfCards, bool horizontal)
    {
        // 首先要清理一下之前算好的位置
        cardPositions.Clear();
        cardRotations.Clear();

        if (horizontal)
        {
            // 卡牌是横向排列的

            // 一共有 numberOfCards 张卡牌，卡牌与卡牌之间的间距是 cardSpacing，那么它们之间的总间距是 currentWidth
            float currentWidth = cardSpacing * (numberOfCards - 1);
            // 如果卡牌太多超过 maxWidth 的话，就需要将最大宽度设置为 maxWidth
            float totalWidth = Mathf.Min(currentWidth, maxWidth);

            // 算出当前卡牌与卡牌之间的间距
            float currentSpacing = totalWidth > 0 ? totalWidth / (numberOfCards - 1) : 0;
            for (int i = 0; i < numberOfCards; i++) 
            {
                // 让卡牌从最左边以此排过来
                float xPos = 0 - totalWidth / 2 + currentSpacing * i;

                var pos = new Vector3(xPos, centerPoint.y, 0);
                var rotation = Quaternion.identity;

                // 然后记录它们的位置和旋转
                cardPositions.Add(pos);
                cardRotations.Add(rotation);
            }
        }
        else
        {
            // 算出第一张卡牌的角度
            float totalAngle = (numberOfCards - 1) * angleBetweenCards;
            totalAngle = Mathf.Min(totalAngle, maxAngle);
            float currentAngleBetweenCards;
            if (numberOfCards <= 1)
            {
                currentAngleBetweenCards = 0;
            }
            else
            {
                currentAngleBetweenCards = totalAngle / (numberOfCards - 1);
            }
            float cardAngle = totalAngle / 2;
            for (int i = 0; i < numberOfCards; i++)
            {
                float angle = cardAngle - i * currentAngleBetweenCards;
                // Debug.Log($"angle = {angle}, cardAngle = {cardAngle}, currentAngleBetweenCards = {currentAngleBetweenCards}");
                // 算出其它卡牌的角度
                var pos = FanCardPosition(angle);
                // 根据左手定理，当 cardAngle 为正的时候，卡牌向左旋转；当 cardAngle 为负的时候，卡牌向右旋转
                var rotation = Quaternion.Euler(0, 0, angle);
                cardPositions.Add(pos);
                cardRotations.Add(rotation);
            }
        }
    }

    private Vector3 FanCardPosition(float angle)
    {
        // 因为第一张牌的角度是正的，但是我们希望它在最左侧的位置，所以只能通过 centerPoint.x - Mathf.Sin 来算出实际的 x 坐标
        return new Vector3(
            centerPoint.x - Mathf.Sin(Mathf.Deg2Rad * angle) * radius,
            centerPoint.y + Mathf.Cos(Mathf.Deg2Rad * angle) * radius,
            0
        );
    }
}
