using UnityEngine;

/**
 * <summary>
 * Класс, описывающий босса.
 * </summary>
 * **/
public class Boss : BaseEnemy
{
    [SerializeField] string bossName;
    public BossWeapon bossWeapon;

    public string GetBossName()
    {
        return bossName;
    }

    new public void Attack()
    {
        bool[] reloading = new bool[3];
        reloading[0] = bossWeapon.isReloading();
        reloading[1] = bossWeapon.Attack2IsReloading();
        reloading[2] = bossWeapon.Attack3IsReloading();

        int selectedWeapon = 2;

        for (; selectedWeapon >= 0; selectedWeapon--) //перебираем атаки с конца, так как приоритет выше у 3 и 2 атаки
        {
            if (bossWeapon.CanUseAttack(selectedWeapon) && !reloading[selectedWeapon])
            {
                bossWeapon.LaunchAttackByNumber(selectedWeapon);
                break;
            }
        }
    }
}
