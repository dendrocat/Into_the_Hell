using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

/**
 * <summary>
 * Класс игрока.<br/>
 * Важно! Вместо weapon используется inventory
 * </summary>
 * **/
[RequireComponent(typeof(PlayerAudio))]
public class Player : Person
{
    [HideInInspector]
    public UnityEvent<float> PotionUsed = new();
    [HideInInspector]
    public UnityEvent ShiftPerformed = new();
    [HideInInspector]
    public UnityEvent<float> ShiftReloadStarted = new();

    float baseMaxHealth = 100f;
    public PlayerInventory inventory;
    public int ShiftCount = 3;
    bool healReloading = false;

    Coroutine timer1Coroutine, timer2Coroutine;

    /**
     * <summary>
     * Обновляет здоровье в соответствии с уровнем брони.
     * </summary>
     * **/
    public void RecalculateHealth()
    {
        maxHealth = baseMaxHealth + inventory.GetPlayerArmor().getHealthBonus();
        health = maxHealth;
    }

    void Start()
    {
        InitializePerson();
        RecalculateHealth();
        _audioPlayer = GetComponent<PlayerAudio>();
        destructionDelay = 1f;
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
                if (enemyCollider.gameObject.tag != "Enemy") continue;
                if (!enemyCollider.gameObject.GetComponent<Person>().isAlive()) continue;
                Vector2 dir = enemyCollider.gameObject.transform.position - transform.position;
                if (dir.magnitude < nearestDirection.magnitude)
                {
                    nearestDirection = dir;
                }
            }
            weaponDirection = nearestDirection.normalized;

            if (nearestDirection == new Vector2(15f, 0f))
            {
                weaponDirection = facingDirection.normalized;
            }
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
     * Метод, вызывающий обычную атаку игрока.
     * </summary>
     * **/
    new public void Attack()
    {
        var weapon = inventory.GetPlayerWeapon();
        if (weapon.isReloading()) return;
        weapon.LaunchAttack();

        if (weapon is Sword) _audioPlayer.Play("Sword");
        else if (weapon is TwoHandedSword) _audioPlayer.Play("TwoHandedSword");
        else _audioPlayer.Play("Bow");
    }

    /**
     * <summary>
     * Метод, вызвающий альтернативную атаку игрока.
     * </summary>
     * **/
    public void AltAttack()
    {
        var weapon = inventory.GetPlayerWeapon();
        if (weapon.isReloadingAlt()) return;
        weapon.LaunchAltAttack();

        if (weapon is Sword) { if (isAlive()) _audioPlayer.Play("Shield"); }
        else if (weapon is TwoHandedSword) _audioPlayer.Play("TwoHandedSword");
        else _audioPlayer.Play("Bow");
    }

    /**
     * <summary>
     * Метод, запускающий рывок игрока.
     * </summary>
     * **/
    public void PerformShift()
    {
        if (!hasEffect(EffectNames.Shift) && (ShiftCount > 0) && isMoving())
        {
            if (timer1Coroutine != null) StopCoroutine(timer1Coroutine);
            if (timer2Coroutine != null) StopCoroutine(timer2Coroutine);

            StartCoroutine(ShiftCoroutine());
            _audioPlayer.Play("Dash");
            ShiftPerformed.Invoke();
        }
    }

    /**
     * <summary>
     * Метод, обрабатывающий использование зелья игроком.
     * </summary>
     * **/
    public void UsePotion()
    {
        if ((health < maxHealth) && !healReloading)
        {
            if (inventory.UsePotion())
            {
                healReloading = true;

                Potion potion = inventory.GetPotion();
                float heal = potion.getTotalHeal();
                health += heal;
                if (health > maxHealth)
                {
                    health = maxHealth;
                }

                float reloadTime = potion.getReloadTime();
                PotionUsed.Invoke(reloadTime);
                StartCoroutine(HealReload(reloadTime));

                HealthChanged.Invoke();
                _audioPlayer.Play("Heal");
            }
        }
    }

    /**
     * <summary>
     * Корутина, обрабатывающая перезарядку зелья
     * </summary>
     * **/
    private IEnumerator HealReload(float reloadTime)
    {
        Debug.Log(gameObject.name + ": started to reload heal (" + reloadTime + " sec)");
        yield return new WaitForSeconds(reloadTime);
        healReloading = false;
        Debug.Log(gameObject.name + ": heal reload complete");
    }

    /**
     * <summary>
     * Метод, показывающий, находится ли зелье на перезарядке
     * </summary>
     * <returns>Находится ли зелье на перезарядке</returns>
     * **/
    public bool isHealReloading()
    {
        return healReloading;
    }

    /**
     * <inheritdoc/>
     * **/
    public override void TakeDamage(float damage, DamageType type)
    {
        if (isAlive())
        {
            if (hasEffect(EffectNames.Shift) && (type == DamageType.None)) return; // не реагируем на урон, если есть рывок и игроку наносится обычный урон

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

            //снижение урона от брони
            float armorDamageReduction = 1f - inventory.GetPlayerArmor().getDamageReduction();
            resultDamage *= armorDamageReduction;

            Debug.Log(gameObject.name +
                " - Reductions: shield=" + ShieldDamageReduction +
                ", expres=" + ExplosionResistDamageReduction +
                ", minigolem=" + MiniGolemDamageReduction +
                ", fireres=" + FireResistDamageReduction +
                ", armor=" + armorDamageReduction +
                ". Result damage: " + resultDamage);

            health -= resultDamage;
            HealthChanged.Invoke();
            if (type != DamageType.Fire)
            {
                var weapon = inventory.GetPlayerWeapon() as Sword;
                if (weapon != null && weapon.altAttackIsActive())
                    _audioPlayer.Play("DamageShield");
                else _audioPlayer.Play("Damage");
            }
            if (health <= 0)
            {
                Die();
            }
        }
    }

    /**
     * <summary>
     * Корутина, обрабатывающая перезарядку рывка (родительская)
     * </summary>
     * **/
    private IEnumerator ShiftCoroutine()
    {
        AddEffect(EffectNames.Shift);
        ShiftCount--;

        yield return new WaitForSeconds(0.25f);

        RemoveEffect(EffectNames.Shift);

        timer1Coroutine = StartCoroutine(Timer1Coroutine());
    }

    /**
     * <summary>
     * Корутина, обрабатывающая перезарядку рывка (таймер 1)
     * </summary>
     * **/
    private IEnumerator Timer1Coroutine()
    {
        yield return new WaitForSeconds(1f);
        timer2Coroutine = StartCoroutine(Timer2Coroutine());
    }

    /**
     * <summary>
     * Корутина, обрабатывающая перезарядку рывка (таймер 2)
     * </summary>
     * **/
    private IEnumerator Timer2Coroutine()
    {
        if (ShiftCount < 3)
        {
            ShiftReloadStarted.Invoke(2f);
            yield return new WaitForSeconds(2f);

            ShiftCount++;
            timer2Coroutine = StartCoroutine(Timer2Coroutine());
        }
    }

    /**
     * <inheritdoc/>
     * **/
    protected override void OnDeath()
    {
        Debug.Log("You died. Game over!");
    }
}
