using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    [Header("Animated Parameters")]
    [SerializeField] HealthBarController _healthBar;
    public HealthBarController HealthBar => _healthBar;

    [SerializeField] ShiftController _shiftController;
    public ShiftController ShiftController => _shiftController;

    [SerializeField] PotionController _potionController;
    public PotionController PotionController => _potionController;

    [SerializeField] BossBarController _bossBarController;
    public BossBarController BossBarController => _bossBarController;

    [Header("Text Parameters")]
    [SerializeField] TextMeshProUGUI _arrows;
    [SerializeField] TextMeshProUGUI _money;

    [Header("Weapon Parameters")]
    [SerializeField] Image _weaponImage;
    [SerializeField] List<Sprite> _weaponImages;

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
        int sign = Convert.ToInt32(Mathf.Sign(money - start));
        while (start != money)
        {
            start += sign;
            _money.text = start.ToString();
            yield return null;
        }
    }

    public void ChangeWeaponImage(WeaponType type)
    {
        _weaponImage.sprite = _weaponImages[(int)type];
    }

}
