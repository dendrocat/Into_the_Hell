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
        InputManager.Instance.OnCancelPressed.AddListener(_button.onClick.Invoke);
    }

    void OnDisable()
    {
        InputManager.Instance?.OnCancelPressed.RemoveListener(_button.onClick.Invoke);
    }
}
