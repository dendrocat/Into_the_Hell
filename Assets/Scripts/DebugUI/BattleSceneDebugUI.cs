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

    [SerializeField] TMP_Text playerHPText;
    [SerializeField] TMP_Text dashCount;
    [SerializeField] TMP_Text potionCount;
    [SerializeField] TMP_Text expArrowCount;
    [SerializeField] TMP_Text playerEffectsText;

    void Start()
    {

    }

    void Update()
    {
        float playerHP = player.getHP();

        playerHPText.SetText("Player HP: " + playerHP + "\nArmor: " + player.inventory.GetPlayerArmor().level);
        dashCount.SetText("Shift count: " + player.ShiftCount);
        potionCount.SetText("Potion count: " + player.inventory.GetPotionCount());
        expArrowCount.SetText("ExpArrow count: " + player.inventory.GetExplosiveArrowCount());

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
    }
}
