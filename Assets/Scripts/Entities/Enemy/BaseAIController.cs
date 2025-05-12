using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;
using System.Collections;

///<summary>
/// Базовый класс, описывающий ИИ персонажа.
/// </summary>
public class BaseAIController : MonoBehaviour
{
    /// <summary>
    /// Дистанция атаки, на которой ИИ начинает атаковать цель.
    /// </summary>
    [Tooltip("Дистанция атаки, на которой ИИ начинает атаковать цель")]
    [SerializeField] protected float attackRange = 2f;

    /// <summary>
    /// Ссылка на компонент <see cref="BaseEnemy">врага</see>, которым управляет ИИ.
    /// </summary>
    protected BaseEnemy NPC;

    /// <summary>
    /// Компонент <see cref="AIPath"/> для управления движением по пути.
    /// </summary>
    protected AIPath aipath;

    /// <summary>
    /// Компонент <see cref="AIDestinationSetter"/> для установки цели движения.
    /// </summary>
    protected AIDestinationSetter aiDestSetter;


    /// <summary>
    /// Инициализация ИИ.
    /// </summary>
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

    /// <summary>
    /// Корутина, устанавливающая состояние движения NPC после первого кадра.
    /// </summary>
    IEnumerator SetMovingFirstFrame()
    {
        yield return null;
        NPC.setMoving(true);
    }


    /// <summary>
    /// Метод, определяющий поведение ИИ. Вызывается каждый кадр.
    /// </summary>
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
