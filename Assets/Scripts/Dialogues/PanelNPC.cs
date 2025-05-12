using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// NPC, который связывает функции Ink с открытием различных UI-панелей.
/// </summary>
public class PanelNPC : DialogableNPC
{
    /// <summary>
    /// Структура, связывающая имя функции Ink и панель для открытия.
    /// </summary>
    [Serializable]
    struct FuncPanel
    {
        /// <summary>
        /// Имя внешней функции в Ink, которую нужно привязать.
        /// </summary>
        [Tooltip("Имя внешней функции в Ink, которую нужно привязать.")]
        public string inkFuncName;

        /// <summary>
        /// Панель, которая будет открываться при вызове функции.
        /// </summary>
        [Tooltip("Панель, которая будет открываться при вызове функции.")]
        public BasePanel panel;
    }

    /// <summary>
    /// Список функций Ink и соответствующих им UI-панелей
    /// </summary>
    [Tooltip("Список функций Ink и соответствующих им UI-панелей")]
    [SerializeField] List<FuncPanel> _funcPanels;


    /// <summary>
    /// Переопределённый метод для установки истории диалога и привязки функций Ink к открытию панелей.
    /// </summary>
    protected override void SetStory()
    {
        base.SetStory();
        foreach (var panel in _funcPanels)
            DialogManager.Instance.BindFunction(
                panel.inkFuncName,
                () => panel.panel.Open()
            );
    }
}
