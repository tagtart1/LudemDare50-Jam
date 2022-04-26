using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Food : MonoBehaviour
{
    [SerializeField] private float hungerToRestore;
    private Player player;
    private StatBarHandler statBarHandler;

    private void Start()
    {
        player = GetComponentInParent<Player>();
        statBarHandler = GetComponentInParent<StatBarHandler>();
    }
    private void Update()
    {
        if (player.pressedInteract && !player.inventory.IsMenuActive())
        {
            ConsumeItem();
        }



    }

    private void ConsumeItem()
    {
        statBarHandler.IncrementStatBar(hungerToRestore, Stat.food);
        player.inventory.Remove(GetComponent<ResourcePickup>().resourceData, 1);
    }

}
