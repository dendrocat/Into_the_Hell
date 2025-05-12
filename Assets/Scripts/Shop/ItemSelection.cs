using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Управляет выбором элемента из набора кнопок, подсвечивая выбранную кнопку.
/// </summary>
public class ItemSelection : MonoBehaviour
{
    [Tooltip("Цвет подсветки выбранной кнопки")]
    [SerializeField] Color _selectedColor;

    /// <summary>
    /// Список управляемых кнопок
    /// </summary>
    List<Button> _buttons;

    /// <summary>
    /// Инициализация списка кнопок
    /// </summary>
    void Start()
    {
        _buttons = new();
        for (int i = 0; i < transform.childCount; ++i)
        {
            var button = transform.GetChild(i).GetComponent<Button>();
            button.onClick.AddListener(() => SelectItem(button));
            _buttons.Add(button);
        }
    }

    /// <summary>
    /// Сбрасывает цвета всех кнопок к их нормальному состоянию.
    /// </summary>
    void ResetColors()
    {
        foreach (var button in _buttons)
            button.targetGraphic.color = button.colors.normalColor;
    }

    /// <summary>
    /// Выбирает кнопку и подсвечивает её.
    /// </summary>
    /// <param name="button">Кнопка, которую нужно выделить.</param>
    public void SelectItem(Button button)
    {
        //Debug.Log(button.gameObject.name);
        ResetColors();

        button.targetGraphic.color = _selectedColor;
    }
}
