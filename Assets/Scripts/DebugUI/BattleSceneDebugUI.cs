using TMPro;
using UnityEngine;

/**
 * <summary>
 * Класс, отвечающий за управление тестовым интерфейсом для дебага.
 * </summary>
 * 
 * **/
public class BattleSceneDebugUI : MonoBehaviour
{
    [SerializeField] Player player;
    [SerializeField] Boss boss;

    [SerializeField] TMP_Text playerHPText;
    [SerializeField] TMP_Text dashCount;
    [SerializeField] TMP_Text potionCount;
    [SerializeField] TMP_Text expArrowCount;
    [SerializeField] TMP_Text moneyCount;
    [SerializeField] TMP_Text playerEffectsText;

    [SerializeField] TMP_Text bossNameText;
    [SerializeField] TMP_Text bossHPText;
    [SerializeField] TMP_Text bossChainAttackText;
    [SerializeField] TMP_Text bossMiniGolemText;

    private void Start()
    {
        if (!boss)
        {
            bossNameText.gameObject.SetActive(false);
            bossHPText.gameObject.SetActive(false);
            bossChainAttackText.gameObject.SetActive(false);
            bossMiniGolemText.gameObject.SetActive(false);
        }
    }

    /**
     * <summary>
     * Метод Update. Обновляет информацию в тестовом интерфейсе дебага.
     * </summary>
     * 
     * **/
    void Update()
    {
        float playerHP = player.getHP();

        playerHPText.SetText("Player HP: " + playerHP + "\nArmor: " + player.inventory.GetPlayerArmor().level);
        dashCount.SetText("Shift count: " + player.ShiftCount);
        potionCount.SetText("Potion count: " + player.inventory.GetPotionCount() + (player.isHealReloading() ? " (reload)" : ""));
        expArrowCount.SetText("ExpArrow count: " + player.inventory.GetExplosiveArrowCount());
        moneyCount.SetText("Money count: " + player.inventory.GetMoney());


        string playerEffects = "Player Effects:\n";
        for (int i = 0; i < 11; i++)
        {
            playerEffects += player.getEffectCount(i);
            if (i < 9)
            {
                playerEffects += ",";
            }
        }

        playerEffectsText.SetText(playerEffects);

        if (boss)
        {
            bossNameText.SetText(boss.GetBossName());
            bossHPText.SetText("" + boss.getHP() + " / " + boss.GetMaxHealth());
            bossChainAttackText.SetText("Attack chain: " + boss.getEffectCount(EffectNames.AttackChain));
            bossMiniGolemText.SetText("Mini-golem level: " + boss.getEffectCount(EffectNames.MiniGolem));
        }
    }
}
