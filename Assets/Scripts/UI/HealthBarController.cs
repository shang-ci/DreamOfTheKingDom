using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UIElements;

public class HealthBarController : MonoBehaviour
{
    private CharacterBase currentCharacter;

    [Header("Elements")]
    public Transform healthBarTransform;
    private UIDocument healthBarDocument;
    private ProgressBar healthBar;

    //防御状态的控制
    private VisualElement defenseElement;
    private Label defenseAmountLabel;

    private VisualElement buffElement;
    private Label buffRound;
    [Header("buff sprite")]
    public Sprite buffSprite;
    public Sprite debuffSprite;

    //制作敌人的意图图标显示
    private Enemy enemy;
    private VisualElement intentSprite;
    private Label intentAmount;

    // 状态条相关
    private VisualElement statusBarElement;

    private void Awake() 
    {
        currentCharacter = GetComponent<CharacterBase>(); 
        enemy = GetComponent<Enemy>();
    }

    //初始化血条——正好让其在玩家启用时调用
    private void OnEnable() 
    {
        InitHealthBar();
    }

    //让制作的UI血条位于玩家头顶
    private void MoveToWorldPosition(VisualElement element, Vector3 worldPosition, Vector2 size)
    {
        Rect rect = RuntimePanelUtils.CameraTransformWorldToPanelRect(element.panel, worldPosition, size, Camera.main);
        element.transform.position = rect.position;
    }

    
    [ContextMenu("Get UI Position")]
    public void InitHealthBar()
    {
        healthBarDocument = GetComponent<UIDocument>();
        healthBar = healthBarDocument.rootVisualElement.Q<ProgressBar>("HealthBar");

        healthBar.highValue = currentCharacter.MaxHP;//设置血条最大值
        MoveToWorldPosition(healthBar, healthBarTransform.position, Vector2.zero);//设置血条位置

        //获取防御的相关图标
        defenseElement = healthBar.Q<VisualElement>("Defense");
        defenseAmountLabel = defenseElement.Q<Label>("DefenseAmount");
        defenseElement.style.display = DisplayStyle.None;//图标隐藏

        buffElement = healthBar.Q<VisualElement>("Buff");
        buffRound = buffElement.Q<Label>("BuffRound");
        buffElement.style.display = DisplayStyle.None;

        intentSprite = healthBar.Q<VisualElement>("Intent");
        intentAmount = healthBar.Q<Label>("IntentAmount");
        intentSprite.style.display = DisplayStyle.None;

        // 初始化状态条
        statusBarElement = healthBar.Q<VisualElement>("StatusBar"); 
        statusBarElement.style.flexDirection = FlexDirection.Row; // 设置为水平排列
    }


    /// <summary>
    /// Update 方法每帧都会更新，如果想要更轻量级，建议使用事件监听
    /// </summary>
    private void Update() 
    {
        UpdateHealthBar();
    }

    //这个就是更新血条的方法，每次血量变化引起事件都会调用这个方法——unity UITool Uss实现
    public void UpdateHealthBar()
    {
        if (currentCharacter.isDead)
        {
            healthBar.style.display = DisplayStyle.None;
            return;
        }

        if (healthBar != null)
        {
            healthBar.title = $"{currentCharacter.CurrentHP}/{currentCharacter.MaxHP}";
            healthBar.value = currentCharacter.CurrentHP;

            //卸掉所有的USS的class——方便根据血量显示不同的颜色，达到不同血量显示不同颜色的效果，添加不同的class即可
            healthBar.RemoveFromClassList("highHealth");
            healthBar.RemoveFromClassList("mediumHealth");
            healthBar.RemoveFromClassList("lowHealth");
            var percentage = (float)currentCharacter.CurrentHP / (float)currentCharacter.MaxHP;
            if (percentage < 0.3f)
            {
                healthBar.AddToClassList("lowHealth");
            }
            else if (percentage < 0.6f)
            {
                healthBar.AddToClassList("mediumHealth");
            }
            else
            {
                healthBar.AddToClassList("highHealth");
            }

            // 防御属性更新
            defenseElement.style.display = currentCharacter.defense.currentValue > 0 ? DisplayStyle.Flex : DisplayStyle.None;
            defenseAmountLabel.text = currentCharacter.defense.currentValue.ToString();

            // buff 回合更新——显示buff图标和持续的回合数
            buffElement.style.display = currentCharacter.buffRound.currentValue > 0 ? DisplayStyle.Flex : DisplayStyle.None;
            buffRound.text = currentCharacter.buffRound.currentValue.ToString();
            buffElement.style.backgroundImage = currentCharacter.baseStrength > 1 ? new StyleBackground(buffSprite) : new StyleBackground(debuffSprite);


            // 更新状态条
            UpdateStatusBar();
        }
    }


    // 添加状态项
    private void AddStatusItem(string name, int value)
    {
        // 创建状态项容器
        VisualElement statusItem = new VisualElement();
        statusItem.AddToClassList("status-item");

        // 创建并添加状态名称
        TextElement statusName = new TextElement { text = name };
        statusName.AddToClassList("status-name");
        statusItem.Add(statusName);

        // 创建并添加状态点数
        TextElement statusValue = new TextElement { text = value.ToString() };
        statusValue.AddToClassList("status-value");
        statusItem.Add(statusValue);

        // 将状态项添加到状态条容器
        statusBarElement.Add(statusItem);
    }



    private void UpdateStatusBar()
    {
        // 清空当前状态条内容
        statusBarElement.Clear();

        // 遍历所有状态效果并生成对应的 UI 元素
        foreach (var effect in currentCharacter.GetStatusEffects())
        {
            AddStatusItem(effect.Key, effect.Value);
        }
    }


    //展示敌人的意图，在玩家回合开始时调用，是下一回合敌人的意图
    public void SetIntentElement()
    {
        intentSprite.style.display = DisplayStyle.Flex;
        intentSprite.style.backgroundImage = new StyleBackground(enemy.currentAction.intentSprite);

        // 判断是否是攻击
        var value = enemy.currentAction.effect.value;
        if (enemy.currentAction.effect.GetType() == typeof(DamageEffect))
        {
            value = (int) math.round(enemy.currentAction.effect.value * enemy.baseStrength);//计算伤害
        }

        intentAmount.text = value.ToString();
    }

    //隐藏敌人的意图——在敌人回合结束时调用
    public void HideIntentElement()
    {
        intentSprite.style.display = DisplayStyle.None;
    }
}
