using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ClickEquip : MonoBehaviour, IPointerDownHandler 
{
    [SerializeField] InventorySlot hotBar1;
    [SerializeField] InventorySlot hotBar2;
   
  
  
   
    private bool isEquipped = false;


    private void OnEnable()
    {
        
    }

    private void Start()
    {
      
    }
    public void OnPointerDown(PointerEventData eventData)
    {
   
        if(eventData.button == PointerEventData.InputButton.Right)
        {

            ItemToHotBar(hotBar2);
        }
        else
        {
            
            ItemToHotBar(hotBar1);
        }
        
    }

    private void ItemToHotBar(InventorySlot hotbarSlot)
    {
        if (!isEquipped)
        {
            hotbarSlot.inventoryItem = GetComponent<InventorySlot>().inventoryItem;
            
             
        }
        else if (isEquipped)
        {
           
        }
    }

 
}
