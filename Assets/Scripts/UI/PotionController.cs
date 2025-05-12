using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Контроллер отображения количества зелий и анимации перезарядки.
/// </summary>
public class PotionController : MonoBehaviour
{
    [Tooltip("Текстовое поле для отображения количества зелий")]
    [SerializeField] TextMeshProUGUI _potions;

    [Tooltip("Изображение индикатора перезарядки")]
    [SerializeField] Image _imageReload;

    void Awake()
    {
        _imageReload.fillAmount = 0;
    }

    /// <summary>
    /// Устанавливает отображаемое количество зелий.
    /// </summary>
    /// <param name="potionCount">Количество зелий.</param>
    public void SetPotions(int potionCount)
    {
        _potions.text = potionCount.ToString();
    }

    /// <summary>
    /// Запускает анимацию перезарядки с заданной длительностью.
    /// </summary>
    /// <param name="reloadTime">Время перезарядки в секундах.</param>
    public void StartReload(float reloadTime)
    {
        _imageReload.fillAmount = 1;
        StartCoroutine(ReloadCoroutine(reloadTime));
    }

    /// <summary>
    /// Корутина анимации заполнения индикатора перезарядки.
    /// </summary>
    /// <param name="time">Длительность анимации в секундах.</param>
    /// <returns>IEnumerator для корутины.</returns>
    IEnumerator ReloadCoroutine(float time)
    {
        float t = 0;
        while (t < 1)
        {
            _imageReload.fillAmount = Mathf.Lerp(1, 0, t);
            t += Time.deltaTime / time;
            yield return null;
        }
        _imageReload.fillAmount = 0;
    }
}
