using System.Collections;
using UnityEngine;

//������� ����� ������
public class BaseWeapon : UpgradableItem
{
    public Texture2D weaponTexture;
    public Person owner;
    public float damage;
    public float baseReloadTime;
    public float basePrepareAttackTime;
    public float baseEndAttackTime;
    public bool reloading = false;

    void Start()
    {
        scaleCoeff = 1f;
    }

    public float getScaledDamage()
    {
        return CalcScale(damage);
    }

    public void LaunchAttack()
    {
        if (!reloading && CheckAttackConditions())
        {
            reloading = true;
            StartCoroutine(PerformAttack());
        }
    }

    private IEnumerator PerformAttack()
    {
        OnPrepareAttackStart();
        yield return new WaitForSeconds(CalcScaleDescending(basePrepareAttackTime));
        OnPrepareAttackEnd();
        Attack();
        OnEndAttackStart();
        yield return new WaitForSeconds(CalcScaleDescending(baseEndAttackTime));
        OnEndAttackEnd();
        StartCoroutine(ReloadWeapon(CalcScaleDescending(baseReloadTime)));
    }

    protected virtual bool CheckAttackConditions() //����� ��� ��������, �������� �� �����. ���������������� � �������-�����������
    {
        return true;
    }

    protected virtual void Attack() //����� ��� �����. ���������������� � �������-�����������
    {

    }

    protected virtual void OnPrepareAttackStart() //��������, ����������� ����� ������� ���������� �����
    {

    }

    protected virtual void OnPrepareAttackEnd() //��������, ����������� ����� ��������� ���������� �����
    {

    }

    protected virtual void OnEndAttackStart() //��������, ����������� ����� ������� ��������� �����
    {

    }

    protected virtual void OnEndAttackEnd() //��������, ����������� ����� ��������� ��������� �����
    {

    }

    private IEnumerator ReloadWeapon(float reloadTime)
    {
        yield return new WaitForSeconds(reloadTime);
        reloading = false;
    }
}
