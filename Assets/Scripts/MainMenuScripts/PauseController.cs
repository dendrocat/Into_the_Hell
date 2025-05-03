using UnityEngine;

public class PauseController : MonoBehaviour
{
    void Awake()
    {
        gameObject.SetActive(false);
    }
    void OnEnable()
    {
        Debug.Log("OnEnable");
        Activate();
    }

    void OnDisable()
    {
        Debug.Log("OnDisable");
        Deactivate();
    }

    public void PauseGame()
    {
        Debug.Log("Pause Game");
        gameObject.SetActive(true);
        GameManager.Instance.PauseGame();
    }

    public void UnPauseGame()
    {
        Debug.Log("Unpause Game");
        gameObject.SetActive(false);
        GameManager.Instance.UnPauseGame();
    }

    public void ToMainMenu()
    {
        Debug.Log("ToMainMenu");
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
