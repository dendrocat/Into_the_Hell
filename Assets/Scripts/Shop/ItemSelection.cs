using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemSelection : MonoBehaviour
{
    [SerializeField] Color _selectedColor;

    List<Button> _buttons;

    void Start()
    {
        _buttons = new();
        for (int i = 0; i < transform.childCount; ++i)
        {
            var button = transform.GetChild(i).GetComponent<Button>();
            button.onClick.AddListener(() => SelectItem(button));
            _buttons.Add(button);
        }
    }

    void ResetColors()
    {
        foreach (var button in _buttons)
            button.targetGraphic.color = button.colors.normalColor;
    }

    public void SelectItem(Button button)
    {
        //Debug.Log(button.gameObject.name);
        ResetColors();

        button.targetGraphic.color = _selectedColor;
    }
}
