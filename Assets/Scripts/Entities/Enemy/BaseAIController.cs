using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;
using UnityEditor;
using System.Collections;

/**
 * <summary>
 * Базовый класс, описывающий ИИ персонажа.
 * </summary>
 * **/
public class BaseAIController : MonoBehaviour
{
    [SerializeField] protected float attackRange = 2f;
    protected BaseEnemy NPC;
    protected AIPath aipath;
    protected AIDestinationSetter aiDestSetter;

    /**
     * <summary>
     * Инициализация ИИ.
     * </summary>
     * **/
    private void Start()
    {
        if (!gameObject.TryGetComponent<BaseEnemy>(out NPC))
        {
            Debug.LogError(gameObject.name + ": missing BaseEmeny component");
        }
        if (!gameObject.TryGetComponent<AIPath>(out aipath))
        {
            Debug.LogError(gameObject.name + ": missing AIPath component");
        }
        if (!gameObject.TryGetComponent<AIDestinationSetter>(out aiDestSetter))
        {
            Debug.LogError(gameObject.name + ": missing AIDestinationSetter component");
        }
        StartCoroutine(SetMovingFirstFrame());
    }

    IEnumerator SetMovingFirstFrame()
    {
        yield return null;
        NPC.setMoving(true);
    }

    /**
     * <summary>
     * Метод, определяющий поведение ИИ. Вызывается каждый кадр.
     * </summary>
     * **/
    private void Update()
    {
        aipath.maxSpeed = NPC.getSpeed();
        Transform target = aiDestSetter.target;

        if (target != null)
        {
            if (Vector2.Distance(target.transform.position, NPC.transform.position) < attackRange)
            {
                NPC.Attack();
            }
            else
            {
                List<RaycastHit2D> hits = Physics2D.RaycastAll(transform.position, NPC.weaponDirection, attackRange).ToList();
                foreach (RaycastHit2D hit in hits)
                {
                    if (hit.collider.gameObject.CompareTag("DestructibleWall"))
                    {
                        NPC.Attack();
                        break;
                    }
                }
            }
        }
    }
}
