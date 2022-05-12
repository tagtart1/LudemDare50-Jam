using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CraftingBench : MonoBehaviour
{

    [SerializeField] private GameObject craftError;
    [SerializeField] private GameObject craftingMenu;
    [SerializeField] private Inventory inventory;
    [SerializeField] private AudioClip cannotCraftSFX;
    [SerializeField] private AudioClip craftSFX;
 
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
        if (canInteract && player.PressedInteract)
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
            player.GetCharacterPlane().localEulerAngles = Vector3.zero;
            isInteracting = true;
            craftingMenu.SetActive(true);
            inventory.ToggleInventoryMenu(true);
        }
    }

    public void CraftItem(CraftingRecipe craftingRecipe) 
    {
        if (craftingRecipe.ingredient2.itemData == null) 
        {
            if (CheckLeftHotbar(craftingRecipe) || CheckRightHotbar(craftingRecipe)) // checks to see if ingredient items are equipped
            {
               if (ingredient1Match.itemData != null )
                inventory.Remove(ingredient1Match.itemData, craftingRecipe.ingredient1.stackSize);
               else
                inventory.Remove(ingredient2Match.itemData, craftingRecipe.ingredient2.stackSize);

                SoundManager.PlayEffectSound_Static(craftSFX);
                 inventory.Add(craftingRecipe.itemToCreate.pickupPrefab.GetComponent<ResourcePickup>().resourceData, craftingRecipe.yield);
                
            }
            else
            {
                StopAllCoroutines();
                StartCoroutine(ErrorMessageUI());
            }
        }
        else if (CheckLeftHotbar(craftingRecipe) && CheckRightHotbar(craftingRecipe)) // checks to see if ingredient items are equipped
        {
            inventory.Remove(ingredient1Match.itemData, craftingRecipe.ingredient1.stackSize);
            inventory.Remove(ingredient2Match.itemData, craftingRecipe.ingredient2.stackSize);

            GameObject craftedItem = Instantiate(craftingRecipe.itemToCreate.pickupPrefab);
            Destroy(craftedItem, .1f);
            SoundManager.PlayEffectSound_Static(craftSFX);
            inventory.Add(craftedItem.GetComponent<ResourcePickup>().resourceData, craftingRecipe.yield, craftingRecipe.damage, craftingRecipe.durability, craftedItem.GetComponent<ResourcePickup>().id);
        }
        else
        {
            StopAllCoroutines();
            StartCoroutine(ErrorMessageUI());
        }
    }

    private IEnumerator ErrorMessageUI()
    {
        craftError.SetActive(true);
        SoundManager.PlayEffectSound_Static(cannotCraftSFX);
        yield return new WaitForSeconds(2f);
        craftError.SetActive(false);
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

    public void ToggleCraftingMenu(bool value)
    {
        craftingMenu.SetActive(value);
    }
}
