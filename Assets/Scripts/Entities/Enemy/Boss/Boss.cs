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

    protected override void Start()
    {
        base.Start();
        _audioPlayer.Play("Start");
        destructionDelay = 4f;
    }


    new public void Attack()
    {
        if (!isAlive()) return;
        bool[] reloading = new bool[3];
        reloading[0] = bossWeapon.isReloading();
        reloading[1] = bossWeapon.Attack2IsReloading();
        reloading[2] = bossWeapon.Attack3IsReloading();

        int selectedWeapon = 2;

        for (; selectedWeapon >= 0; selectedWeapon--) //перебираем атаки с конца, так как приоритет выше у 3 и 2 атаки
        {
            if (bossWeapon.CanUseAttack(selectedWeapon) && !reloading[selectedWeapon])
            {
                _audioPlayer.Play($"Attack{selectedWeapon + 1}");
                bossWeapon.LaunchAttackByNumber(selectedWeapon);
                break;
            }
        }
    }
}
