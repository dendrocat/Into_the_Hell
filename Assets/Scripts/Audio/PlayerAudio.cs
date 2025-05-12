/// <summary>
/// Класс для управления аудио объекта класса <see cref="Player"/>, наследует функциональность <see cref="PersonAudio" />.
/// </summary>
public class PlayerAudio : PersonAudio
{
    /// <inheritdoc />
    protected override void InitAudio()
    {
        base.InitAudio();
        var inventory = GetComponent<PlayerInventory>();
        inventory.MoneyChanged.AddListener(
            (money) => Play("Coins")
        );
    }
}
