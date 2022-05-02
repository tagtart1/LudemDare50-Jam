using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class InventoryItem
{
    
    public ItemData itemData;
    public int stackSize;

    public float damage;
    public float durability;

    public float id;
   
    public InventoryItem(ItemData item, int amount)
    {
        itemData = item;
        AddToStack(amount);
    }

    public InventoryItem(ItemData item, int amount, float damage, float durability, float id)
    {
        itemData = item;
        AddToStack(amount);
        this.damage = damage;
        this.durability = durability;
        this.id = id;
    }

    public void AddToStack(int amount) 
    {
        stackSize += amount;
    }

    public void RemoveFromStack(int amount)
    {
        stackSize -= amount;
    }

    public void DecreaseDurability(float amount)
    {
        durability -= amount;
    }
}
