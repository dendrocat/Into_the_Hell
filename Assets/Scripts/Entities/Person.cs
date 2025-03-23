using UnityEngine;
using System.Collections.Generic;
using System.Collections;

//������� ����� ���������
public class Person : Effectable, IDamagable
{
    bool isAlive = true;

    protected float maxHealth = 100f; //������������ ��������
    protected float health = 100f; //������� ��������
    protected float speed = 2f; //�������� ������������ ���������
    public List<BaseWeapon> weapons; //������ ������ ���������
    public Transform weaponObject;

    public bool isMoving = false;
    public Vector2 currentDirection;
    public Vector2 facingDirection;
    Animator anim = null; //��������� ��������

    public float getHP()
    {
        return health;
    }

    void Start()
    {
        facingDirection = Vector2.right;
        if (!TryGetComponent<Animator>(out anim)) //�������� ������ �� ��������� ���������
        {
            Debug.LogError(gameObject.name + ": Missing animator component");
        }
        if (weaponObject == null)
        {
            Debug.LogError(gameObject.name + ": Missing weapon object");
        }
    }

    void Update()
    {
        if (isAlive)
        {
            // ���������� � ���������� �������� �� ��������
            UpdateEffectRemainingTime();

            //���������� ������� �������
            if (hasEffect(EffectNames.Burning))
            {
                TakeDamage(10f * Time.deltaTime, DamageType.Fire);
            }

            // ���������� ������� ������
            Vector2 normalizedDirection = facingDirection.normalized;
            if (normalizedDirection == Vector2.zero)
            {
                normalizedDirection = Vector2.right;
            }
            Vector2 weaponPosition = normalizedDirection / 2;
            weaponObject.localPosition = weaponPosition;
            weaponObject.localRotation = Quaternion.Euler(0f, 0f, Vector2.SignedAngle(Vector2.right, normalizedDirection));

            //�������� ���������
            if (!hasEffect(EffectNames.Stun) && isMoving) //���� ��� ��������� � �������� ���������
            {
                Move();
            }
        }
    }

    public void TakeDamage(float damage, DamageType type) //���������� ������� TakeDamage �� IDamagable
    {
        if (isAlive)
        {
            float resultDamage = damage; //����������� ����

            //������ �������� ����� �� ��������
            float ShieldDamageReduction = 1f - getEffectCount(EffectNames.ShieldBlock) * 0.1f;
            float ExplosionResistDamageReduction = 1f - getEffectCount(EffectNames.ExplosionResistance) * 0.5f;
            float MiniGolemDamageReduction = Mathf.Pow(0.2f, getEffectCount(EffectNames.MiniGolem));
            float FireResistDamageReduction = 1f - getEffectCount(EffectNames.FireResistance) * 0.5f;

            //�������� ����� �� ��������
            if (type == DamageType.None)
            {
                resultDamage *= ShieldDamageReduction;
                resultDamage *= MiniGolemDamageReduction;
            }
            else if (type == DamageType.Fire)
            {
                resultDamage *= FireResistDamageReduction;
            }
            else if (type == DamageType.Explosion)
            {
                resultDamage *= ExplosionResistDamageReduction;
            }

            Debug.Log(gameObject.name +
                " - Reductions: shield=" + ShieldDamageReduction +
                ", expres=" + ExplosionResistDamageReduction +
                ", minigolem=" + MiniGolemDamageReduction +
                ", fireres=" + FireResistDamageReduction +
                ". Result damage: " + resultDamage);

            health -= resultDamage;
            if (health <= 0)
            {
                Die();
            }
        }
    }

    public void Move() //������� ������������ � �������� �����������
    {
        float currentSpeed = speed;
        float speedCoeff = 1.0f;

        //���������� ������� ���������
        if (hasEffect(EffectNames.Freezing))
        {
            speedCoeff = 0.5f + 0.25f * (getEffectCount(EffectNames.FrostResistance));
        }

        //������ �������� ��������
        currentSpeed *= speedCoeff;

        //������������ ���������
        Vector2 movement = currentDirection.normalized * currentSpeed * Time.deltaTime;
        transform.position = transform.position + (Vector3) movement;

        //���������� ���������� ���������� ������������
        if (currentDirection != Vector2.zero)
            facingDirection = currentDirection;
    }

    public void Attack(int weaponIndex = -1) //������� �����. � ���������� ����� ������ ������
                                             //������, ������� ����� ���� (���� ��� �� �������, �� -1).
    {
        BaseWeapon currentWeapon = (weaponIndex != -1) ? weapons[weaponIndex] : null; //�������� ������ �� ������
        if ((currentWeapon != null) && currentWeapon.reloading) //���� ��������� ������ ��������������, �� �������
        {
            return;
        }

        if (currentWeapon == null) //���� ������ �� �������
        {
            for (int i = weapons.Count - 1; i >= 0; i--) //���������� ��� ������ � �����
            {
                if (!weapons[i].reloading) //���� ������ ������ � �����
                {
                    currentWeapon = weapons[i]; //�������� ���
                    break;
                }
            }
        }
        if (currentWeapon == null) return; //���� ��� ���������� ������, �� �������

        currentWeapon.LaunchAttack();
    }

    void Die() //������� ������ ���������
    {
        isAlive = false;
        if (anim) anim.SetBool("dead", true); //��������������� �������� ������
        weaponObject.gameObject.SetActive(false); //������ ������ ���������
        OnDeath(); //����� ������� ��� ������ ���������
    }

    public void OnDeath() //�����, ���������� ������� ��� ������ ���������.
                          //���������������� � �������-����������� ��� �������������
    {
        Debug.Log(gameObject.name + ": dead");
    }
}
