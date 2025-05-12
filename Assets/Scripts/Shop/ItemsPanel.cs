using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Панель для отображения и управления набором предметов с их стоимостью, количеством и состоянием.
/// </summary>
public class ItemsPanel : MonoBehaviour
{
    /// <summary>
    /// Структура для хранения UI-элементов предмета.
    /// </summary>
    protected struct Item
    {
        public TextMeshProUGUI cost;
        public TextMeshProUGUI count;
        public Button button;
        public GameObject maxText;
    }

    Dictionary<string, Item> _items;

    void Awake()
    {
        _items = new();
        for (int i = 0; i < transform.childCount; i++)
        {
            var t = transform.GetChild(i);
            Item item = new();
            item.count = t.Find("Count")?.GetComponent<TextMeshProUGUI>();
            item.cost = t.Find("ContainerCost").GetComponentInChildren<TextMeshProUGUI>();
            item.button = t.GetComponentInChildren<Button>();
            item.maxText = t.Find("MaxText").gameObject;

            _items[t.name] = item;
        }
    }

    /// <summary>
    /// Устанавливает отображаемое количество предмета по ключу.
    /// </summary>
    /// <param name="key">Ключ предмета.</param>
    /// <param name="count">Количество для отображения.</param>
    public void SetItemCount(string key, int count)
    {
        if (_items[key].count == null) return;
        _items[key].count.text = count.ToString();
    }

    /// <summary>
    /// Устанавливает отображаемую стоимость предмета по ключу.
    /// </summary>
    /// <param name="key">Ключ предмета.</param>
    /// <param name="cost">Стоимость для отображения.</param>
    public void SetItemCost(string key, int cost)
    {
        _items[key].cost.text = cost.ToString();
    }

    /// <summary>
    /// Вычисляет состояние доступности предметов, используя переданный предикат.
    /// </summary>
    /// <param name="predicate">Функция, принимающая стоимость и ключ, возвращающая true, если предмет доступен.</param>
    public void CalcItemsState(Func<int, string, bool> predicate)
    {
        foreach (var p in _items)
        {
            p.Value.button.interactable =
            predicate(Convert.ToInt32(p.Value.cost.text), p.Key);
        }
    }

    /// <summary>
    /// Удаляет предмет из панели, делая кнопку неактивной и показывая текст "Максимум".
    /// </summary>
    /// <param name="key">Ключ предмета для удаления.</param>
    public void RemoveItem(string key)
    {
        _items[key].button.interactable = false;
        _items[key].maxText.SetActive(true);
        Destroy(_items[key].cost.transform.parent.gameObject);
        _items.Remove(key);
    }

    /// <summary>
    /// Деактивирует предмет, делая кнопку неактивной, но не удаляет UI-элементы.
    /// </summary>
    /// <param name="key">Ключ предмета для деактивации.</param>
    public void DeactivateItem(string key)
    {
        _items[key].button.interactable = false;
        _items.Remove(key);
    }
}
