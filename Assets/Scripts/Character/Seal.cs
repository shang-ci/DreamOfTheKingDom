using UnityEngine;
using UnityEngine.Windows;
using System.Collections.Generic;

//��ӡ
public class Seal
{
    private Dictionary<Attribute, double> attributes = new Dictionary<Attribute, double>();
    public Seal(Attribute attribute)
    {
            attributes[attribute] = 0;
    }

    public Seal(ref Dictionary<Attribute, double> _attributes)
    {
        this.attributes = _attributes;
    }

    // ��ȡ���е�ӡ
    public Dictionary<Attribute, double> GetAttributes()
    {
        return attributes;
    }

    // ��ӵ�ӡֵ
    public void AddSealValue(Attribute attribute, double value)
    {
        if (attributes.ContainsKey(attribute))
        {
            attributes[attribute] += value;
        }
    }

    // �Ƴ���ӡֵ
    public void RemoveSealValue(Attribute attribute, double value)
    {
        if (attributes.ContainsKey(attribute))
        {
            attributes[attribute] = System.Math.Max(0, attributes[attribute] - value);
        }
    }

    // ��ȡ��ӡֵ
    public double GetSealValue(Attribute attribute)
    {
        if (attributes.ContainsKey(attribute))
        {
            return attributes[attribute];
        }
        return 0;
    }
}
