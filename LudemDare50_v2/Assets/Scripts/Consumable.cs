using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Consumable : MonoBehaviour
{
    [SerializeField] private float restoreAmount;
    [SerializeField] Stat statToRestore;
    private Player player;
    private StatBarHandler statBarHandler;

    private void Start()
    {
        player = GetComponentInParent<Player>();
        statBarHandler = GetComponentInParent<StatBarHandler>();
    }
    private void Update()
    {
        if (player.PressedInteract && !player.inventory.IsMenuActive())
        {
            ConsumeItem();
        }



    }

    private void ConsumeItem()
    {
        statBarHandler.IncrementStatBar(restoreAmount, statToRestore);
        player.inventory.Remove(GetComponent<ResourcePickup>().resourceData, 1);
    }

}
