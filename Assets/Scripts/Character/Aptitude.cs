using UnityEngine;

//◊ ÷ ¿‡
[System.Serializable]
public class Aptitude 
{
    private int value;

    public Aptitude(int value)
    {
        this.value = value;
    }

    public void SetAptitude(int value)
    {
        this.value = value;
    }

    public float GetAptitude()
    {
        return value;
    }
}
