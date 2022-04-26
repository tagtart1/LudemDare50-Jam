using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CraftingSlot : MonoBehaviour
{
    [SerializeField] CraftingRecipe craftingRecipe;
    [Header("Ingredient 1 UI")]
    [SerializeField] Image ingredient1;
    [SerializeField] TextMeshProUGUI ingredient1Label;
    [SerializeField] TextMeshProUGUI ingredient1StackSize;
    [Header("Ingredient 1 UI")]
    [SerializeField] Image ingredient2;
    [SerializeField] TextMeshProUGUI ingredient2Label;
    [SerializeField] TextMeshProUGUI ingredient2StackSize;
    public Image resultIcon;

    public TextMeshProUGUI labelText;
    


    private void Start()
    {
        DrawCraftSlot();
    }

    private void DrawCraftSlot()
    {
        resultIcon.sprite = craftingRecipe.itemToCreate.icon;
        labelText.text = (craftingRecipe.yield+ "x     " + craftingRecipe.itemToCreate.displayName);

        ingredient1.sprite = craftingRecipe.ingredient1.itemData.icon;
        ingredient1Label.text = craftingRecipe.ingredient1.itemData.displayName;
        ingredient1StackSize.text = craftingRecipe.ingredient1.stackSize.ToString();

        ingredient2.sprite = craftingRecipe.ingredient2.itemData.icon;
        ingredient2Label.text = craftingRecipe.ingredient2.itemData.displayName;
        ingredient2StackSize.text = craftingRecipe.ingredient2.stackSize.ToString();
    }

    
}
