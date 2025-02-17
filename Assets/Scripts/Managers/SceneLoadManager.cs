using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.SceneManagement;

public class SceneLoadManager : MonoBehaviour
{
    public FadePanel fadePanel;
    private AssetReference currentScene;
    public AssetReference map;
    public AssetReference menu;
    public AssetReference intro;

    // 当前房间的坐标，用于更新游戏进度，gamemanager中监听
    private Vector2Int currentRoomVector;
    private Room currentRoom;

    [Header("广播")]
    public ObjectEventSO afterRoomLoadedEvent;//房间加载完成后的事件——在turnbasemanager中监听，显示玩家并启动游戏
    public ObjectEventSO updateRoomEvent;//更新房间事件——根据房间坐标更新房间状态||传递当前房间坐标给gamemanager，让它更新房间状态以及地图数据

    private void Awake()
    {
        currentRoomVector = Vector2Int.one * -1;
        // LoadMenu();
        LoadIntro();
    }

    /// <summary>
    /// 在房间加载事件中监听
    /// </summary>
    /// <param name="data"></param>
    public async void OnLoadRoomEvent(object data)
    {
        if (data is Room)
        {
            currentRoom = data as Room;

            var currentData = currentRoom.roomData;
            currentRoomVector = new(currentRoom.column, currentRoom.line);

            currentScene = currentData.sceneToLoad;
        }

        // 卸载当前场景
        await UnloadSceneTask();

        // 加载房间
        await LoadSceneTask();

        afterRoomLoadedEvent.RaiseEvent(currentRoom, this);
    }

    /// <summary>
    /// 异步操作加载场景
    /// </summary>
    /// <returns></returns>
    private async Awaitable LoadSceneTask()
    {
        var s = currentScene.LoadSceneAsync(LoadSceneMode.Additive);
        await s.Task;

        if (s.Status == AsyncOperationStatus.Succeeded)
        {
            fadePanel.FadeOut(0.2f);
            SceneManager.SetActiveScene(s.Result.Scene);
        }
    }

    //卸载当前场景
    private async Awaitable UnloadSceneTask()
    {
        //等带淡出动画完成在卸载场景
        fadePanel.FadeIn(0.4f);
        await Awaitable.WaitForSecondsAsync(0.45f);
        await Awaitable.FromAsyncOperation(SceneManager.UnloadSceneAsync(SceneManager.GetActiveScene()));
    }


    //加载地图——当点击返回地图按钮时触发loadmap事件||newgame从而进入加载地图触发，从而执行这个函数
    public async void LoadMap()
    {
        await UnloadSceneTask();

        if (currentRoomVector != Vector2.one * -1)
        {
            updateRoomEvent.RaiseEvent(currentRoomVector, this);//传递当前房间坐标给gamemanager，让它更新房间状态以及地图数据
        }
        currentScene = map;
        await LoadSceneTask();
    }


    //加载menu场景——游戏结束时在gameoverpanel中触发loadmenu事件，从而执行这个函数
    public async void LoadMenu()
    {
        if (currentScene != null)
        {
            await UnloadSceneTask();
        }
        currentScene = menu;
        await LoadSceneTask();
    }

    public async void LoadIntro()
    {
        if (currentScene != null)
        {
            await UnloadSceneTask();
        }
        currentScene = intro;
        await LoadSceneTask();
    }
}
