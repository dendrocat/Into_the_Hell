using UnityEngine;

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

    void DoInteract()
    {
        if (_interactable != null)
        {
            _interactable.Interact();
        }
    }
}
