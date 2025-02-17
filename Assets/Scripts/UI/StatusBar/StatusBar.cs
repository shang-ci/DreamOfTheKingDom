using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StatusBar : MonoBehaviour
{
    public GameObject statusItemPrefab; 
    public Transform statusBarTransform; 

    // 更新状态条的显示
    public void UpdateStatusBar(Dictionary<string, int> statusEffects)
    {
        // 清空当前状态条
        foreach (Transform child in statusBarTransform)
        {
            Destroy(child.gameObject);
        }

        // 显示所有状态及其点数
        foreach (var effect in statusEffects)
        {
            GameObject statusItem = Instantiate(statusItemPrefab, statusBarTransform);
            statusItem.GetComponent<StatusItem>().Initialize(effect.Key, effect.Value);
        }
    }
}