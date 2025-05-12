using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// Класс для поиска и получения ссылки на экземпляр <see cref="InputActionReference"/>, 
/// соответствующий действию во время выполнения игры.
/// </summary>
/// <remarks>
/// Позволяет получить runtime-версию <see cref="InputActionReference"/>, которая привязана к активной карте действий <see cref="InputManager"/>.
/// </remarks>
public class ActionRuntimeFinder : MonoBehaviour
{
    /// <summary>
    /// Поиск и создание runtime-ссылки на <see cref="InputActionReference"/>, соответствующую переданному эталонному действию.
    /// </summary>
    /// <param name="reference">Исходная ссылка на <see cref="InputActionReference"/>, которую нужно найти в runtime.</param>
    /// <returns>
    /// Новый экземпляр <see cref="InputActionReference"/>, связанный с runtime-версией действия, 
    /// либо <c>null</c>, если действие не найдено.
    /// </returns>
    public InputActionReference FindRuntimeAction(InputActionReference reference)
    {
        InputActionReference runtimeReference = null;
        var actions = InputManager.Instance.GetActions();
        foreach (var i1 in actions.actionMaps)
        {
            foreach (var i2 in i1.actions)
            {
                if (i2 == reference.action)
                {
                    runtimeReference = ScriptableObject.CreateInstance<InputActionReference>();
                    runtimeReference.Set(i2);
                    break;
                }
            }
            if (runtimeReference) break;
        }
        if (runtimeReference == null)
        {
            Debug.LogError("Action don't find in runtime input action map");
        }
        return runtimeReference;
    }
}
