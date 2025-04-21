using System.Collections;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [SerializeField] Player _player;

    [SerializeField] UIController _ui;

    void Start()
    {
        _ui.SetArrows(_player.inventory.GetExplosiveArrowCount());
        _ui.SetPotions(_player.inventory.GetPotionCount());
        _ui.SetMoney(_player.inventory.GetMoney());
        _ui.HealthBar.SetHealth(_player.Health / _player.MaxHealth);
        _ui.ShiftController.SetShiftCount(_player.ShiftCount);
        StartCoroutine(TestDamageCoroutine());
        StartCoroutine(TestMoneyCorotine());
        StartCoroutine(TestImmidiateCoroutine());
    }

    IEnumerator TestImmidiateCoroutine()
    {
        yield return new WaitForSecondsRealtime(2f);
        _player.inventory.UseExplosiveArrow();
        _player.inventory.UsePotion();
        _ui.SetPotions(_player.inventory.GetPotionCount());
        _ui.SetArrows(_player.inventory.GetExplosiveArrowCount());
    }

    IEnumerator TestDamageCoroutine()
    {
        yield return null;
        yield return new WaitForSecondsRealtime(1f);
        _player.TakeDamage(50f, DamageType.None);
        _ui.HealthBar.SetHealthSmoothed(_player.Health / _player.MaxHealth);
    }

    IEnumerator TestMoneyCorotine()
    {
        yield return new WaitForSecondsRealtime(1.5f);
        _player.inventory.ModifyMoneyCount(20);
        _ui.SetMoneySmooth(_player.inventory.GetMoney());
    }
}
