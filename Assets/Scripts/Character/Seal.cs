using UnityEngine;
using UnityEngine.Windows;
using System.Collections.Generic;

//道印
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

    // 获取所有道印
    public Dictionary<Attribute, double> GetAttributes()
    {
        return attributes;
    }

    // 添加道印值
    public void AddSealValue(Attribute attribute, double value)
    {
        if (attributes.ContainsKey(attribute))
        {
            attributes[attribute] += value;
        }
    }

    // 移除道印值
    public void RemoveSealValue(Attribute attribute, double value)
    {
        if (attributes.ContainsKey(attribute))
        {
            attributes[attribute] = System.Math.Max(0, attributes[attribute] - value);
        }
    }

    // 获取道印值
    public double GetSealValue(Attribute attribute)
    {
        if (attributes.ContainsKey(attribute))
        {
            return attributes[attribute];
        }
        return 0;
    }
}
