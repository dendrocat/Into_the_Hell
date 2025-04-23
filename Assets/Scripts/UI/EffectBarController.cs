using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class EffectBarController : MonoBehaviour
{
    [SerializeField] GameObject _effectPrefab;

    [SerializeField] List<Image> _effects = new();

    Coroutine _animateCoroutine = null;

    Func<int> GetEffectCount;

    [SerializeField] int _effectCount;

    float _effectDuration = Mathf.NegativeInfinity;


    public void SetGetEffectCount(Func<int> func)
    {
        GetEffectCount = func;
    }

    public void SetEffectDuration(float duration)
    {
        _effectDuration = duration;
    }


    void ResetImagesState()
    {
        if (_animateCoroutine == null) return;
        StopCoroutine(_animateCoroutine);
        foreach (var item in _effects)
        {
            item.fillAmount = 1;
        }
    }

    IEnumerator AnimateEffectImage()
    {
        while (_effectCount > 0)
        {
            float t = 1;
            var image = _effects.Last();
            while (t > 0)
            {
                t -= Time.deltaTime / _effectDuration;
                image.fillAmount = t;
                yield return null;
            }

            _effects.Remove(image);
            Destroy(image.gameObject);
            --_effectCount;
        }
    }

    void SetEffectCount(int effectCount)
    {
        ResetImagesState();
        for (int i = 0; i < effectCount - _effectCount; i++)
        {
            var obj = Instantiate(_effectPrefab, transform);
            obj.name = obj.name + "_" + i;
            _effects.Add(obj.GetComponent<Image>());
        }
        _effectCount = effectCount;
        if (_effectDuration != Mathf.NegativeInfinity)
        {
            _animateCoroutine = StartCoroutine(AnimateEffectImage());
        }
    }

    void RemoveEffects(int effectCount)
    {
        if (_effectDuration != Mathf.NegativeInfinity) return;
        Debug.Log("Effects removed");
        for (int i = _effectCount - 1; i >= effectCount; --i)
        {
            Debug.Log(_effects[i].name);
            Destroy(_effects[i].gameObject);
        }
        Debug.Log($"Range index: {effectCount}, count: {_effectCount - effectCount}");
        _effects.RemoveRange(effectCount, _effectCount - effectCount);
        _effectCount = effectCount;
    }

    void Update()
    {
        var effectCount = GetEffectCount();
        if (_effectCount < effectCount)
        {
            SetEffectCount(effectCount);
        }
        else if (_effectCount > effectCount)
            RemoveEffects(effectCount);
    }
}
