using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourcePickup : MonoBehaviour, ICollectible
{
    public static event HandleResourceCollected OnResourceCollected;
    public delegate void HandleResourceCollected(ItemData itemData, int amount);

    public static event HandleToolCollected OnToolCollected;
    public delegate void HandleToolCollected(ItemData itemData, int amount, float damage, float durability, float id);


    public int itemCount = 1;
    public ItemData resourceData;
    public bool isTool;
    public float damage;
    public float durability;
    [SerializeField] AudioClip pickupSFX;

    public float id;

    private void Awake()
    {
        id = GetInstanceID();
    }

    public void Collect()
    {
        SoundManager.PlayEffectSound_Static(pickupSFX);
        if (!isTool)
            OnResourceCollected?.Invoke(resourceData, itemCount);
        else
            OnToolCollected?.Invoke(resourceData, itemCount, damage, durability, id);
        Destroy(gameObject);
        
    }
}
