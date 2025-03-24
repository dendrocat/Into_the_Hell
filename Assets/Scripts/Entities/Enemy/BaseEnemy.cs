using System.Collections.Generic;
using System.Linq;
using Pathfinding;
using UnityEngine;

public class BaseEnemy : Person
{
    Path path;
    Seeker seeker;
    AIPath aipath;
    AIDestinationSetter aiDestSetter;

    void Start()
    {
        base.Start();
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

    protected override void ChangeWeaponPosition()
    {
        List<Collider2D> enemyColliders = Physics2D.OverlapCircleAll(transform.position, 10f).ToList<Collider2D>();
        if (enemyColliders.Count > 1)
        {
            Vector2 nearestDirection = new Vector2(15f, 0f);
            foreach (Collider2D enemyCollider in enemyColliders)
            {
                if (enemyCollider.gameObject.tag != "Player") continue;
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

    void Update()
    {
        if (isAlive())
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
            if (hasEffect(EffectNames.Stun))
            {
                aipath.canMove = false;
            }
        }
    }

    protected override void OnDeath()
    {
        aipath.canMove = false;
        aiDestSetter.target = null;
        Debug.Log(gameObject.name + ": dead");
    }
}
