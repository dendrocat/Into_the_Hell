using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Контроллер финальной сцены (титры).
/// Обеспечивает прокрутку титров и перезагрузку игры после завершения.
/// </summary>
public class FinalController : MonoBehaviour
{
    [Header("Параметры титров")]
    /// <summary>
    /// Задержка перед началом прокрутки титров (в секундах).
    /// </summary>
    [Tooltip("Задержка перед началом прокрутки титров (в секундах)")]
    [SerializeField, Range(0f, 2f)] int delayStartCredits;

    /// <summary>
    /// Время прокрутки титров (в секундах).
    /// </summary>
    [Tooltip("Время прокрутки титров (в секундах)")]
    [SerializeField, Range(10, 200)] int creditsTime;


    [Header("UI компоненты титров")]
    /// <summary>
    /// Скроллбар для управления прокруткой титров.
    /// </summary>
    [Tooltip("Скроллбар для управления прокруткой титров")]
    [SerializeField] Scrollbar _scroll;

    /// <summary>
    /// Список элементов UI, высота которых должна соответствовать высоте экрана.
    /// </summary>
    [Tooltip("Список элементов UI, высота которых должна соответствовать высоте экрана")]
    [SerializeField] List<LayoutElement> fullHeightElements;

    /// <summary>
    /// Инициализация запуска прокрутки титров.
    /// </summary>
    void Awake()
    {
        fullHeightElements.ForEach(e => e.preferredHeight = Screen.height);

        _scroll.value = 1;
        StartCoroutine(LoadInitScene());
    }

    /// <summary>
    /// Корутина, обеспечивающая прокрутку титров и перезагрузку игры после завершения.
    /// </summary>
    IEnumerator LoadInitScene()
    {
        yield return new WaitForSecondsRealtime(delayStartCredits);
        float t = 1;
        while (t > 0f)
        {
            t -= Time.deltaTime / creditsTime;
            _scroll.value = t;
            yield return null;
        }
        yield return new WaitForSecondsRealtime(1f);
        GameManager.Instance.ReloadGame();
    }
}
