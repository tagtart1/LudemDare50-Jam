using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SleepingBag : MonoBehaviour
{
    [SerializeField] GameObject sleepingbagAwake;
    [SerializeField] GameObject sleepingbagSleep;


    private bool canInteract;
    Player player;
    private void Update()
    {
        if (canInteract && player.pressedInteract)
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
        if (!player.isSleeping)
        {
            player.canMove = false;
            player.playerPlane.SetActive(false);
            player.isSleeping = true;
            sleepingbagAwake.SetActive(false);
            sleepingbagSleep.SetActive(true);
        }
        else
        {
            player.canMove = true;
            player.playerPlane.SetActive(true);
            player.isSleeping = false;
            sleepingbagAwake.SetActive(true);
            sleepingbagSleep.SetActive(false);
        }
    }

    
}
