using System.Collections;
using System.Collections.Generic;
using Ink.Runtime;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Класс, отвечающий за отображение диалогов: имени говорящего, текста фразы с эффектом печати и вариантов выбора.
/// </summary>
public class DialogDisplayer : MonoBehaviour
{
    Coroutine showPhraseCoroutine = null;

    /// <summary>
    /// Возвращает true, если в данный момент отображается фраза с эффектом печати.
    /// </summary>
    public bool IsShowingPhrase => showPhraseCoroutine != null;

    [Header("Ссылки на UI элементы для отображения")]

    /// <summary>
    /// Текстовое поле для отображения имени говорящего.
    /// </summary>
    [Tooltip("Текстовое поле для отображения имени говорящего.")]
    [SerializeField] TextMeshProUGUI _nameText;

    /// <summary>
    /// Текстовое поле для отображения самой фразы.
    /// </summary>
    [Tooltip("Текстовое поле для отображения самой фразы.")]
    [SerializeField] TextMeshProUGUI _phraseText;

    /// <summary>
    /// Компонент <see cref="ChoiceDisplayer"/> для отображения вариантов выбора.
    /// </summary>
    [Tooltip("Компонент для отображения вариантов выбора.")]
    [SerializeField] ChoiceDisplayer _choiceDisplayer;


    [Header("Параметры отображения")]

    /// <summary>
    /// Задержка между отображением символов для эффекте печати (в секундах).
    /// </summary>
    [Tooltip("Задержка между отображением символов для эффекте печати (в секундах).")]
    [SerializeField, Range(0.01f, 1f)] float _symbolDelayTime;

    /// <summary>Текущая фраза диалога</summary>
    private string _phrase;

    /// <summary>
    /// Устанавливает имя говорящего.
    /// </summary>
    /// <param name="name">Имя говорящего.</param>
    public void SetName(string name)
    {
        _nameText.text = name;
    }

    /// <summary>
    /// Запускает отображение фразы с эффектом постепенного появления текста.
    /// </summary>
    /// <param name="phrase">Текст фразы.</param>
    public void ShowPhrase(string phrase)
    {
        _phraseText.text = "";
        _phrase = phrase;
        showPhraseCoroutine = StartCoroutine(ShowPhraseCoroutine());
    }

    /// <summary>
    /// Корутина, которая по одному символу отображает фразу с задержкой.
    /// </summary>
    IEnumerator ShowPhraseCoroutine()
    {
        for (int i = 0; _phraseText.text.Length < _phrase.Length; ++i)
        {
            _phraseText.text += _phrase[i];
            yield return new WaitForSecondsRealtime(_symbolDelayTime);
        }
        StopCoroutine(showPhraseCoroutine);
        showPhraseCoroutine = null;
    }

    /// <summary>
    /// Немедленно отображает всю фразу целиком, прерывая эффект печати.
    /// </summary>
    public void ShowWholePhrase()
    {
        _phraseText.text = _phrase;
        StopCoroutine(showPhraseCoroutine);
        showPhraseCoroutine = null;
    }

    /// <summary>
    /// Обрабатывает список тегов, полученных из Ink, для управления отображением.
    /// </summary>
    /// <param name="tags">Список тегов.</param>
    public void HandleTags(List<string> tags)
    {
        foreach (var tag in tags)
        {
            var stag = tag.Split(":");
            HandleTag(stag[0].Trim(), stag[1].Trim());
        }

    }

    /// <summary>
    /// Обрабатывает отдельный тег и его значение.
    /// </summary>
    /// <param name="tag">Имя тега.</param>
    /// <param name="value">Значение тега.</param>
    void HandleTag(string tag, string value)
    {
        switch (tag)
        {
            case "name":
                SetName(value);
                break;
            default:
                Debug.LogWarning($"Неизвестный тег: {tag}");
                break;
        }
    }

    /// <summary>
    /// Создаёт кнопки выбора для списка вариантов.
    /// </summary>
    /// <param name="choices">Список вариантов выбора Ink.</param>
    /// <returns>Список созданных кнопок.</returns>
    public List<Button> CreateChoices(List<Choice> choices)
    {
        return _choiceDisplayer.CreateChoices(choices);
    }

    /// <summary>
    /// Удаляет все текущие варианты выбора из компонента <see cref="ChoiceDisplayer"/>.
    /// </summary>
    public void DestroyChoices()
    {
        _choiceDisplayer.DestroyChoices();
    }

}
