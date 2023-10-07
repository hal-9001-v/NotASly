using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public int CurrentMoney { get; private set; }

    /// <summary>
    /// Current money and change
    /// </summary>
    public Action<int, int> OnMoneyChange;

    public void ChangeMoney(int amount)
    {
        CurrentMoney += amount;

        OnMoneyChange?.Invoke(CurrentMoney, amount);
    }

}
