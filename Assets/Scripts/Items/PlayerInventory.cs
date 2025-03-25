using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    [SerializeField] PlayerArmor armor;
    [SerializeField] Potion potion;
    [SerializeField] AlternateAttackWeapon playerWeapon;
    byte potionsCount = 0;
    byte maxPotionsCount = 12;
    byte explosiveArrowCount = 0;
    byte maxExplosiveArrowCount = 99;
    int money = 0;

    public bool AddPotion()
    {
        if (potionsCount < maxPotionsCount)
        {
            potionsCount++;
            return true;
        }
        return false;
    }

    public bool UsePotion()
    {
        if (potionsCount > 0)
        {
            potionsCount--;
            return true;
        }
        return false;
    }

    public bool AddExplosiveArrow()
    {
        if (explosiveArrowCount < maxExplosiveArrowCount)
        {
            explosiveArrowCount++;
            return true;
        }
        return false;
    }

    public bool UseExplosiveArrow()
    {
        if (explosiveArrowCount > 0)
        {
            explosiveArrowCount--;
            return true;
        }
        return false;
    }

    public PlayerArmor GetPlayerArmor()
    {
        return armor;
    }

    public AlternateAttackWeapon GetPlayerWeapon()
    {
        return playerWeapon;
    }

    public void SetPlayerWeapon(AlternateAttackWeapon newWeapon)
    {
        playerWeapon = newWeapon;
    }

    public Potion GetPotion()
    {
        return potion;
    }
}
