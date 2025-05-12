using System.Collections;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Контроллер полосы здоровья с плавной анимацией изменения заполнения.
/// </summary>
public class HealthBarController : MonoBehaviour
{
    [Header("Изображения здоровья")]
    [Tooltip("Изображение основной полосы здоровья")]
    [SerializeField] protected Image _healthImage;

    [Tooltip("Изображение сглаженной полосы здоровья")]
    [SerializeField] Image _smoothedHealthImage;

    [Header("Параметры анимации")]
    [Tooltip("Время сглаживания анимации в секундах")]
    [SerializeField] float _timeSmoothing;

    [Tooltip("Кривая анимации для сглаживания")]
    [SerializeField] AnimationCurve _curve;

    Coroutine _smoothHealth;
    Coroutine _smoothSmoothed;

    float _targetFill;

    /// <summary>
    /// Виртуальный метод для установки текста здоровья (переопределяется в наследниках).
    /// </summary>
    /// <param name="health">Текущее здоровье.</param>
    /// <param name="maxHealth">Максимальное здоровье.</param>
    protected virtual void SetTextHealth(float health, float maxHealth) { }

    /// <summary>
    /// Устанавливает здоровье без анимации.
    /// </summary>
    /// <param name="health">Текущее здоровье.</param>
    /// <param name="maxHealth">Максимальное здоровье.</param>
    public void SetHealth(float health, float maxHealth)
    {
        _targetFill = health / maxHealth;
        _healthImage.fillAmount = _targetFill;
        _smoothedHealthImage.fillAmount = _targetFill;
        SetTextHealth(health, maxHealth);
    }

    /// <summary>
    /// Устанавливает здоровье с плавной анимацией изменения.
    /// </summary>
    /// <param name="health">Текущее здоровье.</param>
    /// <param name="maxHealth">Максимальное здоровье.</param>
    public void SetHealthSmoothed(float health, float maxHealth)
    {
        _targetFill = health / maxHealth;
        SetTextHealth(health, maxHealth);
        SetHealthSmoothed(_targetFill);
    }

    /// <summary>
    /// Запускает анимацию плавного изменения заполнения полосы здоровья.
    /// </summary>
    /// <param name="fill">Целевое значение заполнения (от 0 до 1).</param>
    void SetHealthSmoothed(float fill)
    {
        _targetFill = fill;
        if (_smoothHealth == null)
            _smoothHealth = StartCoroutine(
                    SmoothHealth(_healthImage, _timeSmoothing / 10)
                );
        if (_smoothSmoothed != null)
            StopCoroutine(_smoothSmoothed);
        _smoothSmoothed = StartCoroutine(
            SmoothHealth(_smoothedHealthImage, _timeSmoothing)
        );
    }

    /// <summary>
    /// Корутина плавного изменения заполнения изображения по заданной кривой.
    /// </summary>
    /// <param name="image">Изображение, у которого изменяется заполнение.</param>
    /// <param name="duration">Длительность анимации в секундах.</param>
    /// <returns><see cref="IEnumerator"/> для корутины.</returns>
    IEnumerator SmoothHealth(Image image, float duration)
    {
        float start = image.fillAmount;
        float t = 0;
        while (t < 1)
        {
            var fill = _curve.Evaluate(t) * (_targetFill - start) + start;
            image.fillAmount = fill;

            t += Time.deltaTime / duration;

            yield return new WaitForSecondsRealtime(Time.deltaTime);
        }
        image.fillAmount = _targetFill;
        if (image == _healthImage)
        {
            _smoothHealth = null;
        }
        else _smoothSmoothed = null;
    }
}
