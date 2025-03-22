using UnityEngine;
using System.Collections.Generic;
using System.Collections;

//Ѕазовый класс персонажа
public class Person : MonoBehaviour, IDamagable
{
    protected float maxHealth = 100f; //максимальное здоровье
    protected float health = 100f; //текущее здоровье
    protected float speed = 1f; //скорость передвижени€ персонажа
    public List<BaseWeapon> weapons; //список оружий персонажа
    public Transform weaponObject;

    public bool isMoving = false;
    public Vector2 currentDirection;
    Animator anim; //компонент аниматор

    void Start()
    {
        if (!TryGetComponent<Animator>(out anim)) //получаем ссылку на компонент аниматора
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
        // обновление позиции оружи€
        Vector2 normalizedDirection = currentDirection.normalized;
        if (normalizedDirection == Vector2.zero)
        {
            normalizedDirection = Vector2.right;
        }
        Vector2 weaponPosition = normalizedDirection / 2;
        weaponObject.localPosition = weaponPosition;
        weaponObject.localRotation = Quaternion.Euler(0f, 0f, Vector2.SignedAngle(Vector2.right, normalizedDirection));
        
        //движение персонажа
        if (isMoving)
        {
            Move();
        }
    }

    public void TakeDamage(float damage) //реализаци€ функции TakeDamage из IDamagable
    {
        health -= damage;
        if (health <= 0)
        {
            Die();
        }
    }

    public void Move() //функци€ передвижени€ в заданном направлении
    {
        Vector2 movement = currentDirection.normalized * speed * Time.deltaTime;
        transform.position = transform.position + (Vector3) movement;
    }

    public void Attack(int weaponIndex = -1) //функци€ атаки. ¬ аргументах может сто€ть индекс
                                             //оружи€, которым нужно бить (если оно не указано, то -1).
    {
        BaseWeapon currentWeapon = (weaponIndex != -1) ? weapons[weaponIndex] : null; //получаем ссылку на оружие
        if ((currentWeapon != null) && currentWeapon.reloading) //если выбранное оружие перезар€жаетс€, то выходим
        {
            return;
        }

        if (currentWeapon == null) //если оружие не указано
        {
            for (int i = weapons.Count - 1; i >= 0; i--) //перебираем все оружи€ с конца
            {
                if (!weapons[i].reloading) //если оружие готово к атаке
                {
                    currentWeapon = weapons[i]; //выбираем его
                    break;
                }
            }
        }
        if (currentWeapon == null) return; //если нет доступного оружи€, то выходим

        currentWeapon.LaunchAttack();
    }

    void Die() //функци€ смерти персонажа
    {
        anim.SetBool("dead", true); //воспроизведение анимации смерти
        OnDeath(); //вызов событий при смерти персонажа
    }

    public void OnDeath() //ћетод, вызывающий событи€ при смерти персонажа.
                          //ѕереопредел€етс€ в классах-наследниках при необходимости
    {
        
    }
}
