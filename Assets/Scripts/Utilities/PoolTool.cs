using System.Diagnostics.Tracing;
using UnityEngine;
using UnityEngine.Pool;

[DefaultExecutionOrder(-100)]
public class PoolTool : MonoBehaviour
{
    public GameObject objPrefab;
    private ObjectPool<GameObject> pool;

    private void Awake()
    {
        // 初始化对象池
        pool = new ObjectPool<GameObject>(
            createFunc: () => Instantiate(objPrefab, transform),
            actionOnGet: (obj) => obj.SetActive(true),
            actionOnRelease: (obj) => obj.SetActive(false),
            actionOnDestroy: (obj) => Destroy(obj),
            // 是否检查我们的对象
            collectionCheck: false,
            defaultCapacity: 10,
            maxSize: 20
        );

        PreFillPool(7);
    }

    private void PreFillPool(int count) 
    {
        var preFillArray = new GameObject[count];

        for (int i = 0; i < count; i++)
        {
            preFillArray[i] = pool.Get();
        }
        foreach (var item in preFillArray)
        {
            pool.Release(item);
        }
    }

    public GameObject GetObjectFromPool()
    {
        return pool.Get();
    }

    public void ReleaseObjectToPool(GameObject obj)
    {
        pool.Release(obj);
    }
}
