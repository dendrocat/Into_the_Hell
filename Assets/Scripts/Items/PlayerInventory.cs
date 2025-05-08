using UnityEngine;
using UnityEngine.Events;

/**
 * <summary>
 * Класс, описывающий инвентарь игрока.
 * </summary>
 * 
 * ps. Я люблю писать докстринги (нет)
 * **/
public class PlayerInventory : MonoBehaviour
{
    [HideInInspector]
    public UnityEvent<int> ExplosiveArrowCountChanged = new();
    [HideInInspector]
    public UnityEvent<int> MoneyChanged = new();
    [HideInInspector]
    public UnityEvent<int> PotionCountChanged = new();

    [SerializeField] PlayerArmor armor;
    [SerializeField] Potion potion;
    [SerializeField] AlternateAttackWeapon playerWeapon;
    [SerializeField] byte potionsCount = 0;
    byte maxPotionsCount = 12;
    public byte MaxPotionsCount => maxPotionsCount;
    [SerializeField] byte explosiveArrowCount = 0;
    byte maxExplosiveArrowCount = 99;
    public byte MaxExplosiveArrowCount => maxExplosiveArrowCount;
    [SerializeField]
    [Range(0, 9999)] int money = 0;

    /**
     * <summary>
     * Метод, добавляющий зелье в инвентарь игрока. Возвращает результат добавления.
     * </summary>
     * <returns>true, если зелье было добавлено, иначе false</returns>
     * **/
    public bool AddPotion()
    {
        if (potionsCount < maxPotionsCount)
        {
            potionsCount++;
            PotionCountChanged.Invoke(potionsCount);
            return true;
        }
        return false;
    }

    /**
     * <summary>
     * Метод, устанавливающий количество зелий в инвентаре игрока.
     * </summary>
     * **/
    public void SetPotionCount(int count)
    {
        potionsCount = (byte)count;
    }

    /** 
     * <summary>
     * Метод, возвращающий количество зелий в инвентаре игрока.
     * </summary>
     * <returns>int - количество зелий в инвентаре.</returns>
     * **/
    public int GetPotionCount()
    {
        return potionsCount;
    }

    /**
     * <summary>
     * Метод, использующий одно зелье в инвентаре игрока. Возвращает результат использования.
     * </summary>
     * <returns>true, если зелье было использовано, иначе false</returns>
     * **/
    public bool UsePotion()
    {
        if (potionsCount > 0)
        {
            potionsCount--;
            PotionCountChanged.Invoke(potionsCount);
            return true;
        }
        return false;
    }

    /**
     * <summary>
     * Метод, добавляющий взрывную стрелу в инвентарь игрока. Возвращает результат добавления.
     * </summary>
     * <returns>true, если взрывная стрела была добавлена, иначе false</returns>
     * **/
    public bool AddExplosiveArrow()
    {
        if (explosiveArrowCount < maxExplosiveArrowCount)
        {
            explosiveArrowCount++;
            ExplosiveArrowCountChanged.Invoke(explosiveArrowCount);
            return true;
        }
        return false;
    }

    /**
     * <summary>
     * Метод, устанавливающий количество взрывных стрел в инвентаре игрока.
     * </summary>
     * **/
    public void SetExplosiveArrowCount(int count)
    {
        explosiveArrowCount = (byte)count;
    }

    /** 
     * <summary>
     * Метод, возвращающий количество взрывных стрел в инвентаре игрока.
     * </summary>
     * <returns>int - количество взрывных стрел в инвентаре.</returns>
     * **/
    public int GetExplosiveArrowCount()
    {
        return explosiveArrowCount;
    }

    /**
     * <summary>
     * Метод, использующий одну взрывную стрелу в инвентаре игрока. 
     * Возвращает результат использования.
     * </summary>
     * <returns>true, если взрывная стрела была использована, иначе false</returns>
     * **/
    public bool UseExplosiveArrow()
    {
        if (explosiveArrowCount > 0)
        {
            explosiveArrowCount--;
            ExplosiveArrowCountChanged.Invoke(explosiveArrowCount);
            return true;
        }
        return false;
    }

    /**
     * <summary>
     * Возвращает ссылку на броню игрока.
     * </summary>
     * **/
    public PlayerArmor GetPlayerArmor()
    {
        return armor;
    }

    /**
     * <summary>
     * Возвращает ссылку на оружие игрока.
     * </summary>
     * **/
    public AlternateAttackWeapon GetPlayerWeapon()
    {
        return playerWeapon;
    }

    /**
     * <summary>
     * Устанавливает оружие для игрока.
     * </summary>
     * **/
    public void SetPlayerWeapon(AlternateAttackWeapon weaponPrefab,
    byte level)
    {
        if (playerWeapon != null)
        {
            Destroy(playerWeapon.gameObject);
        }
        playerWeapon = Instantiate(weaponPrefab.gameObject, transform).GetComponent<AlternateAttackWeapon>();
        playerWeapon.level = level;
        GetComponent<Player>().weaponObject = playerWeapon.transform;
    }

    /**
     * <summary>
     * Возвращает ссылку на зелье.
     * </summary>
     * **/
    public Potion GetPotion()
    {
        return potion;
    }

    /**
     * <summary>
     * Возвращает количество монет в инвентаре.
     * </summary>
     * <returns>Количество денег у игрока.</returns>
     * **/
    public int GetMoney()
    {
        return money;
    }

    /**
     * <summary>
     * Изменяет количество монет у игрока.
     * Возвращает результат выполнения.
     * </summary>
     * <returns>true, если количество монет успешно изменено, иначе false</returns>
     * **/
    public bool ModifyMoneyCount(int diff)
    {
        if (money + diff >= 0)
        {
            money += diff;
            MoneyChanged.Invoke(money);
            return true;
        }
        return false;
    }

    public PlayerData GetPlayerData()
    {
        var data = new PlayerData();

        if (playerWeapon is Sword)
            data.weapon = WeaponType.Sword;
        else if (playerWeapon is TwoHandedSword)
            data.weapon = WeaponType.TwoHandedSword;
        else if (playerWeapon is Bow)
            data.weapon = WeaponType.Bow;
        else
            Debug.LogError("Unsupported weapon type");


        data.money = money;
        data.potionLevel = potion.level;
        data.potionCount = potionsCount;
        data.arrowCount = explosiveArrowCount;
        data.armorLevel = armor.level;
        return data;
    }

    public void SetPlayerData(PlayerData data)
    {
        money = data.money;
        potion.level = data.potionLevel;
        potionsCount = data.potionCount;
        explosiveArrowCount = data.arrowCount;
        armor.level = data.armorLevel;
    }
}
