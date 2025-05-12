using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// Класс, описывающий ИИ врагов дальнего боя.
/// </summary>
public class RangedAIController : BaseAIController
{
    
    /// <summary>
    /// Метод, определяющий поведение ИИ. Вызывается каждый кадр.
    /// </summary>
    private void Update()
    {
        aipath.maxSpeed = NPC.getSpeed();
        Transform target = aiDestSetter.target;

        if (target != null)
        {
            Vector2 raycastStartPos = NPC.weaponObject.position + NPC.weaponObject.rotation * Vector2.right * 0.5f;
            RaycastHit2D hit = Physics2D.Raycast(raycastStartPos, ((Vector2) target.position - raycastStartPos).normalized);
            if (hit.collider)
            {
                if (hit.collider.gameObject == target.gameObject)
                {
                    if (Vector2.Distance(target.transform.position, NPC.transform.position) < attackRange)
                    {
                        NPC.Attack();
                    }
                }
                else
                {
                    List<string> targetTags = NPC.weapon.GetTargetTags();
                    foreach (string targetTag in targetTags)
                    {
                        if (hit.collider.CompareTag(targetTag))
                        {
                            if (Vector2.Distance(hit.collider.transform.position, NPC.transform.position) < attackRange)
                            {
                                NPC.Attack();
                                break;
                            }
                        }
                    }
                }
            }
        }
    }


    /// <summary>
    /// Метод для дебага игры: отображает луч от дальнобойного оружия до игрока
    /// </summary>
    private void OnDrawGizmos()
    {
        if (aiDestSetter)
        {
            Transform target = aiDestSetter.target;
            if (target)
            {
                Vector2 raycastStartPos = NPC.weaponObject.position + NPC.weaponObject.rotation * Vector2.right * 0.5f;
                Vector2 raycastEndPos = (Vector3)raycastStartPos + NPC.weaponObject.rotation * Vector2.right * attackRange;
                Debug.DrawLine(raycastStartPos, target.position, new Color(1.0f, 0.5f, 0.5f));
                Debug.DrawLine(raycastStartPos, raycastEndPos, Color.red);
            }
        }
    }
}
