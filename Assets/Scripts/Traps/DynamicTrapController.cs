using System.Collections;
using UnityEngine;

/**
 * <summary>
 * Тип ловушки
 * </summary>
 * **/
public enum TrapType
{
    NonPeriodical = 0, // без периодической проверки нахождения в ловушке
    Periodical = 1 // с периодической проверкой нахождения в ловушке
}

/**
 * <summary>
 * Класс-контроллер для динамической (имеющей периоды активности и бездействия) ловушки
 * </summary>
 * **/
public class DynamicTrapController : MonoBehaviour
{
    BaseTrap trap;
    [SerializeField] GameObject activeSprite;
    [SerializeField] GameObject inactiveSprite;

    [SerializeField] TrapType trapType;
    [SerializeField] float activatedPeriod; // время, в течение которого ловушка активна
    [SerializeField] float periodicalCheckPeriod; // время между проверками нахождения в ловушке
    [SerializeField] float idlePeriod; // время, в течение которого ловушка неактивна
    Coroutine periodicalCheckCoroutine;

    private void Start()
    {
        trap = GetComponent<BaseTrap>();
        if (trap)
        {
            DeactivateTrap();
            StartCoroutine(IdlePeriod());
        }
    }

    void ActivateTrap()
    {
        inactiveSprite.SetActive(false);
        activeSprite.SetActive(true);
        trap.Activate();
    }

    void DeactivateTrap()
    {
        inactiveSprite.SetActive(true);
        activeSprite.SetActive(false);
        trap.Deactivate();
    }

    /**
     * <summary>
     * Корутина, обрабатывающая период, когда ловушка неактивна.
     * </summary>
     * **/
    private IEnumerator IdlePeriod()
    {
        yield return new WaitForSeconds(idlePeriod);
        ActivateTrap();
        StartCoroutine(ActivatedPeriod());
    }

    /**
     * <summary>
     * Корутина, обрабатывающая период, когда ловушка активна.
     * </summary>
     * **/
    private IEnumerator ActivatedPeriod()
    {
        periodicalCheckCoroutine = StartCoroutine(PeriodicalCheck());
        yield return new WaitForSeconds(activatedPeriod);
        StopCoroutine(periodicalCheckCoroutine);
        DeactivateTrap();
        StartCoroutine(IdlePeriod());
    }

    /**
     * <summary>
     * Корутина, обрабатывающая периодическую проверку.
     * </summary>
     * **/
    private IEnumerator PeriodicalCheck()
    {
        yield return new WaitForSeconds(periodicalCheckPeriod);
        trap.CheckTargetsInTrap();
        periodicalCheckCoroutine = StartCoroutine(PeriodicalCheck());
    }
}
