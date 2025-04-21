using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShiftController : MonoBehaviour
{
    [SerializeField] List<Image> _images;

    int _shiftCount;


    void Start()
    {
        for (int i = 0; i < transform.childCount; ++i)
        {
            _images.Add(transform.GetChild(i).GetChild(0).GetComponent<Image>());
        }
        foreach (var image in _images)
            image.fillAmount = 1;
    }

    public void SetShiftCount(int shiftCount)
    {
        _shiftCount = shiftCount;
        for (int i = 0; i < shiftCount; ++i)
        {
            _images[i].fillAmount = 0;
        }
    }

    public void SetShiftCountSmoothed(int shiftCount)
    {

    }
}
