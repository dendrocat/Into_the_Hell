using System.Collections.Generic;
using System.Linq;
using Pathfinding;
using UnityEngine;


/// <summary>
/// Базовый класс для врагов.
/// </summary>
public class BaseEnemy : Person
{
    /// <summary>
    /// Шанс выпадения денег при смерти врага.
    /// </summary>
    [Tooltip("Шанс выпадения денег при смерти врага")]
    [SerializeField] float dropMoneyChance = 0.5f;

    /// <summary>
    /// Количество денег, которое может выпасть.
    /// </summary>
    [Tooltip("Количество денег, которое может выпасть")]
    [SerializeField] int dropMoneyCount = 5;

    /// <summary>
    /// Текущий <see cref="Path">путь</see> для навигации.
    /// </summary>
    protected Path path;

    /// <summary>
    /// Компонент <see cref="Seeker"/> для поиска пути.
    /// </summary>
    protected Seeker seeker;

    /// <summary>
    /// Компонент <see cref="AIPath"/> для управления движением по пути.
    /// </summary>
    protected AIPath aipath;

    /// <summary>
    /// Компонент <see cref="AIDestinationSetter"/> для установки цели движения.
    /// </summary>
    protected AIDestinationSetter aiDestSetter;


    /// <summary>
    /// Инициализация врага
    /// </summary>
    protected virtual void Start()
    {
        InitializePerson();
        Debug.Log(anim);
        if (!gameObject.TryGetComponent<Seeker>(out seeker))
        {
            Debug.LogError(gameObject.name + ": missing Seeker component");
        }
        if (!gameObject.TryGetComponent<AIPath>(out aipath))
        {
            Debug.LogError(gameObject.name + ": missing AIPath component");
        }
        if (!gameObject.TryGetComponent<AIDestinationSetter>(out aiDestSetter))
        {
            Debug.LogError(gameObject.name + ": missing AIDestinationSetter component");
        }
        if (seeker)
        {
            // Debug.Log(transform.position);
            // Debug.Log(aiDestSetter.target.transform.position);
            path = seeker.StartPath(transform.position, aiDestSetter.target.transform.position);
        }
    }


    /// <summary>
    /// Обновляет скорость ИИ в соответствии с текущей скоростью персонажа
    /// </summary>
    void UpdateSpeed()
    {
        aipath.maxSpeed = isMoving() ? getSpeed() : 0;
    }


    /// <inheritdoc />
    protected override void ChangeWeaponPosition()
    {
        List<Collider2D> enemyColliders = Physics2D.OverlapCircleAll(transform.position, 15f).ToList<Collider2D>();
        if (enemyColliders.Count > 1)
        {
            Vector2 nearestDirection = facingDirection * 15f;
            foreach (Collider2D enemyCollider in enemyColliders)
            {
                if (!enemyCollider.gameObject.CompareTag("Player")) continue;
                Vector2 dir = enemyCollider.gameObject.transform.position - transform.position;
                if (dir.magnitude < nearestDirection.magnitude)
                {
                    nearestDirection = dir;
                }
            }
            //Debug.Log("Nearest direction of " + gameObject.name + ": " + nearestDirection);
            weaponDirection = nearestDirection.normalized;
            facingDirection = weaponDirection;
        }
        else
        {
            //Debug.Log("Facing direction of " + gameObject.name + ": " + facingDirection);
            weaponDirection = facingDirection.normalized;
        }

        Vector2 normalizedDirection = weaponDirection;
        if (normalizedDirection == Vector2.zero)
        {
            normalizedDirection = Vector2.right;
        }
        Vector2 weaponPosition = normalizedDirection / 2;
        weaponObject.localPosition = weaponPosition;
        weaponObject.localRotation = Quaternion.Euler(0f, 0f, Vector2.SignedAngle(Vector2.right, normalizedDirection));
    }


    /// <summary>
    /// Обновление состояния врага. Вызывается каждый кадр
    /// </summary>
    void Update()
    {
        if (isAlive())
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

            // обновление скорости персонажа
            UpdateSpeed();

            //движение персонажа
            if (hasEffect(EffectNames.Stun))
            {
                setMoving(false);
            }
        }
    }


    /// <inheritdoc />
    protected override void OnDeath()
    {
        aipath.canMove = false;
        aiDestSetter.target = null;
        DropMoney();
    }


    /// <summary>
    /// С некоторым шансом выдает деньги игроку.
    /// </summary>
    void DropMoney()
    {
        if (Random.Range(0.0f, 1.0f) < dropMoneyChance)
        {
            GameObject.FindGameObjectWithTag("Player").
                GetComponent<Player>().inventory.ModifyMoneyCount(dropMoneyCount);
        }
    }
}
