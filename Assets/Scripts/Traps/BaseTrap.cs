using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// Класс, описывающий базовую функциональность ловушки.
/// </summary>
public class BaseTrap : MonoBehaviour
{
    [Tooltip("Список тегов объектов, которые ловушка будет отслеживать")]
    [SerializeField] protected List<string> targetTags;

    /// <summary>
    /// Активна ли ловушка.
    /// </summary>
    protected bool isActive = false;

    /// <summary>
    /// Список текущих целей, находящихся в зоне ловушки.
    /// </summary>
    List<Person> targets = new();

    /// <summary>
    /// Флаг, указывающий, что в данный момент происходит проверка целей.
    /// </summary>
    bool isChecking;

    /// <summary>
    /// Множество целей, добавляемых во время проверки.
    /// </summary>
    HashSet<Person> targetsOnAdd = new();

    /// <summary>
    /// Множество целей, удаляемых во время проверки.
    /// </summary>
    HashSet<Person> targetsOnDelete = new();

    /// <summary>
    /// Устанавливает список тегов целей, которые ловушка будет отслеживать.
    /// </summary>
    /// <param name="targetTags">Список тегов.</param>
    public void SetTargetTags(List<string> targetTags)
    {
        this.targetTags = targetTags;
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        GameObject obj = collision.gameObject;
        foreach (string targetTag in targetTags)
        {
            if (obj.CompareTag(targetTag))
            {
                Person target = obj.GetComponent<Person>();
                if (target != null)
                {
                    if (!isChecking) targets.Add(target);
                    else if (targetsOnDelete.Contains(target)) targetsOnDelete.Remove(target);
                    else targetsOnAdd.Add(target);
                    //Debug.Log("Target count: " + targets.Count);
                    if (isActive) OnEnter(target);
                }
                break;
            }
        }
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        GameObject obj = collision.gameObject;
        foreach (string targetTag in targetTags)
        {
            if (obj.CompareTag(targetTag))
            {
                Person target = obj.GetComponent<Person>();
                if (target != null)
                {
                    if (!isChecking) targets.Remove(target);
                    else if (targetsOnAdd.Contains(target)) targetsOnAdd.Remove(target);
                    else targetsOnDelete.Add(target);
                    Debug.Log("Target count: " + targets.Count);
                    if (isActive) OnExit(target);
                }
                break;
            }
        }
    }
    

    /// <summary>
    /// Метод, который применяет эффект OnStay к целям внутри нее.
    /// </summary>
    public void CheckTargetsInTrap()
    {
        if (isActive)
        {
            isChecking = true;
            foreach (Person target in targets)
            {
                OnStay(target);
            }
            isChecking = false;

            targets.AddRange(targetsOnAdd);
            targets.RemoveAll(t => targetsOnDelete.Contains(t));
            targetsOnAdd.Clear();
            targetsOnDelete.Clear();
        }
    }


    /// <summary>
    /// Метод, вызывающийся, когда цель входит в зону действия ловушки.
    /// Переопределяется в классах-наследниках.
    /// </summary>
    /// <param name="target">Цель</param>
    protected virtual void OnEnter(Person target)
    {

    }


    /// <summary>
    /// Метод, вызывающийся, когда цель находится в зоне действия ловушки.
    /// Переопределяется в классах-наследниках.
    /// </summary>
    /// <param name="target">Цель</param>
    protected virtual void OnStay(Person target)
    {

    }


    /// <summary>
    /// Метод, вызывающийся, когда цель выходит из зоны действия ловушки.
    /// Переопределяется в классах-наследниках.
    /// </summary>
    /// <param name="target">Цель</param>
    protected virtual void OnExit(Person target)
    {

    }


    /// <summary>
    /// Внутренний метод, меняющий состояние ловушки (активное/неактивное).
    /// </summary>
    /// <param name="state">Новое состояние ловушки.</param>
    void ChangeState(bool state)
    {
        isActive = state;
    }


    /// <summary>
    /// Метод, активирующий ловушку.
    /// </summary>
    public void Activate()
    {
        ChangeState(true);
        OnActivate();
    }

    /// <summary>
    /// Метод, вызываемый при активации ловушки.
    /// Переопределяется в классах-наследниках.
    /// </summary>
    protected virtual void OnActivate()
    {

    }

    /// <summary>
    /// Метод, деактивирующий ловушку.
    /// </summary>
    public void Deactivate()
    {
        ChangeState(false);
    }
}
