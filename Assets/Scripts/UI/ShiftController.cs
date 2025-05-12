using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Контроллер для отображения и анимации перезарядки рывков.
/// Управляет списком иконок и плавным заполнением индикаторов.
/// </summary>
public class ShiftController : MonoBehaviour
{
    [Tooltip("Список изображений индикаторов перезарядки")]
    [SerializeField] List<Image> _images;

    int _shiftIndex;

    Coroutine shiftReload;

    void Awake()
    {
        for (int i = 0; i < transform.childCount; ++i)
        {
            _images.Add(transform.GetChild(i).Find("ReloadImage").GetComponent<Image>());
        }
        foreach (var image in _images)
            image.fillAmount = 0;
        _shiftIndex = _images.Count - 1;
    }

    /// <summary>
    /// Устанавливает количество доступных рывков и обновляет индикаторы.
    /// </summary>
    /// <param name="shiftCount">Количество доступных рывков.</param>
    public void SetShiftCount(int shiftCount)
    {
        var len = _images.Count;
        for (int i = len; i > shiftCount; --i)
        {
            _images[i - 1].fillAmount = 1;
        }
        _shiftIndex = shiftCount - 1;
        if (shiftReload != null)
        {
            StopCoroutine(shiftReload);
            shiftReload = null;
        }
    }

    /// <summary>
    /// Запускает плавную анимацию перезарядки следующего рывка.
    /// </summary>
    /// <param name="reloadTime">Время перезарядки в секундах.</param>
    public void StartShiftSmoothReload(float reloadTime)
    {
        _shiftIndex++;
        shiftReload = StartCoroutine(ShiftSmoothed(reloadTime));
    }

    /// <summary>
    /// Корутина плавного уменьшения заполнения индикатора рывка.
    /// </summary>
    /// <param name="time">Длительность анимации в секундах.</param>
    /// <returns>IEnumerator для корутины.</returns>
    IEnumerator ShiftSmoothed(float time)
    {
        float t = 0;
        time -= time / 1000;
        while (t < 1)
        {
            _images[_shiftIndex].fillAmount = Mathf.Lerp(1, 0, t);
            t += Time.deltaTime / time;
            yield return null;
        }
        _images[_shiftIndex].fillAmount = 0;
    }
}
