using UnityEngine;

public class VFXController : MonoBehaviour
{
    public GameObject buff;
    public GameObject debuff;
    private float timeCounter;

    private void Update() 
    {
        if (buff.activeInHierarchy)    
        {
            timeCounter += Time.deltaTime;
            if (timeCounter >= 1.2f)
            {
                timeCounter = 0;
                buff.SetActive(false);
            }
        }

        if (debuff.activeInHierarchy)    
        {
            timeCounter += Time.deltaTime;
            if (timeCounter >= 1.2f)
            {
                timeCounter = 0;
                debuff.SetActive(false);
            }
        }
    }
}
