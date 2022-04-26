using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PondWater : MonoBehaviour
{
   
    [SerializeField] private float thirstReplenished;
    private float cooldownTimer;
    private float cooldown = .5f;
    private Player player;
    private bool canInteract;


    private void Update()
    {
        cooldownTimer += Time.deltaTime;
        if (canInteract && player.pressedInteract && cooldownTimer > cooldown)
        {
            Interact();
        }
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<Player>(out player))
        {
            canInteract = true;
        }
   
    }

    private void OnTriggerExit(Collider other)
    {
        canInteract = false;
    }

    private void Interact()
    {
        cooldownTimer = 0;
        player.GetComponent<StatBarHandler>().IncrementStatBar(thirstReplenished, Stat.thirst);
    }
}
