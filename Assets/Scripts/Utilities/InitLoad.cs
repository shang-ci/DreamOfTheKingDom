using UnityEngine;
using UnityEngine.AddressableAssets;

public class InitLoad : MonoBehaviour
{
    public AssetReference persistent;

    public void Awake()
    {
        Addressables.LoadSceneAsync(persistent);
    }
}
