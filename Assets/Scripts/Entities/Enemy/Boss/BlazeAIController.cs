using Pathfinding;
using UnityEngine;

public class BlazeAIController : BossAIController
{
    /// <summary>
    /// Цель, которую должно атаковать Исчадие ада.
    /// </summary>
    [SerializeField] Transform attackTarget;

    /// <summary>
    /// Дистанция, которую будет стараться поддерживать Исчадие ада.
    /// </summary>
    [SerializeField] float evadeDistance = 5f;

    private void Start()
    {
        if (!gameObject.TryGetComponent<Boss>(out Boss))
        {
            Debug.LogError(gameObject.name + ": missing Boss component");
        }
        if (!gameObject.TryGetComponent<AIPath>(out aipath))
        {
            Debug.LogError(gameObject.name + ": missing AIPath component");
        }
        if (!gameObject.TryGetComponent<AIDestinationSetter>(out aiDestSetter))
        {
            Debug.LogError(gameObject.name + ": missing AIDestinationSetter component");
        }
    }

    void RecalculateDestination()
    {
        Vector2 direction = (transform.position - attackTarget.transform.position).normalized;
        aiDestSetter.target.position = attackTarget.transform.position + (Vector3) direction * evadeDistance;
    }

    void Update()
    {
        aipath.maxSpeed = Boss.getSpeed();
        if (Boss.isAlive())
            RecalculateDestination();

        if (!attacked && (attackTarget != null))
        {
            if (Vector2.Distance(attackTarget.transform.position, Boss.transform.position) < attackRange)
            {
                Boss.Attack();
                attacked = true;
                StartCoroutine(UpdateAttackStatus(attackDelay));
            }
        }
    }
}
