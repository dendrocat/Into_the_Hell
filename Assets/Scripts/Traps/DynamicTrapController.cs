using System.Collections;
using UnityEngine;

/// <summary>
/// Класс-контроллер для динамической (имеющей периоды активности и бездействия) ловушки
/// </summary>
[RequireComponent(typeof(Animator))]
public class DynamicTrapController : MonoBehaviour
{
    BaseTrap trap;
    Animator _animator;

    [Tooltip("Тип ловушки, определяющий поведение проверки")]
    [SerializeField] TrapType trapType;

    [Tooltip("Время, в течение которого ловушка активна (в секундах)")]
    [SerializeField] float activatedPeriod;

    [Tooltip("Период между проверками целей в ловушке (в секундах)")]
    [SerializeField] float periodicalCheckPeriod;

    [Tooltip("Время, в течение которого ловушка неактивна (в секундах)")]
    [SerializeField] float idlePeriod;
    Coroutine periodicalCheckCoroutine;

    [Tooltip("Максимальная задержка перед стартом ловушки (в секундах)")]
    [SerializeField, Range(0f, 5f)] float maxStartDelayTime;

    /// <summary>
    /// Инициализация ловушки
    /// </summary>
    void Awake()
    {
        _animator = GetComponent<Animator>();
        trap = GetComponent<BaseTrap>();
        if (trap) trap.Deactivate();
    }


    /// <summary>
    /// Запускает ловушку с некоторой рандомной задержкой
    /// </summary>
    /// <returns><see cref="IEnumerator"/> для корутины</returns>
    IEnumerator Start()
    {
        if (trap)
        {
            yield return new WaitForSeconds(Random.Range(0f, maxStartDelayTime));
            StartCoroutine(IdlePeriod());
        }
    }

    /// <summary>
    /// Корутина активации ловушки с проигрыванием анимации.
    /// </summary>
    IEnumerator ActivateTrap()
    {
        _animator.Play("TrapActivate");
        yield return new WaitForSeconds(_animator.GetCurrentAnimatorStateInfo(0).length);
        trap.Activate();
    }

    /// <summary>
    /// Корутина деактивации ловушки с проигрыванием анимации.
    /// </summary>
    IEnumerator DeactivateTrap()
    {
        _animator.Play("TrapDeactivate");
        yield return new WaitForSeconds(_animator.GetCurrentAnimatorStateInfo(0).length);
        trap.Deactivate();
    }


    /// <summary>
    /// Корутина, обрабатывающая период, когда ловушка неактивна.
    /// </summary>
    private IEnumerator IdlePeriod()
    {
        yield return new WaitForSeconds(idlePeriod);
        yield return StartCoroutine(ActivateTrap());
        yield return StartCoroutine(ActivatedPeriod());
    }


    /// <summary>
    /// Корутина, обрабатывающая период, когда ловушка активна.
    /// </summary>
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


    /// <summary>
    /// Корутина, обрабатывающая периодическую проверку.
    /// </summary>
    private IEnumerator PeriodicalCheck()
    {
        yield return new WaitForSeconds(periodicalCheckPeriod);
        trap.CheckTargetsInTrap();
        periodicalCheckCoroutine = StartCoroutine(PeriodicalCheck());
    }
}
