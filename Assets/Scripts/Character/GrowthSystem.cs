using System.Collections.Generic;

//�ɳ�ϵͳ������ܵ��˻�ȡ��ӡ��������
public class GrowthSystem
{
    private Attribute attribute;//����
    private Aptitude aptitude;//����
    private Seal seal;//��ӡ
    public int coin;//Ӳ����������������Ե���������Դ��

    //�õ���ҵ����ʺ͵�ӡ
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

    // ��ȡ��ӡֵ
    public double GetSealValue(Attribute attribute)
    {
        return seal.GetSealValue(attribute);
    }

    // ��ӵ�ӡֵ
    public void AddSealValue(Attribute attribute, double value)
    {
        seal.AddSealValue(attribute, value);
    }

    // �Ƴ���ӡֵ
    public void RemoveSealValue(Attribute attribute, double value)
    {
        seal.RemoveSealValue(attribute, value);
    }

    // ��ȡ����ֵ
    public float GetAptitude()
    {
        return aptitude.GetAptitude();
    }

    // ��������ֵ
    public void SetAptitude(int value)
    {
        aptitude.SetAptitude(value);
    }

    // �����ӡ��ȡ����
    public float CalculateSealAcquisitionRate()
    {
        return aptitude.GetAptitude() * 0.01f;
    }

    // ת�Ƶ�ӡ�������ܵ��˺󣬽����˵ĵ�ӡת�Ƶ��������
    public void TransferSeals(GrowthSystem from, GrowthSystem to)
    {
        foreach (Attribute attr in System.Enum.GetValues(typeof(Attribute)))
        {
            double value = from.GetSealValue(attr) * to.CalculateSealAcquisitionRate();
            to.AddSealValue(attr, value);
        }
    }
}