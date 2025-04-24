using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShiftController : MonoBehaviour
{
    [SerializeField] List<Image> _images;

    int _shiftIndex;

    Coroutine shiftReload;

    void Awake()
    {
        for (int i = 0; i < transform.childCount; ++i)
        {
            _images.Add(transform.GetChild(i).Find("ReloadImage").GetComponent<Image>());
        }
        foreach (var image in _images)
            image.fillAmount = 0;
        _shiftIndex = _images.Count - 1;
    }

    public void SetShiftCount(int shiftCount)
    {
        var len = _images.Count;
        for (int i = len; i > shiftCount; --i)
        {
            _images[i - 1].fillAmount = 1;
        }
        _shiftIndex = shiftCount - 1;
        if (shiftReload != null)
        {
            StopCoroutine(shiftReload);
            shiftReload = null;
        }
    }

    public void StartShiftSmoothReload(float reloadTime)
    {
        _shiftIndex++;
        shiftReload = StartCoroutine(ShiftSmoothed(reloadTime));
    }

    IEnumerator ShiftSmoothed(float time)
    {
        float t = 0;
        time -= time / 1000;
        while (t < 1)
        {
            _images[_shiftIndex].fillAmount = Mathf.Lerp(1, 0, t);
            t += Time.deltaTime / time;
            yield return null;
        }
        _images[_shiftIndex].fillAmount = 0;
    }
}
