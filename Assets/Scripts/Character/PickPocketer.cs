using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickPocketer : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField][Range(0, 5)] float range = 2;
    [SerializeField] int minCoins;
    [SerializeField] int maxCoins;

    Pocket[] Pockets => FindObjectsByType<Pocket>(FindObjectsSortMode.None);
    Inventory Inventory => GetComponent<Inventory>();
    public bool Check()
    {
        var closest = GetClosestPocket();

        return closest != null;
    }

    public void Poke()
    {
        var closest = GetClosestPocket();
        if (closest)
        {
            var coins = Random.Range(minCoins, maxCoins);
            var item = closest.Poke(coins);

            if (item)
            {
                Debug.Log("Yout got " + item.name + "!");
                Inventory.AddItem(item);
            }
        }
    }

    Pocket GetClosestPocket()
    {
        Pocket closest = null;
        float closestDistance = float.MaxValue;

        foreach (var pocket in Pockets)
        {
            if (pocket.empty)
                continue;

            var distance = Vector3.Distance(transform.position, pocket.transform.position);
            if (distance < range && distance < closestDistance)
            {
                closest = pocket;
                closestDistance = distance;
            }
        }

        return closest;
    }
}
