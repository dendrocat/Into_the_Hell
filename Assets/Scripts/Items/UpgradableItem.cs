using UnityEngine;

//�����, ����������� ���������� �������
public class UpgradableItem : MonoBehaviour
{
    protected float scaleCoeff = 0.2f; //����������� ���������������.
                             //�������� � �������-����������� � ������ Start
    public byte level; //������� ������� ��������
    public byte maxLevel; //������������ ������� ��������

    public int GetUpgradeCost() //���������� ��������� ��������� �� ��������� �������
    {
        if (level < maxLevel) return 100 * level;
        else return -1; //���������� -1 ��� ���� � ���, ��� ������� ����������� �������
    }
    
    public float CalcScale(float scalable) //���������� �������� scalable, ���������� ��������
                                           //�������� ������ ��������
    {
        return scalable * (1f + (level - 1) * scaleCoeff);
    }

    public float CalcScaleDescending(float scalable) //���������� �������� scalable, �����������
                                                     //�������� �������� ������ ��������
    {
        float coeff = 0.5f / (maxLevel - 1);
        return scalable * (1f - (level - 1) * coeff);
    }

    public void Upgrade(byte diff) //�������, ���������� �� ������� ��������
    {
        level = (byte)Mathf.Clamp(level + diff, 1, maxLevel);
    }
}
