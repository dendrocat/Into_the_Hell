using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ItemsPanel : MonoBehaviour
{
    protected struct Item
    {
        public TextMeshProUGUI cost;
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
            item.cost = t.Find("ContainerCost").GetComponentInChildren<TextMeshProUGUI>();
            item.button = t.GetComponentInChildren<Button>();
            item.maxText = t.Find("MaxText").gameObject;

            _items[t.name] = item;
        }
    }

    public void SetItemCost(string key, int cost)
    {
        _items[key].cost.text = cost.ToString();
    }

    public void CalcItemsState(Func<int, bool> predicate)
    {
        foreach (var item in _items.Values)
        {
            item.button.interactable =
            predicate(Convert.ToInt32(item.cost.text));
        }
    }

    public void RemoveItem(string key)
    {
        _items[key].button.interactable = false;
        _items[key].maxText.SetActive(true);
        Destroy(_items[key].cost.transform.parent.gameObject);
        _items.Remove(key);
    }
}
