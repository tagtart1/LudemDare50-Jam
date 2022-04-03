using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ClickEquip : MonoBehaviour, IPointerDownHandler 
{
    [SerializeField] Transform hotbar;
    private InventoryManager inventoryManager;
   
    private bool isEquipped;

       

    private void Start()
    {
        inventoryManager = GetComponentInParent<InventoryManager>();
    }
    public void OnPointerDown(PointerEventData eventData)
    {
        if(eventData.button == PointerEventData.InputButton.Right)
        {
            if (!isEquipped && inventoryManager.equippedItems < 2)
            {
                gameObject.transform.SetParent(hotbar);
                isEquipped = true;
                inventoryManager.equippedItems++;
            }
            else if (isEquipped)
            {
                isEquipped = false;
                inventoryManager.equippedItems--;
                gameObject.transform.SetParent(inventoryManager.transform);
            }
        }
        else
        {
            //gameObject.transform.SetParent(hotbar);
        }
        
    }

 
}
