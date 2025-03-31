using System.Collections.Generic;

//成长系统——打败敌人获取道印升级部分
public class GrowthSystem
{
    private Attribute attribute;//属性
    private Aptitude aptitude;//资质
    private Seal seal;//道印
    public int coin;//硬币数量——或许可以单独做个资源类

    //拿到玩家的资质和道印
    public GrowthSystem()
    {
        attribute = Attribute.gold;
        aptitude = new Aptitude(1);
        seal = new Seal(Attribute.gold);
        coin = 1000;
    }

    public void AddCoin(int value)
    {
        coin += value;
    }

    public int GetCoin()
    {
        return coin;
    }

    public void ExpendCoin(int value)
    {
        coin -= value;
    }

    // 获取道印值
    public double GetSealValue(Attribute attribute)
    {
        return seal.GetSealValue(attribute);
    }

    // 添加道印值
    public void AddSealValue(Attribute attribute, double value)
    {
        seal.AddSealValue(attribute, value);
    }

    // 移除道印值
    public void RemoveSealValue(Attribute attribute, double value)
    {
        seal.RemoveSealValue(attribute, value);
    }

    // 获取资质值
    public float GetAptitude()
    {
        return aptitude.GetAptitude();
    }

    // 设置资质值
    public void SetAptitude(int value)
    {
        aptitude.SetAptitude(value);
    }

    // 计算道印获取比例
    public float CalculateSealAcquisitionRate()
    {
        return aptitude.GetAptitude() * 0.01f;
    }

    // 转移道印——击败敌人后，将敌人的道印转移到玩家身上
    public void TransferSeals(GrowthSystem from, GrowthSystem to)
    {
        foreach (Attribute attr in System.Enum.GetValues(typeof(Attribute)))
        {
            double value = from.GetSealValue(attr) * to.CalculateSealAcquisitionRate();
            to.AddSealValue(attr, value);
        }
    }
}