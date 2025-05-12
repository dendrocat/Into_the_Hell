using UnityEngine;

/// <summary>
/// Активатор подсказок, который запускает показ при изменении здоровья персонажа.
/// </summary>
[RequireComponent(typeof(Person))]
public class HintPersonActivator : HintActivator
{
    /// <summary>
    /// Переопределённый метод активации подсказки.
    /// Отписывается от события изменения здоровья, чтобы подсказка показывалась один раз.
    /// </summary>
    protected override void ActivateHint()
    {
        GetComponent<Person>().HealthChanged.RemoveListener(ActivateHint);
        base.ActivateHint();
    }

    void Awake()
    {
        GetComponent<Person>().HealthChanged.AddListener(ActivateHint);
    }
}
