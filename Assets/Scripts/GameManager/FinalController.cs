using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class FinalController : MonoBehaviour
{
    [Range(0f, 2f)]
    [SerializeField] int delayStartCredits;
    [Range(10, 100)]
    [SerializeField] int creditsTime;

    [SerializeField] Scrollbar _scroll;


    IEnumerator Start()
    {
        _scroll.value = 1;
        yield return new WaitForSecondsRealtime(delayStartCredits);
        yield return StartCoroutine(LoadInitScene());
    }

    IEnumerator LoadInitScene()
    {
        float t = 1;
        while (t > 0f)
        {
            t -= Time.deltaTime / creditsTime;
            _scroll.value = t;
            yield return null;
        }
        yield return new WaitForSecondsRealtime(1f);
        GameManager.Instance.ReloadGame();
    }
}
