using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class InventoryCheck : MonoBehaviour
{
    public UnityEvent OnSuccessEvent;
    public UnityEvent OnFailEvent;

    [SerializeField] InventoryItem[] items;

    Inventory Inventory => FindObjectOfType<Inventory>();

    [ContextMenu("Check")]
    public void Check()
    {
        foreach (var item in items)
        {
            if (Inventory.HasItem(item) == false)
            {
                OnFailEvent?.Invoke();
                return;
            }
        }

        OnSuccessEvent?.Invoke();
    }

}
