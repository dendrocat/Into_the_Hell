using System.Collections;
using UnityEngine;

/// <summary>
/// Абстрактный класс-активатор подсказок, инициализирующий и показывающий подсказки с задержкой.
/// Требует наличия компонента <see cref="ActionRuntimeFinder"/>.
/// </summary>
[RequireComponent(typeof(ActionRuntimeFinder))]
public abstract class HintActivator : MonoBehaviour
{
    [Tooltip("Ссылка на объект подсказок")]
    [SerializeField] Hint _hint;

    [Tooltip("Задержка перед активацией подсказок (в секундах)")]
    [SerializeField] float _delayActivation = .1f;

    ActionRuntimeFinder _finder;

    /// <summary>
    /// Корутина с задержкой, инициализирующая подсказки и вызывающая их отображение.
    /// После отображения уничтожает себя и компонент <see cref="ActionRuntimeFinder"/>.
    /// </summary>
    void Start()
    {
        _finder = GetComponent<ActionRuntimeFinder>();
    }

    IEnumerator DelayActivation()
    {
        _hint.InitHint(_finder);
        yield return new WaitForSeconds(_delayActivation);
        HintManager.Instance.ShowHints(_hint.Hints);
        Destroy(this);
        Destroy(_finder);
    }

    /// <summary>
    /// Запускает процесс активации подсказок с задержкой.
    /// Может быть переопределён в наследниках для кастомного поведения.
    /// </summary>
    protected virtual void ActivateHint()
    {
        StartCoroutine(DelayActivation());
    }
}
