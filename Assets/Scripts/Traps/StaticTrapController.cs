using System.Collections;
using UnityEngine;

/**
 * <summary>
 * Класс-контроллер для статической (всегда активной) ловушки.
 * </summary>
 * **/
public class StaticTrapController : MonoBehaviour
{
    BaseTrap trap;
    [SerializeField] float periodicalCheckPeriod; // время между проверками нахождения в ловушке
    
    void Start()
    {
        trap = GetComponent<BaseTrap>();
        if (trap)
        {
            trap.Activate();
            StartCoroutine(PeriodicalCheckCoroutine());
        }
    }

    private IEnumerator PeriodicalCheckCoroutine()
    {
        yield return new WaitForSeconds(periodicalCheckPeriod);
        trap.CheckTargetsInTrap();
        StartCoroutine(PeriodicalCheckCoroutine());
    }
}
