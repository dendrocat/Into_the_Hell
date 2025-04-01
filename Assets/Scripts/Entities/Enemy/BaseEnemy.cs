using System.Collections.Generic;
using System.Linq;
using Pathfinding;
using UnityEngine;

/**
 * <summary>
 * Базовый класс для врагов.
 * </summary>
 * **/
public class BaseEnemy : Person
{
    [SerializeField] float dropMoneyChance = 0.5f;
    [SerializeField] int dropMoneyCount = 5;
    Path path;
    Seeker seeker;
    AIPath aipath;
    AIDestinationSetter aiDestSetter;

    /**
     * <summary>
     * Инициализация врага
     * </summary>
     * **/
    void Start()
    {
        InitializePerson();
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
            path = seeker.StartPath(transform.position, aiDestSetter.target.transform.position);
        }
    }

    /**
     * <summary>
     * Обновляет скорость ИИ в соответствии с текущей скоростью персонажа
     * </summary>
     * **/
    void UpdateSpeed()
    {
        aipath.maxSpeed = getSpeed();
    }

    /**
     * <inheritdoc/>
     * **/
    protected override void ChangeWeaponPosition()
    {
        List<Collider2D> enemyColliders = Physics2D.OverlapCircleAll(transform.position, 10f).ToList<Collider2D>();
        if (enemyColliders.Count > 1)
        {
            Vector2 nearestDirection = new Vector2(15f, 0f);
            foreach (Collider2D enemyCollider in enemyColliders)
            {
                if (!enemyCollider.gameObject.CompareTag("Player")) continue;
                Vector2 dir = enemyCollider.gameObject.transform.position - transform.position;
                if (dir.magnitude < nearestDirection.magnitude)
                {
                    nearestDirection = dir;
                }
            }
            weaponDirection = nearestDirection.normalized;
        }
        else
        {
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

    /**
     * <summary>
     * Обновление состояния врага. Вызывается каждый кадр
     * </summary>
     * **/
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
                aipath.canMove = false;
            }
        }
    }

    /**
     * <inheritdoc/>
     * **/
    protected override void OnDeath()
    {
        aipath.canMove = false;
        aiDestSetter.target = null;
        DropMoney();
    }

    /**
     * <summary>
     * С некоторым шансом выдает деньги игроку.
     * </summary>
     * **/
    void DropMoney()
    {
        if (Random.Range(0.0f, 1.0f) < dropMoneyChance)
        {
            GameObject.FindGameObjectWithTag("Player").
                GetComponent<Player>().inventory.ModifyMoneyCount(dropMoneyCount);
        }
    }
}
