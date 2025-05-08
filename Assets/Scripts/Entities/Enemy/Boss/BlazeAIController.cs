using Pathfinding;
using UnityEngine;

public class BlazeAIController : BossAIController
{
    /// <summary>
    /// ����, ������� ������ ��������� ������� ���.
    /// </summary>
    [SerializeField] Transform attackTarget;

    /// <summary>
    /// ���������, ������� ����� ��������� ������������ ������� ���.
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
        float maxEvadeDistance = evadeDistance;
        float curEvadeDistance = maxEvadeDistance;
        bool targetRaycasted = false;

        Vector2 destPos = attackTarget.transform.position + (Vector3)direction * curEvadeDistance;
        while (!targetRaycasted)
        {
            RaycastHit2D hit = Physics2D.Raycast(aiDestSetter.target.position, -direction, curEvadeDistance);
            if (hit)
            {
                if (hit.collider.gameObject.CompareTag("Player")) break;
            }
            curEvadeDistance -= 0.1f;
            aiDestSetter.target.position = attackTarget.transform.position + (Vector3)direction * curEvadeDistance;
            if (curEvadeDistance <= 1f) break;
        }
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

    public void SetAttackTarget(Transform target)
    {
        attackTarget = target;
    }
}
