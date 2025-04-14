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
}
