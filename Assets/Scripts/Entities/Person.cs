using UnityEngine;
using System.Collections.Generic;
using System.Collections;

//Базовый класс персонажа
public class Person : Effectable, IDamagable
{
    bool isAlive = true;

    protected float maxHealth = 100f; //максимальное здоровье
    protected float health = 100f; //текущее здоровье
    protected float speed = 2f; //скорость передвижения персонажа
    public List<BaseWeapon> weapons; //список оружий персонажа
    public Transform weaponObject;

    public bool isMoving = false;
    public Vector2 currentDirection;
    public Vector2 facingDirection;
    Animator anim = null; //компонент аниматор

    public float getHP()
    {
        return health;
    }

    void Start()
    {
        facingDirection = Vector2.right;
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
        if (isAlive)
        {
            // обновление и применение действий от эффектов
            UpdateEffectRemainingTime();

            //применение эффекта горения
            if (hasEffect(EffectNames.Burning))
            {
                TakeDamage(10f * Time.deltaTime, DamageType.Fire);
            }

            // обновление позиции оружия
            Vector2 normalizedDirection = facingDirection.normalized;
            if (normalizedDirection == Vector2.zero)
            {
                normalizedDirection = Vector2.right;
            }
            Vector2 weaponPosition = normalizedDirection / 2;
            weaponObject.localPosition = weaponPosition;
            weaponObject.localRotation = Quaternion.Euler(0f, 0f, Vector2.SignedAngle(Vector2.right, normalizedDirection));

            //движение персонажа
            if (!hasEffect(EffectNames.Stun) && isMoving) //если нет оглушения и персонаж двигается
            {
                Move();
            }
        }
    }

    public void TakeDamage(float damage, DamageType type) //реализация функции TakeDamage из IDamagable
    {
        if (isAlive)
        {
            float resultDamage = damage; //изначальный урон

            //расчет снижения урона от эффектов
            float ShieldDamageReduction = 1f - getEffectCount(EffectNames.ShieldBlock) * 0.1f;
            float ExplosionResistDamageReduction = 1f - getEffectCount(EffectNames.ExplosionResistance) * 0.5f;
            float MiniGolemDamageReduction = Mathf.Pow(0.2f, getEffectCount(EffectNames.MiniGolem));
            float FireResistDamageReduction = 1f - getEffectCount(EffectNames.FireResistance) * 0.5f;

            //снижение урона от эффектов
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

    public void Move() //функция передвижения в заданном направлении
    {
        float currentSpeed = speed;
        float speedCoeff = 1.0f;

        //применение эффекта заморозки
        if (hasEffect(EffectNames.Freezing))
        {
            speedCoeff = 0.5f + 0.25f * (getEffectCount(EffectNames.FrostResistance));
        }

        //расчет итоговой скорости
        currentSpeed *= speedCoeff;

        //передвижение персонажа
        Vector2 movement = currentDirection.normalized * currentSpeed * Time.deltaTime;
        transform.position = transform.position + (Vector3) movement;

        //обновление последнего ненулевого передвижения
        if (currentDirection != Vector2.zero)
            facingDirection = currentDirection;
    }

    public void Attack(int weaponIndex = -1) //функция атаки. В аргументах может стоять индекс
                                             //оружия, которым нужно бить (если оно не указано, то -1).
    {
        BaseWeapon currentWeapon = (weaponIndex != -1) ? weapons[weaponIndex] : null; //получаем ссылку на оружие
        if ((currentWeapon != null) && currentWeapon.reloading) //если выбранное оружие перезаряжается, то выходим
        {
            return;
        }

        if (currentWeapon == null) //если оружие не указано
        {
            for (int i = weapons.Count - 1; i >= 0; i--) //перебираем все оружия с конца
            {
                if (!weapons[i].reloading) //если оружие готово к атаке
                {
                    currentWeapon = weapons[i]; //выбираем его
                    break;
                }
            }
        }
        if (currentWeapon == null) return; //если нет доступного оружия, то выходим

        currentWeapon.LaunchAttack();
    }

    void Die() //функция смерти персонажа
    {
        isAlive = false;
        if (anim) anim.SetBool("dead", true); //воспроизведение анимации смерти
        weaponObject.gameObject.SetActive(false); //скрыть оружие персонажа
        OnDeath(); //вызов событий при смерти персонажа
    }

    public void OnDeath() //Метод, вызывающий события при смерти персонажа.
                          //Переопределяется в классах-наследниках при необходимости
    {
        Debug.Log(gameObject.name + ": dead");
    }
}
