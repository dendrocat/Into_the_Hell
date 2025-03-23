using TMPro;
using UnityEngine;

public class BattleSceneDebugUI : MonoBehaviour
{
    public Person player;
    public Person enemy;

    public TMP_Text playerHPText;
    public TMP_Text enemyHPText;
    public TMP_Text playerEffectsText;
    public TMP_Text enemyEffectsText;

    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        float playerHP = player.getHP();
        float enemyHP = enemy.getHP();

        playerHPText.SetText("Player HP: " + playerHP);
        enemyHPText.SetText("Enemy HP: " + enemyHP);

        string playerEffects = "Player Effects:\n";
        string enemyEffects = "Enemy Effects:\n";
        for (int i = 0; i < 10; i++)
        {
            playerEffects += player.getEffectCount(i);
            enemyEffects += enemy.getEffectCount(i);
            if (i < 9)
            {
                playerEffects += ",";
                enemyEffects += ",";
            }
        }

        playerEffectsText.SetText(playerEffects);
        enemyEffectsText.SetText(enemyEffects);
    }
}
