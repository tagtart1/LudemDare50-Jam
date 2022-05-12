using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TemperatureControl : MonoBehaviour
{
    [SerializeField] private float temperaturePerSecond;
    [SerializeField] private float durabilityLoss;
    private StatBarHandler statBarHandler;
    private Player player;

    private void Start()
    {
        statBarHandler = GetComponentInParent<StatBarHandler>();
        player = GetComponentInParent<Player>();
    }


    private void Update()
    {
        if (!player.inventory.IsMenuActive())
        {
            statBarHandler.IncrementStatBar(temperaturePerSecond, Stat.temperature);
            player.inventory.HandleToolItem(GetComponent<ResourcePickup>().id, durabilityLoss);

        }
    }
}
