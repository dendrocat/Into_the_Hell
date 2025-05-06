public class PlayerAudio : PersonAudio
{
    protected override void InitAudio()
    {
        base.InitAudio();
        var inventory = GetComponent<PlayerInventory>();
        inventory.MoneyChanged.AddListener(
            (money) => Play("Coins")
        );
    }
}
