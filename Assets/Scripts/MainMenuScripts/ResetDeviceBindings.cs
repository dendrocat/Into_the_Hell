using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// Компонент для сброса пользовательских привязок управления.
/// Позволяет сбросить все привязки или только привязки для определённой схемы управления.
/// </summary>
public class ResetDeviceBindings : MonoBehaviour
{
    /// <summary>
    /// Ссылка на набор действий ввода.
    /// </summary>
    [Tooltip("Набор действий ввода")]
    [SerializeField] private InputActionAsset _inputActions;

    /// <summary>
    /// Имя целевой схемы управления, для которой будут сброшены привязки.
    /// </summary>
    [Tooltip("Имя целевой схемы управления для сброса привязок")]
    [SerializeField] private string _targetControlScheme;

       /// <summary>
    /// Инициализация: получает действия ввода из InputManager.
    /// </summary>
    void Start()
    {
        _inputActions = InputManager.Instance.GetActions();
    }

    /// <summary>
    /// Сбрасывает все пользовательские переопределения привязок для всех карт действий.
    /// </summary>
    public void ResetAllBindings()
    {
        foreach (InputActionMap map in _inputActions.actionMaps)
        {
            map.RemoveAllBindingOverrides();
        }
    }

    /// <summary>
    /// Сбрасывает пользовательские переопределения привязок только для указанной схемы управления.
    /// </summary>
    public void ResetControlSchemeBinding()
    {
        foreach (InputActionMap map in _inputActions.actionMaps)
        {
            foreach (InputAction action in map.actions)
            {
                action.RemoveBindingOverride(InputBinding.MaskByGroup(_targetControlScheme));
            }
        }
    }
}
