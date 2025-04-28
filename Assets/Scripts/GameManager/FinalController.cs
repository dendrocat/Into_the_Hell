using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class FinalController : MonoBehaviour
{
    [SerializeField] float delayBeforeRestart;

    [SerializeField] Image _progressImage;

    void Start()
    {
        StartCoroutine(LoadInitScene());
    }

    IEnumerator LoadInitScene()
    {
        yield return null;
        float t = 0;
        while (t < 1f)
        {
            t += Time.deltaTime / delayBeforeRestart;
            _progressImage.fillAmount = t;
            yield return null;
        }
        Destroy(DontDestroyManager.Instance.gameObject);
    }
}
