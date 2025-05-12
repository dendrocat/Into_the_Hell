
using System;
using TMPro;
using UnityEngine;

/// <summary>
/// Контроллер полосы здоровья босса, расширяющий базовый контроллер здоровья.
/// Управляет отображением имени босса, текста здоровья и изменением цвета в зависимости от эффекта.
/// </summary>
public class BossHealthBarController : HealthBarController
{
    [Header("Параметры текста")]
    [Tooltip("Текстовое поле для отображения имени босса")]
    [SerializeField] TextMeshProUGUI _name;
    [Tooltip("Текстовое поле для отображения здоровья босса")]
    [SerializeField] TextMeshProUGUI _textHealth;

    [Header("Параметры цвета")]
    [Tooltip("Цвет полосы здоровья при эффекте MiniGolem")]
    [SerializeField] Color _miniGolemColor;

    Color _startColor;

    Func<bool> predicateEffect;

    void Awake()
    {
        _startColor = _healthImage.color;
    }

    /// <summary>
    /// Устанавливает имя босса в UI.
    /// </summary>
    /// <param name="name">Имя босса.</param>
    public void SetBossName(string name)
    {
        _name.text = name;
    }

    /// <summary>
    /// Переопределённый метод для установки текста здоровья.
    /// Отображает текущее и максимальное здоровье в формате "текущее / максимальное".
    /// </summary>
    /// <param name="health">Текущее здоровье.</param>
    /// <param name="maxHealth">Максимальное здоровье.</param>
    protected override void SetTextHealth(float health, float maxHealth)
    {
        _textHealth.text = $"{Mathf.Ceil(health)} / {maxHealth}";
    }

    /// <summary>
    /// Устанавливает функцию-предикат, определяющую, когда менять цвет полосы здоровья.
    /// </summary>
    /// <param name="func">Функция, возвращающая <see langword="true"/>, если цвет нужно менять.</param>
    public void SetPredicateColorChange(Func<bool> func)
    {
        predicateEffect = func;
    }

    void Update()
    {
        if (predicateEffect())
            _healthImage.color = _miniGolemColor;
        else _healthImage.color = _startColor;
    }
}
