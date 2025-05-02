using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class ButtonCancelSubscribe : MonoBehaviour
{
    Button _button;
    void Awake()
    {
        _button = GetComponent<Button>();
    }

    void OnEnable()
    {
        InputManager.Instance.CancelPressed.AddListener(_button.onClick.Invoke);
    }

    void OnDisable()
    {
        InputManager.Instance?.CancelPressed.RemoveListener(_button.onClick.Invoke);
    }
}
