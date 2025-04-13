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

    public byte potionLevel;
    public byte potionCount;

    public byte arrowCount;

    public byte armorLevel;
}

[Serializable]
public struct GameData
{
    public int level;
    public PlayerData playerData;
    public List<byte> weaponLevels;
}
