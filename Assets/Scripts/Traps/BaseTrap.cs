using System.Collections.Generic;
using UnityEngine;

/**
 * <summary>
 * Класс, описывающий базовую функциональность ловушки.
 * </summary>
 * **/
public class BaseTrap : MonoBehaviour
{
    [SerializeField] protected List<string> targetTags;
    protected bool isActive = false;
    bool checkTargetsInTrap = false;

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (isActive)
        {
            GameObject obj = collision.gameObject;
            foreach (string targetTag in targetTags)
            {
                if (obj.CompareTag(targetTag))
                {
                    Person? target = obj.GetComponent<Person>();
                    OnEnter(target);
                    break;
                }
            }
        }
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if (isActive)
        {
            GameObject obj = collision.gameObject;
            foreach (string targetTag in targetTags)
            {
                if (obj.CompareTag(targetTag))
                {
                    Person? target = obj.GetComponent<Person>();
                    OnExit(target);
                    break;
                }
            }
        }
    }
    /**
     * <summary>
     * Метод, который разрешает ловушке применить эффект OnStay к целям внутри нее. 
     * Должен вызываться через корутину в контроллере ловушки.
     * </summary>
     * **/
    public void CheckTargetsInTrap()
    {
        checkTargetsInTrap = true;
    }

    /**
     * <summary>
     * Метод, который применяет эффект OnStay к целям внутри ловушки.
     * </summary>
     * **/
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (checkTargetsInTrap)
        {
            if (isActive)
            {
                GameObject obj = collision.gameObject;
                foreach (string targetTag in targetTags)
                {
                    if (obj.CompareTag(targetTag))
                    {
                        Person? target = obj.GetComponent<Person>();
                        OnStay(target);
                        break;
                    }
                }
            }
        }
        checkTargetsInTrap = false;
    }

    /**
     * <summary>
     * Метод, вызывающийся, когда цель входит в зону действия ловушки.
     * Переопределяется в классах-наследниках.
     * </summary>
     * <param name="target">Цель</param>
     * **/
    protected virtual void OnEnter(Person target)
    {

    }

    /**
     * <summary>
     * Метод, вызывающийся, когда цель находится в зоне действия ловушки.
     * Переопределяется в классах-наследниках.
     * </summary>
     * <param name="target">Цель</param>
     * **/
    protected virtual void OnStay(Person target)
    {

    }

    /**
     * <summary>
     * Метод, вызывающийся, когда цель выходит из зоны действия ловушки.
     * Переопределяется в классах-наследниках.
     * </summary>
     * <param name="target">Цель</param>
     * **/
    protected virtual void OnExit(Person target)
    {

    }

    /**
     * <summary>
     * Внутренний метод, меняющий состояние ловушки (активное/неактивное).
     * </summary>
     * <param name="state">Новое состояние ловушки.</param>
     * **/
    void ChangeState(bool state)
    {
        isActive = state;
    }

    /**
     * <summary>
     * Метод, активирующий ловушку.
     * </summary>
     * **/
    public void Activate()
    {
        ChangeState(true);
    }

    /**
     * <summary>
     * Метод, деактивирующий ловушку.
     * </summary>
     * **/
    public void Deactivate()
    {
        ChangeState(false);
    }
}
