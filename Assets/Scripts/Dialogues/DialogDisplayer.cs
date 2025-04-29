using System.Collections;
using System.Collections.Generic;
using Ink.Runtime;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DialogDisplayer : MonoBehaviour
{
    Coroutine showPhraseCoroutine = null;
    public bool IsShowingPhrase => showPhraseCoroutine != null;

    [Header("Refs to display")]
    [SerializeField] TextMeshProUGUI _nameText;

    [SerializeField] TextMeshProUGUI _phraseText;

    [SerializeField] ChoiceDisplayer _choiceDisplayer;

    [Header("Display params")]
    [Range(0.01f, 1f)]
    [SerializeField] float _symbolDelayTime;


    private string _phrase;

    public void SetName(string name)
    {
        _nameText.text = name;
        _nameText.GetComponentInParent<ContentSizeFitter>();
    }

    public void ShowPhrase(string phrase)
    {
        _phraseText.text = "";
        _phrase = phrase;
        showPhraseCoroutine = StartCoroutine(ShowPhraseCoroutine());
    }

    IEnumerator ShowPhraseCoroutine()
    {
        for (int i = 0; _phraseText.text.Length < _phrase.Length; ++i)
        {
            _phraseText.text += _phrase[i];
            yield return new WaitForSecondsRealtime(_symbolDelayTime);
        }
        StopCoroutine(showPhraseCoroutine);
        showPhraseCoroutine = null;
    }

    public void ShowWholePhrase()
    {
        _phraseText.text = _phrase;
        StopCoroutine(showPhraseCoroutine);
        showPhraseCoroutine = null;
    }

    public void HandleTags(List<string> tags)
    {
        foreach (var tag in tags)
        {
            var stag = tag.Split(":");
            HandleTag(stag[0].Trim(), stag[1].Trim());
        }

    }

    void HandleTag(string tag, string value)
    {
        switch (tag)
        {
            case "name":
                SetName(value);
                break;
        }
    }

    public List<Button> CreateChoices(List<Choice> choices)
    {
        return _choiceDisplayer.CreateChoices(choices);
    }

    public void DestroyChoices()
    {
        _choiceDisplayer.DestroyChoices();
    }

}
