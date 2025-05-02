using System.Collections;
using System.Data;
using UnityEngine;
using UnityEngine.UI;

public class HealthBarController : MonoBehaviour
{
    [Header("Images with health")]
    [SerializeField] protected Image _healthImage;
    [SerializeField] Image _smoothedHealthImage;

    [Header("Parameters for animation")]
    [SerializeField] float _timeSmoothing;
    [SerializeField] AnimationCurve _curve;

    Coroutine _smoothHealth;
    Coroutine _smoothSmoothed;

    float _targetFill;

    protected virtual void SetTextHealth(float health, float maxHealth) { }

    public void SetHealth(float health, float maxHealth)
    {
        _targetFill = health / maxHealth;
        _healthImage.fillAmount = _targetFill;
        _smoothedHealthImage.fillAmount = _targetFill;
        SetTextHealth(health, maxHealth);
    }

    public void SetHealthSmoothed(float health, float maxHealth)
    {
        _targetFill = health / maxHealth;
        SetTextHealth(health, maxHealth);
        SetHealthSmoothed(_targetFill);
    }

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
