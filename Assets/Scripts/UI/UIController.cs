using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Контроллер UI, управляющий отображением здоровья, сдвига, зелий, босса, мини-карты,
/// а также текстовыми параметрами (стрелы, деньги) и изображением оружия.
/// </summary>
public class UIController : MonoBehaviour
{
    [Header("Сложные параметры")]
    [Tooltip("Контроллер полосы здоровья")]
    [SerializeField] HealthBarController _healthBar;
    /// <summary>
    /// Контроллер полосы здоровья
    /// </summary>
    public HealthBarController HealthBar => _healthBar;

    [Tooltip("Контроллер отображения рывков")]
    [SerializeField] ShiftController _shiftController;
    /// <summary>
    /// Контроллер отображения рывков
    /// </summary>
    public ShiftController ShiftController => _shiftController;

    [Tooltip("Контроллер отображения зелий")]
    [SerializeField] PotionController _potionController;
    /// <summary>
    /// Контроллер отображения зелий
    /// </summary>
    public PotionController PotionController => _potionController;

    [Tooltip("Контроллер полосы босса")]
    [SerializeField] BossBarController _bossBarController;
    /// <summary>
    /// Контроллер полосы босса
    /// </summary>
    public BossBarController BossBarController => _bossBarController;

    [Tooltip("Контроллер мини-карты")]
    [SerializeField] MiniMapUi _miniMapUI;
    /// <summary>
    /// Контроллер мини-карты
    /// </summary>
    public MiniMapUi MiniMapUI  => _miniMapUI;


    [Header("Текстовые параметры")]

    [Tooltip("Текстовое поле для отображения количества стрел")]
    [SerializeField] TextMeshProUGUI _arrows;

    [Tooltip("Текстовое поле для отображения количества денег")]
    [SerializeField] TextMeshProUGUI _money;


    [Header("Параметры оружия")]

    [Tooltip("Изображение текущего оружия")]
    [SerializeField] Image _weaponImage;

    [Tooltip("Список спрайтов оружия")]
    [SerializeField] List<Sprite> _weaponImages;

    Coroutine _smoothMoney;

    /// <summary>
    /// Устанавливает количество стрел в UI.
    /// </summary>
    /// <param name="arrows">Количество стрел.</param>
    public void SetArrows(int arrows)
    {
        _arrows.text = arrows.ToString();
    }

    /// <summary>
    /// Устанавливает количество денег в UI мгновенно.
    /// </summary>
    /// <param name="money">Количество денег.</param>
    public void SetMoney(int money)
    {
        _money.text = money.ToString();
    }

    /// <summary>
    /// Плавно изменяет отображаемое количество денег до заданного значения.
    /// </summary>
    /// <param name="money">Целевое количество денег.</param>
    public void SetMoneySmooth(int money)
    {
        if (_smoothMoney != null)
            StopCoroutine(_smoothMoney);
        _smoothMoney = StartCoroutine(SmoothMoney(money));
    }

    /// <summary>
    /// Корутина для плавного увеличения или уменьшения количества денег.
    /// </summary>
    /// <param name="money">Целевое количество денег.</param>
    /// <returns><see cref="IEnumerator"/> для корутины.</returns>
    IEnumerator SmoothMoney(int money)
    {
        int start = Convert.ToInt32(_money.text);
        int sign = Convert.ToInt32(Mathf.Sign(money - start));
        while (start != money)
        {
            start += sign;
            _money.text = start.ToString();
            yield return null;
        }
    }

    /// <summary>
    /// Меняет изображение оружия в UI в соответствии с <see cref="WeaponType">типом оружия</see>.
    /// </summary>
    /// <param name="type"><see cref="WeaponType">Тип оружия</see>.</param>
    public void ChangeWeaponImage(WeaponType type)
    {
        _weaponImage.sprite = _weaponImages[(int)type];
    }

}
