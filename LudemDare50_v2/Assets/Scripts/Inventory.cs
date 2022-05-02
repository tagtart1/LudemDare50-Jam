using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Inventory : MonoBehaviour
{
    [SerializeField] Player player;

    public List<InventorySlot> inventorySlots = new List<InventorySlot>();
    public List<InventoryItem> inventory = new List<InventoryItem>();
    private Dictionary<ItemData, InventoryItem> itemDictionary = new Dictionary<ItemData, InventoryItem>();

    [SerializeField] private GameObject inventoryMenu;

    private bool isMenuActive = false;

    private void Start()
    {
        inventoryMenu.SetActive(false);
    }
    private void OnEnable()
    {
        ResourcePickup.OnResourceCollected += Add;
        ResourcePickup.OnToolCollected += Add;

    }

    private void OnDisable()
    {
        ResourcePickup.OnResourceCollected -= Add;
        ResourcePickup.OnToolCollected -= Add;

    }

    public void Add(ItemData itemData, int amount)
    {
       
        if (itemData.itemType != ItemData.ItemType.tool && itemDictionary.TryGetValue(itemData, out InventoryItem item))
        {
            
            item.AddToStack(amount);
            foreach(InventorySlot inventorySlot in inventorySlots)
            {
                if (inventorySlot.inventoryItem == item)
                {
                    inventorySlot.DrawSlot(item);
                }
            }
        }
        else
        {
            InventoryItem newItem = new InventoryItem(itemData, amount);
            
            if (FindEmptySlot(newItem))
            {
                inventory.Add(newItem); // add to list
                if (itemData.itemType != ItemData.ItemType.tool) itemDictionary.Add(itemData, newItem); //tools cannot be stacked so do not add to dictionary
            }
            else
            {
                player.CreateDroppedPickup(newItem);
            }
            

        }
    }

    public void Add(ItemData itemData, int amount, float damage, float durability, float id)
    {
         InventoryItem newItem = new InventoryItem(itemData, amount, damage, durability, id);

         if (FindEmptySlot(newItem))
         {
             inventory.Add(newItem); // add to list
             
         }
         else
         {
             player.CreateDroppedPickup(newItem);
         }
 
    }


    public void Remove(ItemData itemData, int amount) //called for crafting purposes
    {
        if (itemDictionary.TryGetValue(itemData, out InventoryItem item))
        {
            item.RemoveFromStack(amount);
            if (item.stackSize == 0)
            {
                player.UnequipItemInHand(item);
                inventory.Remove(item);
                itemDictionary.Remove(itemData);

                foreach (InventorySlot inventorySlot in inventorySlots)
                {
                    if (inventorySlot.inventoryItem == item)
                    {
                        inventorySlot.inventoryItem = null;
                        inventorySlot.activated = false;
                        inventorySlot.ClearSlot();
                        break;
                    }
                }
            }
            else
            {
                foreach (InventorySlot inventorySlot in inventorySlots)
                {
                    if (inventorySlot.inventoryItem == item)
                    {
                        inventorySlot.DrawSlot(item);
                        break;
                    }
                }
            }
        }
    }

    public void HandleToolItem(float id)
    {
        
        foreach(InventoryItem inventoryItem in inventory.ToArray())
        {
            if (inventoryItem.id == id)
            {
                inventoryItem.DecreaseDurability(1f);
                foreach (InventorySlot inventorySlot in inventorySlots)
                {

                    if (inventorySlot.inventoryItem == inventoryItem)
                    {
                        if (inventoryItem.durability <= 0)
                        {
                            inventory.Remove(inventoryItem);
                            player.UnequipItemInHand(inventoryItem);
                            inventorySlot.inventoryItem = null;
                            inventorySlot.activated = false;
                            inventorySlot.ClearSlot();

                            break;
                        }
                        else
                        {
                            inventorySlot.DrawSlot(inventoryItem);
                        }
                        
                    }
                }

                           
            }
        }

    }

    public void DropItem(InventorySlot inventorySlot)
    {
        if (inventorySlot.inventoryItem.itemData.itemType == ItemData.ItemType.tool) //avoids dictionary check
        {
            foreach(InventoryItem inventoryItem in inventory)
            {
                if (inventoryItem == inventorySlot.inventoryItem)
                {                      
                   inventory.Remove(inventoryItem);
                   inventorySlot.inventoryItem = null;
                   inventorySlot.activated = false;
                   inventorySlot.ClearSlot();          
                   break;
                }
            }
        }
        else if (itemDictionary.TryGetValue(inventorySlot.inventoryItem.itemData, out InventoryItem item))
        {
            inventory.Remove(item);
            itemDictionary.Remove(inventorySlot.inventoryItem.itemData);
            inventorySlot.inventoryItem = null;
            inventorySlot.activated = false;
            inventorySlot.ClearSlot();        
        }
    }

    public bool IsMenuActive()
    {
        return isMenuActive;
    }

    public void ToggleInventoryMenu()
    {
        if (!inventoryMenu.activeInHierarchy)
        {
            inventoryMenu.SetActive(true);
            isMenuActive = true;
        }
        else
        {
            TooltipDynamic.HideTooltip_Static();
            inventoryMenu.SetActive(false);
            isMenuActive = false;
        }
    }

    public void ToggleInventoryMenu(bool value)
    {
        inventoryMenu.SetActive(value);
        isMenuActive = value; 
        
    }

    private bool FindEmptySlot(InventoryItem newItem)
    {
        foreach (InventorySlot inventorySlot in inventorySlots)
        {
            if (inventorySlot.activated == false)
            {
                inventorySlot.activated = true;
                inventorySlot.inventoryItem = newItem;
                inventorySlot.DrawSlot(newItem);

                return true;
            }
           
        }
        return false;
    }
}
