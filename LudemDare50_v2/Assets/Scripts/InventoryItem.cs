using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class InventoryItem
{
    
    public ItemData itemData;
    public int stackSize;
   
    public InventoryItem(ItemData item, int amount)
    {
        itemData = item;
        AddToStack(amount);
    }

    public void AddToStack(int amount) 
    {
        stackSize += amount;
    }

    public void RemoveFromStack(int amount)
    {
        stackSize -= amount;
    }
}
