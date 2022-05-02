using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class InventorySlot : MonoBehaviour
{
    public Image icon;
    public Slider durabilitySlider;
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
        durabilitySlider.gameObject.SetActive(false);
        stackSizeText.enabled = false;
    }


    public void DrawSlot(InventoryItem item)
    {
        if (item.durability != 0)
        {
            durabilitySlider.gameObject.SetActive(true);
            durabilitySlider.value = item.durability / 20f;
        }
        else
        {
            durabilitySlider.gameObject.SetActive(false);
        }

        FillSlot();
        inventoryItem = item;
        icon.sprite = item.itemData.icon;
        
        stackSizeText.text = item.stackSize.ToString();


    }

    private void FillSlot()
    {
        icon.enabled = true;
        
        stackSizeText.enabled = true;
    }
}
