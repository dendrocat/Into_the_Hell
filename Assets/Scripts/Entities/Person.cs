using UnityEngine;
using System.Collections;
using UnityEngine.Events;

/**
 * <summary>
 * Базовый класс персонажа
 * </summary>
 * **/
public class Person : Effectable, IDamagable
{
    [HideInInspector]
    public UnityEvent OnHealthChanged = new();
    [HideInInspector]
    public UnityEvent<Person> OnDied = new();
    const float iceDriftDeceleration = 0.9f;

    bool alive = true;

    protected float destructionDelay = 1f;
    [SerializeField] protected float maxHealth = 100f; //максимальное здоровье
    public float MaxHealth => maxHealth;

    [SerializeField] protected float health = 100f; //текущее здоровье

    [SerializeField] protected float speed = 2f; //скорость передвижения персонажа
    public BaseWeapon weapon; //оружие персонажа
    public Transform weaponObject;

    [SerializeField] bool moving = false;
    public Vector2 currentDirection;
    public Vector2 facingDirection;
    public Vector2 weaponDirection;

    Rigidbody2D rb = null;
    Animator anim = null; //компонент аниматор

    /**
     * <summary>
     * Метод, возвращающий текущее здоровье персонажа
     * </summary>
     * <returns>float - текущий запас здоровья персонажа.</returns>
     * **/
    public float getHP()
    {
        return health;
    }

    /**
     * <summary>
     * Метод, определяющий, двигается ли персонаж.
     * </summary>
     * <returns>bool - двигается ли персонаж.</returns>
     * **/
    public bool isMoving()
    {
        return moving;
    }

    /**
     * <summary>
     * Метод, устанавливающий, двигается ли персонаж.
     * </summary>
     * <param name="moving">Двигается ли персонаж?</param>
     * **/
    public void setMoving(bool moving)
    {
        this.moving = moving;
    }

    /**
     * <summary>
     * Метод, устанавливающий, жив ли персонаж.
     * </summary>
     * <returns>bool - жив ли персонаж.</returns>
     * **/
    public bool isAlive()
    {
        return alive;
    }

    /**
     * <summary>
     * Инициализация персонажа.
     * </summary>
     * **/
    protected void InitializePerson()
    {
        facingDirection = Vector2.right;
        if (!TryGetComponent<Animator>(out anim)) //получаем ссылку на компонент аниматора
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

    void Start()
    {
        InitializePerson();
    }

    /**
     * <summary>
     * Изменяет позицию оружия персонажа. Переопределяется в классах-наследниках.
     * </summary>
     * **/
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

    /**
     * <summary>
     * Обработка действий персонажа.
     * </summary>
     * **/
    void Update()
    {
        if (alive)
        {
            // обновление и применение действий от эффектов
            UpdateEffectRemainingTime();

            //применение эффекта горения
            if (hasEffect(EffectNames.Burning))
            {
                TakeDamage(10f * Time.deltaTime, DamageType.Fire);
            }

            // обновление позиции оружия
            ChangeWeaponPosition();

            //движение персонажа
            if ((!hasEffect(EffectNames.Stun) && !hasEffect(EffectNames.HoleStun) && moving) || hasEffect(EffectNames.Shift))
            //если нет оглушения или падения в яму и персонаж двигается, или на нем есть эффект рывка
            {
                Move();
            }
            else if (hasEffect(EffectNames.Stun) || hasEffect(EffectNames.HoleStun) || !moving)
            {
                if (hasEffect(EffectNames.IceDrifting))
                {
                    rb.linearVelocity = Vector2.Lerp(rb.linearVelocity, Vector2.zero, iceDriftDeceleration * Time.deltaTime);
                }
                else rb.linearVelocity = Vector2.zero;
            }
        }
    }

    /**
     * <inheritdoc/>
     * **/
    public virtual void TakeDamage(float damage, DamageType type) //реализация функции TakeDamage из IDamagable
    {
        if (alive)
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

            /*Debug.Log(gameObject.name +
                " - Reductions: shield=" + ShieldDamageReduction +
                ", expres=" + ExplosionResistDamageReduction +
                ", minigolem=" + MiniGolemDamageReduction +
                ", fireres=" + FireResistDamageReduction +
                ". Result damage: " + resultDamage);*/

            health -= resultDamage;
            OnHealthChanged.Invoke();
            if (health <= 0)
            {
                Die();
            }
        }
    }

    /**
     * <summary>
     * Определяет текущую скорость персонажа с учетом эффектов.
     * </summary>
     * <returns>Текущая скорость персонажа.</returns>
     * **/
    public float getSpeed()
    {
        float currentSpeed = speed;
        float speedCoeff = 1.0f;

        //применение эффекта заморозки
        if (hasEffect(EffectNames.Freezing))
        {
            speedCoeff *= 0.5f + 0.25f * (getEffectCount(EffectNames.FrostResistance));
        }

        //применение эффекта блока щитом
        if (hasEffect(EffectNames.ShieldBlock))
        {
            speedCoeff *= 0.5f;
        }

        if (hasEffect(EffectNames.Shift))
        {
            speedCoeff = 5f;
        }

        //расчет итоговой скорости
        currentSpeed *= speedCoeff;

        return currentSpeed;
    }

    /**
     * <summary>
     * Обработка передвижения персонажа.
     * </summary>
     * **/
    public void Move() //функция передвижения в заданном направлении
    {
        float currentSpeed = getSpeed();

        //передвижение персонажа
        if (!hasEffect(EffectNames.Shift))
        {
            Vector2 movement = currentDirection.normalized * currentSpeed;
            if (hasEffect(EffectNames.IceDrifting))
            {
                rb.linearVelocity = Vector2.Lerp(rb.linearVelocity, movement, iceDriftDeceleration * Time.deltaTime);
            }
            else
            {
                rb.linearVelocity = movement;
            }


            //обновление последнего ненулевого передвижения
            if (currentDirection != Vector2.zero)
                facingDirection = currentDirection;
        }
        else
        {
            Vector2 movement = facingDirection.normalized * currentSpeed;
            rb.linearVelocity = movement;
        }
    }

    /**
     * <summary>
     * Метод, запускающий атаку персонажа.
     * </summary>
     * **/
    public void Attack() //функция атаки
    {
        if (!weapon.isReloading())
            weapon.LaunchAttack();
    }

    /**
     * <summary>
     * Метод, обрабатывающий смерть персонажа.
     * </summary>
     * **/
    protected void Die()
    {
        alive = false;
        if (anim) anim.SetBool("dead", true); //воспроизведение анимации смерти
        weaponObject.gameObject.SetActive(false); //скрыть оружие персонажа
        OnDeath(); //вызов событий при смерти персонажа
        OnDied.Invoke(this);
        StartCoroutine(DestructionDelayCoroutine());
    }

    /**
     * <summary>
     * Метод, вызывающий события при смерти персонажа. Переопределяется в классах-
     * наследниках при необходимости.
     * </summary>
     * **/
    protected virtual void OnDeath()
    {
        Debug.Log(gameObject.name + ": dead");
    }

    /**
     * <summary>
     * <see cref="Coroutine">Корутина</see>, обрабатывающая задержку уничтожения объекта.
     * </summary>
     * <returns><see cref="IEnumerator"/>, использующийся в корутинах.</returns>
     * **/
    private IEnumerator DestructionDelayCoroutine()
    {
        yield return new WaitForSeconds(destructionDelay);
        Destroy(gameObject);
    }
}
