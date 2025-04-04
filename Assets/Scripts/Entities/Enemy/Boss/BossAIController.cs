using System.Collections;
using Pathfinding;
using UnityEngine;

/**
 * <summary>
 * Класс для описания базового ИИ босса.
 * </summary>
 * **/
public class BossAIController : BaseAIController
{
    protected Boss Boss;

    /// <summary>
    /// Показывает, атаковал ли босс в последние 2 секунды. 
    /// Используется для предотвращения одновременного запуска атак.
    /// </summary>
    bool attacked = false;

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
    void Update()
    {
        aipath.maxSpeed = Boss.getSpeed();
        Transform target = aiDestSetter.target;

        if (!attacked && (target != null))
        {
            if (Vector2.Distance(target.transform.position, Boss.transform.position) < attackRange)
            {
                Boss.Attack();
                attacked = true;
                StartCoroutine(UpdateAttackStatus(2f));
            }
        }
    }

    IEnumerator UpdateAttackStatus(float delay)
    {
        yield return new WaitForSeconds(delay);
        attacked = false;
    }
}
