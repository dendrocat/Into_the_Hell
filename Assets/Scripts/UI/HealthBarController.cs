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

    public void SetHealth(float fill)
    {
        _healthImage.fillAmount = fill;
        _smoothedHealthImage.fillAmount = fill;
    }

    public void SetHealthSmoothed(float fill)
    {
        if (_smoothHealth != null)
        {
            StopCoroutine(_smoothHealth);
            _smoothHealth = null;
        }
        if (_smoothSmoothed != null)
        {
            StopCoroutine(_smoothSmoothed);
            _smoothSmoothed = null;
        }
        _smoothHealth = StartCoroutine(
            SmoothHealth(_healthImage, fill, 0.2f)
        );
    }

    IEnumerator SmoothHealth(Image image, float targetFill, float duration)
    {
        float start = image.fillAmount;
        float t = 0;
        while (t < 1)
        {
            var fill = _curve.Evaluate(t) * (targetFill - start) + start;
            image.fillAmount = fill;

            t += Time.deltaTime / duration;

            yield return new WaitForSecondsRealtime(Time.deltaTime);
        }
        image.fillAmount = targetFill;
        if (_smoothSmoothed == null)
        {
            _smoothSmoothed = StartCoroutine(
                SmoothHealth(_smoothedHealthImage, targetFill, _timeSmoothing)
            );
        }
    }
}
