using System.Collections;
using System.Data;
using UnityEngine;
using UnityEngine.UI;

public class HealthBarController : MonoBehaviour
{
    [Header("Images with health")]
    [SerializeField] Image _healthImage;
    [SerializeField] Image _smoothedHealthImage;

    [Header("Parameters for animation")]
    [SerializeField] float _timeSmoothing;
    [SerializeField] AnimationCurve _curve;

    Coroutine _smoothHealth;
    Coroutine _smoothSmoothed;

    float _targetFill;

    public void SetHealth(float fill)
    {
        _targetFill = fill;
        _healthImage.fillAmount = fill;
        _smoothedHealthImage.fillAmount = fill;
    }

    void Update()
    {
        if (_smoothedHealthImage.fillAmount != _targetFill)
        {
            SetHealthSmoothed(_targetFill);
        }
    }

    public void SetHealthSmoothed(float fill)
    {
        _targetFill = fill;
        if (_smoothHealth == null)
            _smoothHealth = StartCoroutine(
                SmoothHealth(_healthImage, _timeSmoothing / 10)
            );
        if (_smoothSmoothed == null)
        {
            _smoothSmoothed = StartCoroutine(
                SmoothHealth(_smoothedHealthImage, _timeSmoothing)
            );

        }
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
