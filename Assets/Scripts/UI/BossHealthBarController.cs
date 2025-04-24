
using System;
using TMPro;
using UnityEngine;

public class BossHealthBarController : HealthBarController
{
    [Header("Bar's text parameters")]
    [SerializeField] TextMeshProUGUI _name;
    [SerializeField] TextMeshProUGUI _textHealth;

    [Header("Color Parameters")]
    [SerializeField] Color _miniGolemColor;

    Color _startColor;

    Func<bool> predicateEffect;

    void Awake()
    {
        _startColor = _healthImage.color;
    }

    public void SetBossName(string name)
    {
        _name.text = name;
    }

    protected override void SetTextHealth(float health, float maxHealth)
    {
        _textHealth.text = $"{Mathf.Ceil(health)} / {maxHealth}";
    }

    public void SetPredicateColorChange(Func<bool> func)
    {
        predicateEffect = func;
    }

    void Update()
    {
        if (predicateEffect())
            _healthImage.color = _miniGolemColor;
        else _healthImage.color = _startColor;
    }
}
