using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryItem : MonoBehaviour
{
    Inventory Inventory => FindObjectOfType<Inventory>();

    [ContextMenu("Add to inventory")]
    public void AddToInventory()
    {
        Inventory.AddItem(this);
    }

}
