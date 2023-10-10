using System;
using UnityEngine;

public class Pocket : MonoBehaviour
{
    public bool canBePocketed = true;
    public bool empty { get; private set; }

    [SerializeField] PocketItem[] items;

    int currentItemIndex;


    [SerializeField] Spawner spawner;

    int remainingMoney;
    InventoryItem currentItem;

    private void Start()
    {
        remainingMoney = items[currentItemIndex].moneyCount;
    }

    public InventoryItem Poke(int coins)
    {
        if (empty)
            return null;

        InventoryItem item = null;

        if (remainingMoney <= 0)
        {
            item = items[currentItemIndex].item;
            SetNextItem();
        }
        else
        {
            var coinsToSpawn = Mathf.Min(coins, remainingMoney);
            remainingMoney -= coinsToSpawn;
            if (remainingMoney <= 0)
                remainingMoney = 0;

            spawner.SpawnObjects(coinsToSpawn);
        }

        return item;
    }

    [ContextMenu("Poke")]
    InventoryItem Poke()
    {
        return Poke(UnityEngine.Random.Range(1, 3));
    }

    void SetNextItem()
    {
        currentItemIndex++;
        if (currentItemIndex >= items.Length)
        {
            empty = true;

        }
        else
        {
            remainingMoney = items[currentItemIndex].moneyCount;
        }
    }

    [Serializable]
    class PocketItem
    {
        public InventoryItem item;
        public int moneyCount;
    }
}
