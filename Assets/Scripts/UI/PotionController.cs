using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PotionController : MonoBehaviour
{

    [SerializeField] TextMeshProUGUI _potions;

    [SerializeField] Image _imageReload;

    void Awake()
    {
        _imageReload.fillAmount = 0;
    }


    public void SetPotions(int potionCount)
    {
        _potions.text = potionCount.ToString();
    }

    public void StartReload(float reloadTime)
    {
        _imageReload.fillAmount = 1;
        StartCoroutine(ReloadCoroutine(reloadTime));
    }

    IEnumerator ReloadCoroutine(float time)
    {
        float t = 0;
        while (t < 1)
        {
            _imageReload.fillAmount = Mathf.Lerp(1, 0, t);
            t += Time.deltaTime / time;
            yield return null;
        }
        _imageReload.fillAmount = 0;
    }
}
