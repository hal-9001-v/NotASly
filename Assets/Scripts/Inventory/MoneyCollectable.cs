using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collectable))]
public class MoneyCollectable : MonoBehaviour
{
    [SerializeField] [Range(1, 100)] int moneyValue = 1;
    Collectable Collectable => GetComponent<Collectable>();

    private void Start()
    {
        Collectable.OnCollectCallback += Collect;
    }

    void Collect(Collector collector)
    {
        var inventory = collector.GetComponent<Inventory>();
        if (inventory)
        {
            inventory.ChangeMoney(moneyValue);
        }
    }
}
