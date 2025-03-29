using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

/**
 * <summary>
 * ������� �����, ����������� �� ���������.
 * </summary>
 * **/
public class BaseAIController : MonoBehaviour
{
    public float attackRange = 2f;
    BaseEnemy NPC;
    AIPath aipath;
    AIDestinationSetter aiDestSetter;

    /**
     * <summary>
     * ������������� ��.
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
    }

    /**
     * <summary>
     * �����, ������������ ��������� ��. ���������� ������ ����.
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
        }
    }
}
