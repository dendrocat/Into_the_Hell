
using TMPro;
using UnityEngine;

public class BossHealthBarController : HealthBarController
{
    [Header("Bar's text parameters")]
    [SerializeField] TextMeshProUGUI _name;
    [SerializeField] TextMeshProUGUI _textHealth;


    public void SetBossName(string name)
    {
        _name.text = name;
    }

    protected override void SetTextHealth(float health, float maxHealth)
    {
        _textHealth.text = $"{Mathf.Ceil(health)} / {maxHealth}";
    }
}
