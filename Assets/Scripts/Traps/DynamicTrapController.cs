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
[RequireComponent(typeof(Animator))]
public class DynamicTrapController : MonoBehaviour
{
    BaseTrap trap;
    // [SerializeField] GameObject activeSprite;
    // [SerializeField] GameObject inactiveSprite;
    Animator _animator;

    [SerializeField] TrapType trapType;
    [SerializeField] float activatedPeriod; // время, в течение которого ловушка активна
    [SerializeField] float periodicalCheckPeriod; // время между проверками нахождения в ловушке
    [SerializeField] float idlePeriod; // время, в течение которого ловушка неактивна
    Coroutine periodicalCheckCoroutine;

    [Range(0f, 5f)]
    [SerializeField] float maxStartDelayTime;

    void Awake()
    {
        _animator = GetComponent<Animator>();
        trap = GetComponent<BaseTrap>();
        if (trap) trap.Deactivate();
    }

    IEnumerator Start()
    {
        if (trap)
        {
            yield return new WaitForSeconds(Random.Range(0f, maxStartDelayTime));
            StartCoroutine(IdlePeriod());
        }
    }

    IEnumerator ActivateTrap()
    {
        _animator.Play("TrapActivate");
        yield return new WaitForSeconds(_animator.GetCurrentAnimatorStateInfo(0).length);
        trap.Activate();
    }

    IEnumerator DeactivateTrap()
    {
        _animator.Play("TrapDeactivate");
        yield return new WaitForSeconds(_animator.GetCurrentAnimatorStateInfo(0).length);
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
        yield return StartCoroutine(ActivateTrap());
        yield return StartCoroutine(ActivatedPeriod());
    }

    /**
     * <summary>
     * Корутина, обрабатывающая период, когда ловушка активна.
     * </summary>
     * **/
    private IEnumerator ActivatedPeriod()
    {
        if (trapType == TrapType.Periodical)
            periodicalCheckCoroutine = StartCoroutine(PeriodicalCheck());
        yield return new WaitForSeconds(activatedPeriod);
        if (trapType == TrapType.Periodical)
            StopCoroutine(periodicalCheckCoroutine);
        yield return StartCoroutine(DeactivateTrap());
        yield return StartCoroutine(IdlePeriod());
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
