using Pathfinding;
using UnityEngine;

public class BlazeAIController : BossAIController
{
    /// <summary>
    /// Цель атаки, от которой босс будет отходить.
    /// </summary>
    [Tooltip("Цель атаки, от которой босс будет отходить")]
    [SerializeField] Transform attackTarget;

    /// <summary>
    /// Максимальное расстояние, на котором босс будет держаться от цели.
    /// </summary>
    [Tooltip("Максимальное расстояние уклонения от цели")]
    [SerializeField] float evadeDistance = 5f;

    /// <summary>
    /// Инициализация компонентов и проверка их наличия.
    /// </summary>
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

    /// <summary>
    /// Пересчитывает позицию уклонения босса от цели с учётом препятствий.
    /// </summary>
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

    /// <summary>
    /// Обновление логики ИИ каждый кадр.
    /// </summary>
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

    /// <summary>
    /// Устанавливает цель атаки для босса.
    /// </summary>
    /// <param name="target">Трансформ цели.</param>
    public void SetAttackTarget(Transform target)
    {
        attackTarget = target;
    }
}
