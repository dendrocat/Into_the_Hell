using System;
using System.Collections.Generic;

public enum WeaponType
{
    Sword, TwoHandedSword, Bow
}

[Serializable]
public struct PlayerData
{
    public WeaponType weapon;
    public int money;

    public int potions;
    public int potionCount;

    public int arrowCount;

    public int armorLevel;
}

[Serializable]
public struct GameData
{
    int level;
    public PlayerData playerData;
    public List<int> weaponLevel;
}
