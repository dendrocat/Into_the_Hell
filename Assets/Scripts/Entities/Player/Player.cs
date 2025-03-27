using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class Player : Person, IDamagable
{
    public PlayerInventory inventory;
    public int ShiftCount = 3;

    protected override void ChangeWeaponPosition()
    {
        List<Collider2D> enemyColliders = Physics2D.OverlapCircleAll(transform.position, 10f).ToList<Collider2D>();
        if (enemyColliders.Count > 1)
        {
            Vector2 nearestDirection = new Vector2(15f, 0f);
            foreach(Collider2D enemyCollider in enemyColliders)
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

    public void Attack()
    {
        if (inventory.GetPlayerWeapon().reloading) return;
        inventory.GetPlayerWeapon().LaunchAttack();
    }

    public void AltAttack()
    {
        if (inventory.GetPlayerWeapon().altReloading) return;
        inventory.GetPlayerWeapon().LaunchAltAttack();
    }

    public void PerformShift()
    {
        if (!hasEffect(EffectNames.Shift) && (ShiftCount > 0) && isMoving)
        {
            StopAllCoroutines();

            StartCoroutine(ShiftCoroutine());
        }
    }

    public void UsePotion()
    {
        if (health < maxHealth)
        {
            if (inventory.UsePotion())
            {
                Potion potion = inventory.GetPotion();
                float heal = potion.getTotalHeal();
                health += heal;
                if (health > maxHealth)
                {
                    health = maxHealth;
                }
            }
        }
    }

    public new void TakeDamage(float damage, DamageType type)
    {
        if (isAlive())
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
            if (health <= 0)
            {
                Die();
            }
        }
    }

    private IEnumerator ShiftCoroutine()
    {
        AddEffect(EffectNames.Shift);
        ShiftCount--;

        yield return new WaitForSeconds(0.25f);

        RemoveEffect(EffectNames.Shift);

        StartCoroutine(Timer1Coroutine());
    }

    private IEnumerator Timer1Coroutine()
    {
        yield return new WaitForSeconds(1f);
        StartCoroutine(Timer2Coroutine());
    }

    private IEnumerator Timer2Coroutine()
    {
        yield return new WaitForSeconds(2f);
        if (ShiftCount < 3)
        {
            ShiftCount++;
            StartCoroutine(Timer2Coroutine());
        }
    }

    protected override void OnDeath()
    {
        Debug.Log("You died. Game over!");
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
