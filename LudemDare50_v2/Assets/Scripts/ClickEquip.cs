using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ClickEquip : MonoBehaviour, IPointerDownHandler , IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] Player player;

    public static event HandleDropItem OnDropItem;
    public delegate void HandleDropItem(ItemData itemData);

    Inventory inventory;
    InventorySlot _inventorySlot;
    private bool isHoveringOnItem = false;
    private bool tooltipActive = false;
  

    private void Awake()
    {
        _inventorySlot = GetComponent<InventorySlot>();
        inventory = _inventorySlot.inventory;
    }

    private void OnEnable()
    {
        isHoveringOnItem = false;
    }

    private void Update()
    {
       
        if (isHoveringOnItem && player.PressedDropItem && _inventorySlot.activated) // drop item from inventory
        {
            if (_inventorySlot.isLeftHotbarSlot || _inventorySlot.isRightHotbarSlot)
                player.UnequipItemInHand(_inventorySlot.inventoryItem);
       
          

            isHoveringOnItem = false;
            player.CreateDroppedPickup(_inventorySlot.inventoryItem);
            inventory.DropItem(_inventorySlot);
        }

        if (isHoveringOnItem && _inventorySlot.activated && !tooltipActive)
        {
           
            tooltipActive = true;
            TooltipDynamic.ShowToolTip_Static(_inventorySlot.inventoryItem.itemData.displayName);
        }
        else if ((!isHoveringOnItem && tooltipActive)   || ( tooltipActive && !_inventorySlot.activated))
        {
           
            tooltipActive = false;
            TooltipDynamic.HideTooltip_Static();
        }


    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (!inventory.IsMenuActive()) return;

        if(eventData.button == PointerEventData.InputButton.Right)
        {
            if (_inventorySlot.isRightHotbarSlot && _inventorySlot.activated) //uniquipping
            {
                UnequipItem(_inventorySlot.inventoryItem);
            }
            else if (_inventorySlot.activated && !_inventorySlot.isLeftHotbarSlot)
            {
                foreach (InventorySlot inventorySlot in inventory.inventorySlots)
                {
                    if (inventorySlot.isRightHotbarSlot )
                    {
                        if (!inventorySlot.activated)  //equip into empty slot
                        {
                            PutItemToSlot(inventorySlot);
                            
                            player.EquipItemToHand(inventorySlot.inventoryItem, false);
                            break;
                        }
                        else  //switches items
                        {
                            SwitchItems(inventorySlot);
                            player.EquipItemToHand(inventorySlot.inventoryItem, false);
                            break;
                        }
                    }
                }
            }
        }
        else
        {
            if (_inventorySlot.isLeftHotbarSlot && _inventorySlot.activated ) //uniquipping
            {
                UnequipItem(_inventorySlot.inventoryItem);
            }
            else if (_inventorySlot.activated && !_inventorySlot.isRightHotbarSlot)
            {
                foreach (InventorySlot inventorySlot in inventory.inventorySlots)
                {
                    if (inventorySlot.isLeftHotbarSlot)
                    {
                        if (!inventorySlot.activated)  //equip into empty slot
                        {
                            PutItemToSlot(inventorySlot);
                            player.EquipItemToHand(inventorySlot.inventoryItem, true);
                            break;
                        }
                        else  //switches items
                        {
                            SwitchItems(inventorySlot);
                            player.EquipItemToHand(inventorySlot.inventoryItem, true);
                            break;
                        }
                    }
                }
            }
        }
        
    }

    private void UnequipItem(InventoryItem itemToUnequip)
    {
        foreach (InventorySlot inventorySlot in inventory.inventorySlots)
        {
            if (!inventorySlot.activated && !inventorySlot.isLeftHotbarSlot && !inventorySlot.isRightHotbarSlot)
            {
               
                player.UnequipItemInHand(itemToUnequip);
                PutItemToSlot(inventorySlot);
                break;
            }
        }
    }


    private void SwitchItems(InventorySlot inventorySlot)
    {
        InventoryItem tempItem = _inventorySlot.inventoryItem;
        _inventorySlot.inventoryItem = inventorySlot.inventoryItem;
        _inventorySlot.DrawSlot(inventorySlot.inventoryItem);
        
        inventorySlot.inventoryItem = tempItem;
        inventorySlot.DrawSlot(tempItem);
        TooltipDynamic.ShowToolTip_Static(_inventorySlot.inventoryItem.itemData.displayName);

    }

    private void PutItemToSlot(InventorySlot inventorySlot)
    {
        inventorySlot.activated = true;
        inventorySlot.inventoryItem = _inventorySlot.inventoryItem;
        inventorySlot.DrawSlot(inventorySlot.inventoryItem);
        _inventorySlot.inventoryItem = null;
        _inventorySlot.activated = false;
        _inventorySlot.ClearSlot();
    }

 
    public void OnPointerEnter(PointerEventData eventData)
    {
       
        isHoveringOnItem = true;
    
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        isHoveringOnItem = false;
       
    }

   
}
