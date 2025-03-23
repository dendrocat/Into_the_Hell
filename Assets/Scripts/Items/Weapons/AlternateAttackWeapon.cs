using System.Collections;
using UnityEngine;

public class AlternateAttackWeapon : BaseWeapon
{
    public float altDamage;
    public float baseAltReloadTime;
    public float baseAltPrepareAttackTime;
    public float baseAltEndAttackTime;
    public bool altReloading = false;
    public void LaunchAltAttack()
    {
        if (!altReloading && CheckAltAttackConditions())
        {
            altReloading = true;
            StartCoroutine(PerformAltAttack());
        }
    }

    private IEnumerator PerformAltAttack()
    {
        yield return new WaitForSeconds(CalcScaleDescending(baseAltPrepareAttackTime));
        AltAttack();
        yield return new WaitForSeconds(CalcScaleDescending(baseAltEndAttackTime));
        StartCoroutine(ReloadAltWeapon(CalcScaleDescending(baseAltReloadTime)));
    }

    protected virtual bool CheckAltAttackConditions() //����� ��� ��������, �������� �� ��������������� �����.
                                                      //���������������� � �������-�����������
    {
        return false;
    }

    protected virtual void AltAttack() //����� ��� �������������� �����. ���������������� � �������-�����������
    {

    }

    private IEnumerator ReloadAltWeapon(float reloadTime)
    {
        yield return new WaitForSeconds(reloadTime);
        altReloading = false;
    }
}
