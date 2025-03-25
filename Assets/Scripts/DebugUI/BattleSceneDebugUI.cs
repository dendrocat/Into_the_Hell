using TMPro;
using UnityEngine;

public class BattleSceneDebugUI : MonoBehaviour
{
    public Player player;

    public TMP_Text playerHPText;
    public TMP_Text dashCount;
    public TMP_Text playerEffectsText;

    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        float playerHP = player.getHP();

        playerHPText.SetText("Player HP: " + playerHP);
        dashCount.SetText("Shift count: " + player.ShiftCount);

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
