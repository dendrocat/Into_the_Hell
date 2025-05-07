using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

[RequireComponent(typeof(AudioSource))]
public class IntroController : MonoBehaviour
{
    [Serializable]
    struct ObjectDuration
    {
        public GameObject gameObject;

        [Range(0f, 1f)]
        public float durationPart;
    }

    [Header("Intro canvas")]
    [SerializeField] Image _mask;
    [SerializeField] List<ObjectDuration> _introObjects;

    [Header("Intro Parameters")]
    [SerializeField] float delayBetweenObjects;

    void Awake()
    {
        StartCoroutine(ShowIntro(GetComponent<AudioSource>().clip.length));
    }

    IEnumerator ChangeMaskState(float duration)
    {
        var color = _mask.color;
        duration /= 2;
        float t = 1f;
        while (t > 0)
        {
            t -= Time.deltaTime / duration;
            color.a = t;
            _mask.color = color;
            yield return null;
        }
        color.a = 0;
        t = 0;
        while (t < 1f)
        {
            t += Time.deltaTime / duration;
            color.a = t;
            _mask.color = color;
            yield return null;
        }
        color.a = 1;
    }

    IEnumerator ShowIntro(float duration)
    {
        yield return null;
        duration -= delayBetweenObjects * (_introObjects.Count - 1);
        foreach (var item in _introObjects)
        {
            item.gameObject.SetActive(true);
            var itemDuration = duration * item.durationPart;
            StartCoroutine(ChangeMaskState(itemDuration));
            yield return new WaitForSeconds(itemDuration + delayBetweenObjects);
            item.gameObject.SetActive(false);
        }
        SceneManager.LoadScene(1);
    }
}
