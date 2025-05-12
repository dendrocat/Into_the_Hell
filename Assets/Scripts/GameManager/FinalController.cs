using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FinalController : MonoBehaviour
{
    [Range(0f, 2f)]
    [SerializeField] int delayStartCredits;
    [Range(10, 100)]
    [SerializeField] int creditsTime;

    [SerializeField] Scrollbar _scroll;

    [SerializeField] List<LayoutElement> fullHeightElements;

    IEnumerator Start()
    {
        fullHeightElements.ForEach(e => e.preferredHeight = Screen.height);

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
