using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

// <summary>
/// Контроллер интро-сцены.
/// Обеспечивает последовательное отображение объектов и переключение на следующую сцену.
/// </summary>
[RequireComponent(typeof(AudioSource))]
public class IntroController : MonoBehaviour
{
    /// <summary>
    /// Структура для хранения информации об объекте интро и его длительности отображения.
    /// </summary>
    [Serializable]
    struct ObjectDuration
    {
        /// <summary>
        /// Игровой объект для отображения в интро.
        /// </summary>
        [Tooltip("Игровой объект для отображения в интро")]
        public GameObject gameObject;

        // <summary>
        /// Доля времени от общей длительности интро, в течение которой объект будет активен.
        /// </summary>
        [Tooltip("Доля времени от общей длительности интро (0..1)")]
        [Range(0f, 1f)] public float durationPart;
    }

    [Header("Настройки холста интро")]

    /// <summary>
    /// Изображение маски для плавного появления и исчезновения объектов.
    /// </summary>
    [Tooltip("Изображение маски для плавного появления и исчезновения объектов")]
    [SerializeField] Image _mask;

    /// <summary>
    /// Список объектов интро с указанием длительности их отображения.
    /// </summary>
    [Tooltip("Список объектов интро с указанием длительности их отображения")]
    [SerializeField] List<ObjectDuration> _introObjects;


    [Header("Параметры интро")]

    /// <summary>
    /// Задержка между отображением объектов интро (в секундах).
    /// </summary>
    [Tooltip("Задержка между отображением объектов интро (в секундах)")]
    [SerializeField] float delayBetweenObjects;

    /// <summary>
    /// Запуск корутины отображения интро.
    /// </summary>
    void Awake()
    {
        StartCoroutine(ShowIntro(GetComponent<AudioSource>().clip.length));
    }

    /// <summary>
    /// Корутина изменения состояния маски (плавное появление и исчезновение).
    /// </summary>
    /// <param name="duration">Длительность анимации (в секундах).</param>
    IEnumerator ChangeMaskState(float duration)
    {
        var color = _mask.color;
        duration /= 2;
        float t = 1f;
        while (t > 0)
        {
            t -= Time.deltaTime / duration;
            color.a = t;
            _mask.color = color;
            yield return null;
        }
        color.a = 0;
        t = 0;
        while (t < 1f)
        {
            t += Time.deltaTime / duration;
            color.a = t;
            _mask.color = color;
            yield return null;
        }
        color.a = 1;
    }

    /// <summary>
    /// Корутина отображения интро и переключения на следующую сцену.
    /// </summary>
    /// <param name="duration">Общая длительность интро (в секундах).</param>
    IEnumerator ShowIntro(float duration)
    {
        yield return null;
        duration -= delayBetweenObjects * (_introObjects.Count - 1);
        foreach (var item in _introObjects)
        {
            item.gameObject.SetActive(true);
            var itemDuration = duration * item.durationPart;
            StartCoroutine(ChangeMaskState(itemDuration));
            yield return new WaitForSeconds(itemDuration + delayBetweenObjects);
            item.gameObject.SetActive(false);
        }
        SceneManager.LoadScene(1);
    }
}
