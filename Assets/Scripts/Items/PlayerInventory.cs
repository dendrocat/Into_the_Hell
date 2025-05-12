using UnityEngine;
using UnityEngine.Events;


/// <summary>
/// Класс, описывающий инвентарь игрока.
/// </summary>
public class PlayerInventory : MonoBehaviour
{
    /// <summary>
    /// Событие, вызываемое при изменении количества взрывных стрел.
    /// </summary>
    [HideInInspector] public UnityEvent<int> ExplosiveArrowCountChanged = new();
    
    /// <summary>
    /// Событие, вызываемое при изменении количества денег.
    /// </summary>
    [HideInInspector] public UnityEvent<int> MoneyChanged = new();
    
    // <summary>
    /// Событие, вызываемое при изменении количества зелий.
    /// </summary>
    [HideInInspector] public UnityEvent<int> PotionCountChanged = new();

    /// <summary>
    /// <see cref="PlayerArmor">Броня</see> игрока.
    /// </summary>
    [Tooltip("Броня игрока")]
    [SerializeField] PlayerArmor armor;

    /// <summary>
    /// <see cref="Potion">Зелье</see> игрока.
    /// </summary>
    [Tooltip("Зелье игрока")]
    [SerializeField] Potion potion;

    /// <summary>
    /// <see cref="AlternateAttackWeapon">Оружие</see> игрока.
    /// </summary>
    [Tooltip("Оружие игрока с альтернативной атакой")]
    [SerializeField] AlternateAttackWeapon playerWeapon;

    /// <summary>
    /// Количество зелий в инвентаре.
    /// </summary>
    [Tooltip("Количество зелий в инвентаре")]
    [SerializeField, Range(0, 12)] byte potionsCount = 0;

    /// <summary>
    /// Максимально возможное количество зелий.
    /// </summary>
    byte maxPotionsCount = 12;

    // <summary>
    /// Максимальное количество зелий (для внешнего доступа).
    /// </summary>
    public byte MaxPotionsCount => maxPotionsCount;

    /// <summary>
    /// Количество взрывных стрел в инвентаре.
    /// </summary>
    [Tooltip("Количество взрывных стрел в инвентаре")]
    [SerializeField, Range(0, 99)] byte explosiveArrowCount = 0;

    /// <summary>
    /// Максимально возможное количество взрывных стрел.
    /// </summary>
    byte maxExplosiveArrowCount = 99;

    /// <summary>
    /// Максимальное количество взрывных стрел (для внешнего доступа).
    /// </summary>
    public byte MaxExplosiveArrowCount => maxExplosiveArrowCount;

    /// <summary>
    /// Количество денег у игрока.
    /// </summary>
    [Tooltip("Количество денег у игрока")]
    [SerializeField, Range(0, 9999)] int money = 0;

    /// <summary>
    /// Метод, добавляющий зелье в инвентарь игрока. Возвращает результат добавления.
    /// </summary>
    /// <returns><see langword="true"/>, если зелье было добавлено, иначе <see langword="false"/></returns>
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

    /// <summary>
    /// Метод, устанавливающий количество зелий в инвентаре игрока.
    /// </summary>
    /// <param name="count">Новое количество зелий.</param>
    public void SetPotionCount(int count)
    {
        potionsCount = (byte)count;
    }

 
    /// <summary>
    /// Метод, возвращающий количество зелий в инвентаре игрока.
    /// </summary>
    /// <returns><see langword="int"/> - количество зелий в инвентаре.</returns>
    public int GetPotionCount()
    {
        return potionsCount;
    }

    /// <summary>
    /// Метод, использующий одно зелье в инвентаре игрока. Возвращает результат использования.
    /// </summary>
    /// <returns><see langword="true"/>, если зелье было использовано, иначе <see langword="false"/></returns>
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

    /// <summary>
    /// Метод, добавляющий взрывную стрелу в инвентарь игрока. Возвращает результат добавления.
    /// </summary>
    /// <returns><see langword="true"/>, если взрывная стрела была добавлена, иначе <see langword="false"/></returns>
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


    /// <summary>
    /// Метод, устанавливающий количество взрывных стрел в инвентаре игрока.
    /// </summary>
    /// <param name="count">Новое количество взрывных стрел.</param>
    public void SetExplosiveArrowCount(int count)
    {
        explosiveArrowCount = (byte)count;
    }

 
    /// <summary>
    /// Метод, возвращающий количество взрывных стрел в инвентаре игрока.
    /// </summary>
    /// <returns><see langword="int"/> - количество взрывных стрел в инвентаре.</returns>
    public int GetExplosiveArrowCount()
    {
        return explosiveArrowCount;
    }


    /// <summary>
    /// Метод, использующий одну взрывную стрелу в инвентаре игрока. 
    /// Возвращает результат использования.
    /// </summary>
    /// <returns><see langword="true"/>, если взрывная стрела была использована, иначе <see langword="false"/></returns>
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


    /// <summary>
    /// Возвращает ссылку на <see cref="armor">броню</see> игрока.
    /// </summary>
    public PlayerArmor GetPlayerArmor()
    {
        return armor;
    }

    /// <summary>
    /// Возвращает ссылку на <see cref="playerWeapon">оружие</see> игрока.
    /// </summary>
    public AlternateAttackWeapon GetPlayerWeapon()
    {
        return playerWeapon;
    }


    /// <summary>
    /// Устанавливает <see cref="playerWeapon">оружие</see> для игрока.
    /// </summary>
    /// <param name="weaponPrefab">Префаб нового оружия.</param>
    public void SetPlayerWeapon(AlternateAttackWeapon weaponPrefab)
    {
        if (playerWeapon != null)
        {
            Destroy(playerWeapon.gameObject);
        }
        playerWeapon = Instantiate(weaponPrefab.gameObject, transform).GetComponent<AlternateAttackWeapon>();
        GetComponent<Player>().weaponObject = playerWeapon.transform;
    }


    /// <summary>
    /// Возвращает ссылку на <see cref="potion">зелье</see>.
    /// </summary>
    public Potion GetPotion()
    {
        return potion;
    }


    /// <summary>
    /// Возвращает количество <see cref="money">монет</see> в инвентаре.
    /// </summary>
    /// <returns><see langword="int"/> - Количество денег у игрока.</returns>
    public int GetMoney()
    {
        return money;
    }

    ///<summary>
    /// Изменяет количество <see cref="money">монет</see> у игрока.
    /// Возвращает результат выполнения.
    /// </summary>
    /// <param name="diff">Изменение количества денег (может быть отрицательным).</param>
    /// <returns><see langword="true"/>, если количество монет успешно изменено, иначе <see langword="false"/></returns>
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

    // <summary>
    /// Собирает и выдает данные игрока для сохранения.
    /// </summary>
    /// <returns>Структура <see cref="PlayerData"/> с текущими данными игрока.</returns>
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

    /// <summary>
    /// Устанавливает данные игрока из сохранения.
    /// </summary>
    /// <param name="data">Структура <see cref="PlayerData"/> с сохранёнными данными.</param>
    public void SetPlayerData(PlayerData data)
    {
        money = data.money;
        potion.level = data.potionLevel;
        SetPotionCount(data.potionCount);
        SetExplosiveArrowCount(data.arrowCount);
        armor.level = data.armorLevel;
    }
}
