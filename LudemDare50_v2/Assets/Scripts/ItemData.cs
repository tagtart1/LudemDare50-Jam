using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class ItemData : ScriptableObject
{
    public enum ItemType
    {
        resource,
        tool
    }

    public ItemType itemType;
    public string displayName;
    public Sprite icon;
    public GameObject pickupPrefab;

}
