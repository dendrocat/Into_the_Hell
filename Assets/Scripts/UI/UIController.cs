using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    [Header("Bar Parameters")]
    [SerializeField] HealthBarController _healthBar;
    public HealthBarController HealthBar => _healthBar;

    [SerializeField] ShiftController _shiftController;
    public ShiftController ShiftController => _shiftController;

    [Header("Text Parameters")]
    [SerializeField] TextMeshProUGUI _potions;
    [SerializeField] TextMeshProUGUI _arrows;
    [SerializeField] TextMeshProUGUI _money;

    public void SetArrows(int arrows)
    {
        _arrows.text = arrows.ToString();
    }

    public void SetMoney(int money)
    {
        _money.text = money.ToString();
    }

    Coroutine _smoothMoney;

    public void SetMoneySmooth(int money)
    {
        if (_smoothMoney != null)
            StopCoroutine(_smoothMoney);
        _smoothMoney = StartCoroutine(SmoothMoney(money));
    }

    IEnumerator SmoothMoney(int money)
    {
        int start = Convert.ToInt32(_money.text);
        while (start < money)
        {
            ++start;
            _money.text = start.ToString();
            yield return null;
        }
    }

    public void SetPotions(int potions)
    {
        _potions.text = potions.ToString();
    }
}
