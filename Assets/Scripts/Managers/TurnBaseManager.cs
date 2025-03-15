using System.Collections.Generic;
using UnityEngine;

//战斗中的回合管理、是否开启战斗、玩家的显示控制
public class TurnBaseManager : MonoBehaviour
{
    public static TurnBaseManager instance;

    public GameObject playerObj;
    public Player player;
    public Enemy enemy;

    [SerializeField]private bool isPlayerTurn = false;
    private bool isEnemyTurn = false;
    public bool battleEnd = true;//战斗是否结束——只有处于战斗时，才会进行回合管理

    private float timeCounter;//计时器，管理每个回合的时间
    public float enemyTurnDuration;
    public float playerTurnDuration;

    private int playerTurnCount = 0; // 玩家回合计数
    private int enemyTurnCount = 0; // 敌人回合计数

    [Header("事件广播")]
    public ObjectEventSO playerTurnBegin;//玩家回合结束用敌人回合开始代替表示，也可以在这里把抽牌阶段抽取出来
    public ObjectEventSO enemyTurnBegin;
    public ObjectEventSO enemyTurnEnd;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Update() 
    {
        if (battleEnd)    
        {
            return;
        }

        if (isEnemyTurn)
        {
            timeCounter += Time.deltaTime;
            if (timeCounter >= enemyTurnDuration)
            {
                timeCounter = 0;
                // 敌人回合结束
                EnemyTurnEnd();               
            }
        }

        if (isPlayerTurn)
        {
            timeCounter += Time.deltaTime;
            if (timeCounter >= playerTurnDuration)
            {
                timeCounter = 0f;
                // 玩家回合开始
                PlayerTurnBegin();
                isPlayerTurn = false;
            }
        }
    }

    [ContextMenu("Game Start")]
    public void GameStart()
    {
        playerObj.GetComponent<Player>().ClearAllStatusEffects();//清除玩家身上的所有状态
        player = playerObj.GetComponent<Player>();
        player.deck = CardDeck.instance.drawDeck;//这里能保证player的deck每次都是最新的
        enemy = (Enemy)GameManager.Instance.GetSingleOrMultipleEnemies();//TOOD：这里返回的敌人可以是多个，所以不能只用Enemy类型接收，以后要改成Enemy[]

        isPlayerTurn = true;
        isEnemyTurn = false;
        battleEnd = false;
        timeCounter = 0;
        playerTurnCount = 0;
        enemyTurnCount = 0;

        //测试代码——强化卡还原移除强化效果
        //List<CardDataSO> attackCards = CardDeck.instance.GetAllCardDataByName("攻击");
        //foreach (var cardData in attackCards)
        //{
        //    Debug.Log(cardData.description);
        //}
    }

    public void PlayerTurnBegin()
    {
        playerTurnCount++;
        playerTurnBegin.RaiseEvent(null, this);
        player.UpdateStatusEffectRounds();//更新玩家状态效果回合数
    }

    //玩家回合结束调用——当点击回合转换按钮时玩家回合结束
    public void EnemyTurnBegin()
    {
        enemyTurnCount++;
        isEnemyTurn = true;
        enemyTurnBegin.RaiseEvent(null, this);
        enemy.UpdateStatusEffectRounds();//更新敌人状态效果回合数
    }

    public void EnemyTurnEnd()
    {
        isEnemyTurn = false;
        enemyTurnEnd.RaiseEvent(null, this);
        isPlayerTurn = true;
    }

    //控制玩家的显示&&游戏开始——在点击房间时判断房间类型调用
    public void AfterRoomLoadedEvent(object obj)
    {
        Room room = obj as Room;
        switch (room.roomData.roomType)
        {
            case RoomType.MinorEnemy:
            case RoomType.EliteEnemy:
            case RoomType.Boss:
                playerObj.SetActive(true);
                GameStart();//启动游戏
                break;
            case RoomType.Shop:
            case RoomType.Treasure:
                playerObj.SetActive(false);
                break;
            case RoomType.RestRoom:
                playerObj.SetActive(true);
                playerObj.GetComponent<PlayerAnimation>().SetSleepAnimation();
                break;
        }
    }

    //停止对战——当对战去的胜负时调用——gameover、loadma事件
    public void StopTurnBaseSystem(object obj) 
    {
        battleEnd = true;
        playerObj.SetActive(false);
    }

    //初始化玩家——在点击新游戏时调用，因为玩家自身是隐藏的无法监听到新游戏事件
    public void NewGame()
    {
        playerObj.GetComponent<Player>().NewGame();
    }
}
