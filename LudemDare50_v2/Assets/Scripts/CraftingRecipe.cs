using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu]
public class CraftingRecipe : ScriptableObject
{
    public ItemData itemToCreate;
    public int yield;
    public InventoryItem ingredient1;
    public InventoryItem ingredient2;
}

