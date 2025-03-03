using UnityEngine;

//����֮��:�鿨��ÿ��һ���������͵ģ�Attack���ƾ����value���˺�
[CreateAssetMenu(fileName = "StarMoonlightEffect", menuName = "CardEffect/StarMoonlightEffect", order = 0)]
public class StarMoonlightEffect : Effect
{
    public override void Execute(CharacterBase from, CharacterBase target)
    { 
        // �����˺�
        int damage = value * CardDeck.instance.GetDrawDeckCountByType();

        // ��Ŀ������˺�
        target.TakeDamage(damage);
    }
}