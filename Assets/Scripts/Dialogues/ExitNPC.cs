public class ExitNPC : DialogableNPC
{
    void Exit()
    {
        GameManager.Instance.NextLevel();
    }

    protected override void SetStory()
    {
        base.SetStory();
        DialogManager.Instance.BindFunction("exit", Exit);
    }
}
