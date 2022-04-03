using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourcePickup : MonoBehaviour, ICollectible
{
    public static event HandleResourceCollected OnResourceCollected;
    public delegate void HandleResourceCollected(ItemData itemData);

    public ItemData resourceData;


    public void Collect()
    {
     
        Destroy(gameObject);
        OnResourceCollected?.Invoke(resourceData);
    }
}
