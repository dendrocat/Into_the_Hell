using System.Collections.Generic;
using Ink.Runtime;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Класс, отвечающий за отображение вариантов выбора в диалогах Ink.
/// </summary>
/// <remarks>
/// Создаёт кнопки для каждого варианта выбора и управляет их удалением.
/// </remarks>
public class ChoiceDisplayer : MonoBehaviour
{
    /// <summary>
    /// Префаб кнопки для варианта выбора.
    /// </summary>
    [Tooltip("Префаб кнопки для варианта выбора.")]
    [SerializeField] GameObject _buttonPrefab;

    // <summary>
    /// Инициализация. Удаляет все существующие варианты выбора при старте.
    /// </summary>
    void Start()
    {
        DestroyChoices();
    }

    /// <summary>
    /// Создаёт кнопки для каждого варианта выбора.
    /// </summary>
    /// <param name="choices">Список вариантов выбора из Ink.</param>
    /// <returns>Список созданных кнопок.</returns>
    public List<Button> CreateChoices(List<Choice> choices)
    {
        var buttons = new List<Button>();
        foreach (var choice in choices)
        {
            var button = Instantiate(_buttonPrefab, transform);
            button.GetComponentInChildren<TextMeshProUGUI>().text = choice.text;
            buttons.Add(button.GetComponent<Button>());
        }
        return buttons;
    }

    /// <summary>
    /// Удаляет все дочерние объекты (варианты выбора) из текущего объекта.
    /// </summary>
    public void DestroyChoices()
    {
        for (int i = 0; i < transform.childCount; ++i)
            Destroy(transform.GetChild(i).gameObject);
    }
}
