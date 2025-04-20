using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemSelection : MonoBehaviour
{
    [SerializeField] Color _selectedColor;

    [SerializeField] List<Button> _buttons;

    void Start()
    {
        for (int i = 0; i < transform.childCount; ++i)
            _buttons.Add(transform.GetChild(i).GetComponent<Button>());
    }

    void ResetColors()
    {
        foreach (var button in _buttons)
            button.targetGraphic.color = button.colors.normalColor;
    }

    public void SelectItem(Button button)
    {
        ResetColors();

        button.targetGraphic.color = _selectedColor;
    }
}
