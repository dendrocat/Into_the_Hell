using System.Collections;
using UnityEngine;


/// <summary>
/// Класс-контроллер для статической (всегда активной) ловушки.
/// </summary>
public class StaticTrapController : MonoBehaviour
{
    BaseTrap trap;

    [Tooltip("Время между проверками нахождения целей в ловушке (в секундах)")]
    [SerializeField] float periodicalCheckPeriod;

    /// <summary>
    /// Устанавливает период между проверками целей в ловушке.
    /// </summary>
    /// <param name="periodicalCheckPeriod">Период в секундах.</param>
    public void SetPeriodicalCheckPeriod(float periodicalCheckPeriod)
    {
        this.periodicalCheckPeriod = periodicalCheckPeriod;
    }

    void Start()
    {
        trap = GetComponent<BaseTrap>();
        if (trap)
        {
            trap.Activate();
            StartCoroutine(PeriodicalCheckCoroutine());
        }
    }

    /// <summary>
    /// Корутина, периодически вызывающая проверку целей внутри ловушки.
    /// </summary>
    /// <returns><see cref="IEnumerator"/> для корутины</returns>
    private IEnumerator PeriodicalCheckCoroutine()
    {
        yield return new WaitForSeconds(periodicalCheckPeriod);
        trap.CheckTargetsInTrap();
        StartCoroutine(PeriodicalCheckCoroutine());
    }
}
