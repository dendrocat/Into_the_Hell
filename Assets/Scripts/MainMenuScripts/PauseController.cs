using UnityEngine;

public class PauseController : MonoBehaviour
{
    void Awake()
    {
        gameObject.SetActive(false);
    }
    void OnEnable()
    {
        Activate();
    }

    void OnDisable()
    {
        Deactivate();
    }

    public void PauseGame()
    {
        gameObject.SetActive(true);
        GameManager.Instance.PauseGame();
    }

    public void UnPauseGame()
    {
        gameObject.SetActive(false);
        GameManager.Instance.UnPauseGame();
    }

    public void ToMainMenu()
    {
        GameManager.Instance.ToMainMenu();
    }

    public void Activate()
    {
        InputManager.Instance.PausePressed.RemoveListener(PauseGame);
        InputManager.Instance.CancelPressed.AddListener(UnPauseGame);
    }

    public void Deactivate()
    {
        InputManager.Instance.PausePressed.AddListener(PauseGame);
        InputManager.Instance.CancelPressed.RemoveListener(UnPauseGame);
    }
}
