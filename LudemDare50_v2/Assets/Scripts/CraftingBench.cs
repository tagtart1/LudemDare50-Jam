using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CraftingBench : MonoBehaviour
{


    [SerializeField] private GameObject craftingMenu;
    [SerializeField] private Inventory inventory;

    InventoryItem ingredient1Match = null;
    InventoryItem ingredient2Match = null;

    private bool isInteracting = false;
    private bool canInteract;
    Player player;

    private void Start()
    {
        player = FindObjectOfType<Player>();
        craftingMenu.SetActive(false);
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<Player>())
        {
            Debug.Log("triggered");
            canInteract = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.GetComponent<Player>()) return;
            
        canInteract = false;
        isInteracting = false;
        craftingMenu.SetActive(false);
        if (craftingMenu.activeInHierarchy) 
        inventory.ToggleInventoryMenu(false);
    }

    private void Update()
    {
        if (canInteract && player.pressedInteract)
        {
            Interact();
        }
    }

    private void Interact()
    {
        if (isInteracting)
        {
            isInteracting = false;
            craftingMenu.SetActive(false);
            inventory.ToggleInventoryMenu(false);
        }
        else
        {
            isInteracting = true;
            craftingMenu.SetActive(true);
            inventory.ToggleInventoryMenu(true);
        }
    }

    public void CraftItem(CraftingRecipe craftingRecipe)
    {
        if (CheckLeftHotbar(craftingRecipe) && CheckRightHotbar(craftingRecipe)) // checks to see if ingredient items are equipped
        {
            inventory.Remove(ingredient1Match.itemData, craftingRecipe.ingredient1.stackSize);
            inventory.Remove(ingredient2Match.itemData, craftingRecipe.ingredient2.stackSize);

            inventory.Add(craftingRecipe.itemToCreate, craftingRecipe.yield);
        }
        else
        {
            Debug.Log("equip items to craft"); 
        }
    }

    private bool CheckLeftHotbar(CraftingRecipe craftingRecipe)
    {
        foreach (InventorySlot inventorySlot in inventory.inventorySlots)
        {
            if (inventorySlot.isLeftHotbarSlot && inventorySlot.activated)
            {
                if (inventorySlot.inventoryItem.itemData == craftingRecipe.ingredient1.itemData  )
                {
                    if (inventorySlot.inventoryItem.stackSize >= craftingRecipe.ingredient1.stackSize)
                    {
                        ingredient1Match = inventorySlot.inventoryItem;
                        return true;
                    }
                }
                else if (inventorySlot.inventoryItem.itemData == craftingRecipe.ingredient2.itemData)
                {
                   if ( inventorySlot.inventoryItem.stackSize >= craftingRecipe.ingredient2.stackSize)
                    {
                        ingredient2Match = inventorySlot.inventoryItem;
                        return true;
                    }
                }
               
            }
        }
       
        return false;
    }
    private bool CheckRightHotbar(CraftingRecipe craftingRecipe)
    {
        foreach (InventorySlot inventorySlot in inventory.inventorySlots)
        {
            if (inventorySlot.isRightHotbarSlot && inventorySlot.activated)
            {
                if (inventorySlot.inventoryItem.itemData == craftingRecipe.ingredient1.itemData)
                {
                    if (inventorySlot.inventoryItem.stackSize >= craftingRecipe.ingredient1.stackSize)
                    {
                        ingredient1Match = inventorySlot.inventoryItem;
                        return true;
                    }
                }
                else if (inventorySlot.inventoryItem.itemData == craftingRecipe.ingredient2.itemData)
                {
                    if (inventorySlot.inventoryItem.stackSize >= craftingRecipe.ingredient2.stackSize)
                    {
                        ingredient2Match = inventorySlot.inventoryItem;
                        return true;
                    }
                }

            }
        }
        
        return false;
    }
}
