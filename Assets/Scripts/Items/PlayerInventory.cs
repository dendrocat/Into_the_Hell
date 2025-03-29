using UnityEngine;

/**
 * <summary>
 * �����, ����������� ��������� ������.
 * </summary>
 * 
 * ps. � ����� ������ ���������� (���)
 * **/
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

    /**
     * <summary>
     * �����, ����������� ����� � ��������� ������. ���������� ��������� ����������.
     * </summary>
     * <returns>true, ���� ����� ���� ���������, ����� false</returns>
     * **/
    public bool AddPotion()
    {
        if (potionsCount < maxPotionsCount)
        {
            potionsCount++;
            return true;
        }
        return false;
    }

    /**
     * <summary>
     * �����, ��������������� ���������� ����� � ��������� ������.
     * </summary>
     * **/
    public void SetPotionCount(int count)
    {
        potionsCount = (byte)count;
    }

    /** 
     * <summary>
     * �����, ������������ ���������� ����� � ��������� ������.
     * </summary>
     * <returns>int - ���������� ����� � ���������.</returns>
     * **/
    public int GetPotionCount()
    {
        return potionsCount;
    }

    /**
     * <summary>
     * �����, ������������ ���� ����� � ��������� ������. ���������� ��������� �������������.
     * </summary>
     * <returns>true, ���� ����� ���� ������������, ����� false</returns>
     * **/
    public bool UsePotion()
    {
        if (potionsCount > 0)
        {
            potionsCount--;
            return true;
        }
        return false;
    }

    /**
     * <summary>
     * �����, ����������� �������� ������ � ��������� ������. ���������� ��������� ����������.
     * </summary>
     * <returns>true, ���� �������� ������ ���� ���������, ����� false</returns>
     * **/
    public bool AddExplosiveArrow()
    {
        if (explosiveArrowCount < maxExplosiveArrowCount)
        {
            explosiveArrowCount++;
            return true;
        }
        return false;
    }

    /**
     * <summary>
     * �����, ��������������� ���������� �������� ����� � ��������� ������.
     * </summary>
     * **/
    public void SetExplosiveArrowCount(int count)
    {
        explosiveArrowCount = (byte) count;
    }

    /** 
     * <summary>
     * �����, ������������ ���������� �������� ����� � ��������� ������.
     * </summary>
     * <returns>int - ���������� �������� ����� � ���������.</returns>
     * **/
    public int GetExplosiveArrowCount()
    {
        return explosiveArrowCount;
    }

    /**
     * <summary>
     * �����, ������������ ���� �������� ������ � ��������� ������. 
     * ���������� ��������� �������������.
     * </summary>
     * <returns>true, ���� �������� ������ ���� ������������, ����� false</returns>
     * **/
    public bool UseExplosiveArrow()
    {
        if (explosiveArrowCount > 0)
        {
            explosiveArrowCount--;
            return true;
        }
        return false;
    }

    /**
     * <summary>
     * ���������� ������ �� ����� ������.
     * </summary>
     * **/
    public PlayerArmor GetPlayerArmor()
    {
        return armor;
    }

    /**
     * <summary>
     * ���������� ������ �� ������ ������.
     * </summary>
     * **/
    public AlternateAttackWeapon GetPlayerWeapon()
    {
        return playerWeapon;
    }

    /**
     * <summary>
     * ������������� ������ ��� ������.
     * </summary>
     * **/
    public void SetPlayerWeapon(AlternateAttackWeapon newWeapon)
    {
        playerWeapon = newWeapon;
    }

    /**
     * <summary>
     * ���������� ������ �� �����.
     * </summary>
     * **/
    public Potion GetPotion()
    {
        return potion;
    }

    /**
     * <summary>
     * ���������� ���������� ����� � ���������.
     * </summary>
     * <returns>���������� ����� � ������.</returns>
     * **/
    public int GetMoney()
    {
        return money;
    }

    /**
     * <summary>
     * �������� ���������� ����� � ������.
     * ���������� ��������� ����������.
     * </summary>
     * <returns>true, ���� ���������� ����� ������� ��������, ����� false</returns>
     * **/
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
