using System;
using UnityEngine;

/// <summary>
/// Интерфейс, определяющий базовые методы для менеджера диалогов Ink.
/// </summary>
public interface IDialogManager
{
    /// <summary>
    /// Устанавливает Ink-историю из JSON-файла.
    /// </summary>
    /// <param name="inkJSONfile">Файл с Ink-историей в формате JSON.</param>
    public void SetStory(TextAsset inkJSONfile);

    /// <summary>
    /// Привязывает внешнюю функцию к Ink-истории для вызова из скрипта.
    /// </summary>
    /// <param name="inkFuncName">Имя функции в Ink.</param>
    /// <param name="innerFunc">Делегат с реализацией функции.</param>
    public void BindFunction(string inkFuncName, Action innerFunc);

    /// <summary>
    /// Запускает воспроизведение диалога.
    /// </summary>
    public void StartStory();
}