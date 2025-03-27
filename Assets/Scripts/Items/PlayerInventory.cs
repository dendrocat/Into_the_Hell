using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    [SerializeField] PlayerArmor armor;
    [SerializeField] Potion potion;
    [SerializeField] AlternateAttackWeapon playerWeapon;
    [SerializeField] byte potionsCount = 0;
    byte maxPotionsCount = 12;
    [SerializeField] byte explosiveArrowCount = 0;
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

    public void SetPotionCount(int count)
    {
        potionsCount = (byte)count;
    }

    public int GetPotionCount()
    {
        return potionsCount;
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

    public void SetExplosiveArrowCount(int count)
    {
        explosiveArrowCount = (byte) count;
    }

    public int GetExplosiveArrowCount()
    {
        return explosiveArrowCount;
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

    public int GetMoney()
    {
        return money;
    }

    public bool ModifyMoneyCount(int diff)
    {
        if (money - diff > 0)
        {
            money -= diff;
            return true;
        }
        return false;
    }
}
