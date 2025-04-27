using UnityEngine;
using UnityEngine.SceneManagement;

public class ExitNPC : DialogableNPC
{
    [SerializeField] string _levelScene;

    void Exit()
    {
        SceneManager.LoadScene("LevelScene");
    }

    protected override void SetStory()
    {
        base.SetStory();
        DialogManager.Instance.BindFunction("exit", Exit);
    }
}
