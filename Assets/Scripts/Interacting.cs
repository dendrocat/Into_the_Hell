using UnityEngine;

/// <summary>
/// Компонент, позволяющий взаимодействовать с объектами, реализующими интерфейс <see cref="IInteractable"/>, при входе и выходе из триггера.
/// </summary>
[RequireComponent(typeof(Collider2D))]
public class Interacting : MonoBehaviour
{
    IInteractable _interactable;
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.TryGetComponent(out IInteractable i))
        {
            if (_interactable == null)
            {
                _interactable = i;
                InputManager.Instance.InteractPressed.AddListener(DoInteract);
            }
        }
    }
    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out IInteractable i))
        {
            _interactable = null;
            InputManager.Instance.InteractPressed.RemoveListener(DoInteract);
        }
    }

    /// <summary>
    /// Вызывает метод взаимодействия у текущего объекта IInteractable.
    /// </summary>
    void DoInteract()
    {
        if (_interactable != null)
        {
            _interactable.Interact();
        }
    }
}
