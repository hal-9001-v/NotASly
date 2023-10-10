using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public int CurrentMoney { get; private set; }

    [SerializeField] List<InventoryItem> items;

    /// <summary>
    /// Current money and change
    /// </summary>
    public Action<int, int> OnMoneyChange;

    public void ChangeMoney(int amount)
    {
        CurrentMoney += amount;

        OnMoneyChange?.Invoke(CurrentMoney, amount);
    }

    public void AddItem(InventoryItem item)
    {
        items.Add(item);
    }

    public bool HasItem(InventoryItem item)
    {
        return items.Contains(item);
    }

    public void RemoveItem(InventoryItem item)
    {
        items.Remove(item);
    }

}
