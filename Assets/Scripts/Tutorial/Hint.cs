using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// Скриптабельный объект для хранения подсказок, связанных с действиями ввода.
/// </summary>
[CreateAssetMenu(fileName = "HintConfig", menuName = "Hint")]
public class Hint : ScriptableObject
{
    /// <summary>
    /// Структура, описывающая действие ввода и связанную с ним подсказку.
    /// </summary>
    [Serializable]
    struct ActionHint
    {
        [Tooltip("Ссылка на действие ввода")]
        public InputActionReference inputAction;

        [Tooltip("Текст подсказки с плейсхолдерами для клавиш")]
        public string hint;
    }

    [Tooltip("Список подсказок")]
    [SerializeField] List<ActionHint> _actionHints;

    /// <summary>
    /// Сформированные подсказки с учётом текущих привязок клавиш.
    /// </summary>
    public List<string> Hints { get; private set; }

    /// <summary>
    /// Заполняет текст подсказки, подставляя текущие отображаемые строки привязок клавиш.
    /// </summary>
    /// <param name="actionHint">Структура с действием и шаблоном подсказки.</param>
    /// <returns>Заполненный текст подсказки.</returns>
    string FillHintByBindings(ActionHint actionHint)
    {
        List<string> bindDisplay = new();
        var action = actionHint.inputAction.action;
        for (int bindingIndex = 0; bindingIndex < action.bindings.Count; bindingIndex++)
        {
            if (action.bindings[bindingIndex].isPartOfComposite) continue;

            bindDisplay.Add(action.GetBindingDisplayString(
                bindingIndex,
                InputBinding.DisplayStringOptions.DontIncludeInteractions
            ));
        }

        var hint = actionHint.hint;
        int cnt = hint.CountIndices('$');
        for (int i = 0; i < cnt; ++i)
            hint = hint.Replace($"${i}", bindDisplay[i]);
        return hint;
    }

    /// <summary>
    /// Инициализирует список подсказок, используя рантайм-ссылки на действия.
    /// </summary>
    /// <param name="finder">Объект для поиска рантайм-экземпляров действий.</param>
    public void InitHint(ActionRuntimeFinder finder)
    {
        Hints = new();
        for (int i = 0; i < _actionHints.Count; ++i)
        {
            if (_actionHints[i].inputAction == null)
            {
                Hints.Add(_actionHints[i].hint);
                continue;
            }
            var a = new ActionHint
            {
                inputAction = finder.FindRuntimeAction(_actionHints[i].inputAction),
                hint = _actionHints[i].hint
            };
            Hints.Add(FillHintByBindings(a));
        }
    }
}
