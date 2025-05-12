/// <summary>
/// NPC, который при взаимодействии запускает диалог и может инициировать переход на следующий уровень.
/// </summary>
public class ExitNPC : DialogableNPC
{
    /// <summary>
    /// Метод, вызываемый через внешний внешнюю функцию Ink-диалога "exit".
    /// Запускает переход на следующий уровень через <see cref="GameManager"/>.
    /// </summary>
    void Exit()
    {
        GameManager.Instance.NextLevel();
    }

    /// <summary>
    /// Переопределённый метод для установки истории диалога и привязки внешней функции "exit" из Ink-диалога к функции <see cref="Exit"/>.
    /// </summary>
    protected override void SetStory()
    {
        base.SetStory();
        DialogManager.Instance.BindFunction("exit", Exit);
    }
}
