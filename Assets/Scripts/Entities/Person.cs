using UnityEngine;
using System.Collections.Generic;
using System.Collections;

//������� ����� ���������
public class Person : Effectable, IDamagable
{
    bool alive = true;

    protected float destructionDelay = 1f;
    protected float maxHealth = 100f; //������������ ��������
    protected float health = 100f; //������� ��������
    protected float speed = 2f; //�������� ������������ ���������
    public List<BaseWeapon> weapons; //������ ������ ���������
    public Transform weaponObject;
    public List<string> targetTags; //������ ����� ��������� �����

    public bool isMoving = false;
    public Vector2 currentDirection;
    public Vector2 facingDirection;
    public Vector2 weaponDirection;

    Rigidbody2D rb = null;
    Animator anim = null; //��������� ��������

    public float getHP()
    {
        return health;
    }

    public bool isAlive()
    {
        return alive;
    }

    protected void Start()
    {
        facingDirection = Vector2.right;
        if (!TryGetComponent<Animator>(out anim)) //�������� ������ �� ��������� ���������
        {
            Debug.LogError(gameObject.name + ": Missing animator component");
        }
        if (!TryGetComponent<Rigidbody2D>(out rb))
        {
            Debug.LogError(gameObject.name + ": Missing rigidbody2d component");
        }
        else
        {
            rb.freezeRotation = true;
        }
        if (weaponObject == null)
        {
            Debug.LogError(gameObject.name + ": Missing weapon object");
        }
    }

    protected virtual void ChangeWeaponPosition()
    {
        weaponDirection = facingDirection;
        Vector2 normalizedDirection = weaponDirection.normalized;
        if (normalizedDirection == Vector2.zero)
        {
            normalizedDirection = Vector2.right;
        }
        Vector2 weaponPosition = normalizedDirection / 2;
        weaponObject.localPosition = weaponPosition;
        weaponObject.localRotation = Quaternion.Euler(0f, 0f, Vector2.SignedAngle(Vector2.right, normalizedDirection));
    }

    void Update()
    {
        if (alive)
        {
            // ���������� � ���������� �������� �� ��������
            UpdateEffectRemainingTime();

            //���������� ������� �������
            if (hasEffect(EffectNames.Burning))
            {
                TakeDamage(10f * Time.deltaTime, DamageType.Fire);
            }

            // ���������� ������� ������
            ChangeWeaponPosition();

            //�������� ���������
            if ((!hasEffect(EffectNames.Stun) && isMoving) || hasEffect(EffectNames.Shift)) 
                //���� ��� ��������� � �������� ���������, ��� �� ��� ���� ������ �����
            {
                Move();
            }
            else if (!isMoving)
            {
                rb.linearVelocity = new Vector2(0, 0);
            }
        }
    }

    public void TakeDamage(float damage, DamageType type) //���������� ������� TakeDamage �� IDamagable
    {
        if (alive)
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

            /*Debug.Log(gameObject.name +
                " - Reductions: shield=" + ShieldDamageReduction +
                ", expres=" + ExplosionResistDamageReduction +
                ", minigolem=" + MiniGolemDamageReduction +
                ", fireres=" + FireResistDamageReduction +
                ". Result damage: " + resultDamage);*/

            health -= resultDamage;
            if (health <= 0)
            {
                Die();
            }
        }
    }

    public float getSpeed()
    {
        float currentSpeed = speed;
        float speedCoeff = 1.0f;

        //���������� ������� ���������
        if (hasEffect(EffectNames.Freezing))
        {
            speedCoeff *= 0.5f + 0.25f * (getEffectCount(EffectNames.FrostResistance));
        }

        //���������� ������� ����� �����
        if (hasEffect(EffectNames.ShieldBlock))
        {
            speedCoeff *= 0.5f;
        }

        if (hasEffect(EffectNames.Shift))
        {
            speedCoeff = 5f;
        }

        //������ �������� ��������
        currentSpeed *= speedCoeff;

        return currentSpeed;
    }

    public void Move() //������� ������������ � �������� �����������
    {
        float currentSpeed = getSpeed();

        //������������ ���������
        if (!hasEffect(EffectNames.Shift))
        {
            Vector2 movement = currentDirection.normalized * currentSpeed;
            rb.linearVelocity = movement;

            //���������� ���������� ���������� ������������
            if (currentDirection != Vector2.zero)
                facingDirection = currentDirection;
        }
        else
        {
            Vector2 movement = facingDirection.normalized * currentSpeed;
            rb.linearVelocity = movement;
        }
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

    protected void Die() //������� ������ ���������
    {
        alive = false;
        if (anim) anim.SetBool("dead", true); //��������������� �������� ������
        weaponObject.gameObject.SetActive(false); //������ ������ ���������
        OnDeath(); //����� ������� ��� ������ ���������
        StartCoroutine(DestructionDelayCoroutine());
    }

    protected virtual void OnDeath() //�����, ���������� ������� ��� ������ ���������.
                          //���������������� � �������-����������� ��� �������������
    {
        Debug.Log(gameObject.name + ": dead");
    }

    private IEnumerator DestructionDelayCoroutine()
    {
        yield return new WaitForSeconds(destructionDelay);
        Destroy(gameObject);
    }
}
