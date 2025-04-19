using System.Collections.Generic;
using Ink.Runtime;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ChoiceDisplayer : MonoBehaviour
{
    void Start()
    {
        DestroyChoices();
    }

    [SerializeField] GameObject _buttonPrefab;

    public List<Button> CreateChoices(List<Choice> choices)
    {
        var buttons = new List<Button>();
        foreach (var choice in choices)
        {
            var button = Instantiate(_buttonPrefab, transform);
            button.GetComponentInChildren<TextMeshProUGUI>().text = choice.text;
            buttons.Add(button.GetComponent<Button>());
        }
        return buttons;
    }

    public void DestroyChoices()
    {
        for (int i = 0; i < transform.childCount; ++i)
            Destroy(transform.GetChild(i).gameObject);
    }
}
