using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : Person
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
