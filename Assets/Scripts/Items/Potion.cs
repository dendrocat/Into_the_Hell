using UnityEngine;

/**
 * <summary>
 * �����, ����������� ����� ������.
 * </summary>
 * **/
public class Potion : UpgradableItem
{
    float baseHeal = 50f;

    private void Start()
    {
        maxLevel = 3;
        scaleCoeff = 1f;
    }

    /**
     * <summary>
     * �������, ������������ ���������� ��, ����������������� ������.
     * </summary>
     * <returns>���������� ��, ����������������� ������.</returns>
     * **/
    public float getTotalHeal()
    {
        return CalcScale(baseHeal);
    }
}
