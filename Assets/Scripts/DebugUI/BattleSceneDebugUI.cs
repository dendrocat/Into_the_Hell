using TMPro;
using UnityEngine;

/// <summary>
/// Класс, отвечающий за управление тестовым интерфейсом для дебага.
/// </summary>
public class BattleSceneDebugUI : MonoBehaviour
{
    [Tooltip("Ссылка на объект игрока")]
    [SerializeField] Player player;

    [Tooltip("Ссылка на объект босса (может быть отсутствовать)")]
    [SerializeField] Boss boss;


    [Tooltip("Текстовое поле для отображения здоровья игрока")]
    [SerializeField] TMP_Text playerHPText;

    [Tooltip("Текстовое поле для отображения количества рывков (Shift)")]
    [SerializeField] TMP_Text dashCount;

    [Tooltip("Текстовое поле для отображения количества зелий")]
    [SerializeField] TMP_Text potionCount;

    [Tooltip("Текстовое поле для отображения количества взрывных стрел")]
    [SerializeField] TMP_Text expArrowCount;

    [Tooltip("Текстовое поле для отображения количества денег")]
    [SerializeField] TMP_Text moneyCount;

    [Tooltip("Текстовое поле для отображения эффектов игрока")]
    [SerializeField] TMP_Text playerEffectsText;


    [Tooltip("Текстовое поле для отображения имени босса")]
    [SerializeField] TMP_Text bossNameText;

    [Tooltip("Текстовое поле для отображения здоровья босса")]
    [SerializeField] TMP_Text bossHPText;

    [Tooltip("Текстовое поле для отображения уровня цепной атаки босса")]
    [SerializeField] TMP_Text bossChainAttackText;

    [Tooltip("Текстовое поле для отображения уровня мини-голиема босса")]
    [SerializeField] TMP_Text bossMiniGolemText;


    /// <summary>
    /// Метод Start вызывается при инициализации объекта.
    /// Проверяет наличие босса и скрывает соответствующие элементы интерфейса, если босса нет.
    /// </summary>
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


    /// <summary>
    /// Метод Update. Обновляет информацию в тестовом интерфейсе дебага.
    /// </summary>
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
            bossHPText.SetText("" + boss.getHP() + " / " + boss.MaxHealth);
            bossChainAttackText.SetText("Attack chain: " + boss.getEffectCount(EffectNames.AttackChain));
            bossMiniGolemText.SetText("Mini-golem level: " + boss.getEffectCount(EffectNames.MiniGolem));
        }
    }
}
