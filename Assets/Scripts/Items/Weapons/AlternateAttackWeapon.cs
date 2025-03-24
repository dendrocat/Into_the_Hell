using System.Collections;
using UnityEngine;

public class AlternateAttackWeapon : BaseWeapon
{
    public float altDamage;
    public float baseAltReloadTime;
    public float baseAltPrepareAttackTime;
    public float baseAltEndAttackTime;
    public bool altReloading = false;
    public virtual void LaunchAltAttack()
    {
        if (!altReloading && CheckAltAttackConditions())
        {
            altReloading = true;
            StartCoroutine(PerformAltAttack());
        }
    }

    private IEnumerator PerformAltAttack()
    {
        OnPrepareAltAttackStart();
        yield return new WaitForSeconds(CalcScaleDescending(baseAltPrepareAttackTime));
        OnPrepareAltAttackEnd();
        AltAttack();
        OnEndAltAttackStart();
        yield return new WaitForSeconds(CalcScaleDescending(baseAltEndAttackTime));
        OnEndAltAttackEnd();
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

    protected virtual void OnPrepareAltAttackStart() //��������, ����������� ����� ������� ���������� ���� �����
    {

    }

    protected virtual void OnPrepareAltAttackEnd() //��������, ����������� ����� ��������� ���������� ���� �����
    {

    }

    protected virtual void OnEndAltAttackStart() //��������, ����������� ����� ������� ��������� ���� �����
    {

    }

    protected virtual void OnEndAltAttackEnd() //��������, ����������� ����� ��������� ��������� ���� �����
    {

    }

    private IEnumerator ReloadAltWeapon(float reloadTime)
    {
        yield return new WaitForSeconds(reloadTime);
        altReloading = false;
    }
}
