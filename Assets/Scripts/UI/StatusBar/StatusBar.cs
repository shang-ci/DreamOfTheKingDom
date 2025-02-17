using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StatusBar : MonoBehaviour
{
    public GameObject statusItemPrefab; 
    public Transform statusBarTransform; 

    // ����״̬������ʾ
    public void UpdateStatusBar(Dictionary<string, int> statusEffects)
    {
        // ��յ�ǰ״̬��
        foreach (Transform child in statusBarTransform)
        {
            Destroy(child.gameObject);
        }

        // ��ʾ����״̬�������
        foreach (var effect in statusEffects)
        {
            GameObject statusItem = Instantiate(statusItemPrefab, statusBarTransform);
            statusItem.GetComponent<StatusItem>().Initialize(effect.Key, effect.Value);
        }
    }
}