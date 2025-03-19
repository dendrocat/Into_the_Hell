using System.Collections;
using UnityEngine;

public class TestScript : MonoBehaviour
{

    IEnumerator checkAttack()
    {
        float t = 1f;
        yield return new WaitForSecondsRealtime(t);
        for (int i = 0; i < 10; ++i)
        {
            Debug.Log($"Attack: {InputManager.Instance.attack}");
            yield return new WaitForSecondsRealtime(t);
            Debug.Log($"After {t} seconds");
            Debug.Log($"Attack: {InputManager.Instance.attack}");
        }

        yield return null;
    }

    void Start()
    {
        StartCoroutine(checkAttack());
    }

    void Update()
    {
    }
}
