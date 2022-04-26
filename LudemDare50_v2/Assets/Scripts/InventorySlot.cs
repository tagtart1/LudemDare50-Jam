using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class InventorySlot : MonoBehaviour
{
    public Image icon;
    public TextMeshProUGUI labelText;
    public TextMeshProUGUI stackSizeText;
    public InventoryItem inventoryItem = null;
    public bool activated = false;
    [SerializeField] public bool isLeftHotbarSlot;
    [SerializeField] public bool isRightHotbarSlot;
   // [SerializeField] public bool isHotbarSlot;
    [SerializeField] public Inventory inventory;

    private void Awake()
    {
        
        ClearSlot();
        activated = false;
    }

    private void Start()
    {
      
    }

    private void OnEnable()
    {
       
    }

    private void OnDisable()
    {

    }
    public void ClearSlot()
    {
        icon.enabled = false;
        labelText.enabled = false;
        stackSizeText.enabled = false;
    }


    public void DrawSlot(InventoryItem item)
    {


        FillSlot();
        inventoryItem = item;
        icon.sprite = item.itemData.icon;
        labelText.text = item.itemData.displayName;
        stackSizeText.text = item.stackSize.ToString();


    }

    private void FillSlot()
    {
        icon.enabled = true;
        labelText.enabled = true;
        stackSizeText.enabled = true;
    }
}
