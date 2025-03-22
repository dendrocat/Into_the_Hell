using UnityEngine;
using System.Collections.Generic;
using System.Collections;

//������� ����� ���������
public class Person : MonoBehaviour, IDamagable
{
    protected float maxHealth = 100f; //������������ ��������
    protected float health = 100f; //������� ��������
    protected float speed = 1f; //�������� ������������ ���������
    public List<BaseWeapon> weapons; //������ ������ ���������
    public Transform weaponObject;

    public bool isMoving = false;
    public Vector2 currentDirection;
    Animator anim; //��������� ��������

    void Start()
    {
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
        // ���������� ������� ������
        Vector2 normalizedDirection = currentDirection.normalized;
        if (normalizedDirection == Vector2.zero)
        {
            normalizedDirection = Vector2.right;
        }
        Vector2 weaponPosition = normalizedDirection / 2;
        weaponObject.localPosition = weaponPosition;
        weaponObject.localRotation = Quaternion.Euler(0f, 0f, Vector2.SignedAngle(Vector2.right, normalizedDirection));
        
        //�������� ���������
        if (isMoving)
        {
            Move();
        }
    }

    public void TakeDamage(float damage) //���������� ������� TakeDamage �� IDamagable
    {
        health -= damage;
        if (health <= 0)
        {
            Die();
        }
    }

    public void Move() //������� ������������ � �������� �����������
    {
        Vector2 movement = currentDirection.normalized * speed * Time.deltaTime;
        transform.position = transform.position + (Vector3) movement;
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
        anim.SetBool("dead", true); //��������������� �������� ������
        OnDeath(); //����� ������� ��� ������ ���������
    }

    public void OnDeath() //�����, ���������� ������� ��� ������ ���������.
                          //���������������� � �������-����������� ��� �������������
    {
        
    }
}
