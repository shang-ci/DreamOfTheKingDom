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

    private Vector2Int currentRoomVector;
    private Room currentRoom;

    [Header("广播")]
    public ObjectEventSO afterRoomLoadedEvent;
    public ObjectEventSO updateRoomEvent;

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

            // Debug.Log($"{currentData.roomType}");

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

    private async Awaitable UnloadSceneTask()
    {
        fadePanel.FadeIn(0.4f);
        await Awaitable.WaitForSecondsAsync(0.45f);
        await Awaitable.FromAsyncOperation(SceneManager.UnloadSceneAsync(SceneManager.GetActiveScene()));
    }

    /// <summary>
    /// 监听返回房间的事件函数
    /// </summary>
    public async void LoadMap()
    {
        await UnloadSceneTask();

        if (currentRoomVector != Vector2.one * -1)
        {
            updateRoomEvent.RaiseEvent(currentRoomVector, this);
        }
        currentScene = map;
        await LoadSceneTask();
    }

    /// <summary>
    /// 返回菜单
    /// </summary>
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
