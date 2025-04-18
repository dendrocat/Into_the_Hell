using UnityEngine;

public class TestGameManager : MonoBehaviour
{
    void Start()
    {
        LoadGame();
    }
    public void LoadGame()
    {
        SaveLoadManager.Load();
    }

    public void SaveGame()
    {
        SaveLoadManager.Save();
    }

    void Update()
    {
        if (InputManager.Instance.Interact)
        {
            SaveGame();
        }
        if (InputManager.Instance.Pause)
        {
            SaveLoadRepository.RemoveSave();
        }
    }
}
