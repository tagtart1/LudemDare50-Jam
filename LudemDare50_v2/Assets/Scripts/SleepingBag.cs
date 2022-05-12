using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SleepingBag : MonoBehaviour
{
    [SerializeField] GameObject sleepingbagAwake;
    [SerializeField] GameObject sleepingbagSleep;


    private bool canInteract;
    Player player;

    private void Start()
    {
        player = FindObjectOfType<Player>();
    }
    private void Update()
    {
        if (canInteract)
        {
            Debug.Log("canInteract");
            if (player.PressedInteract)
            {
                Debug.Log("pressingINteract");
                Interact();
            }
        }
            
        
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<Player>())
        {
            Debug.Log("in range");
            canInteract = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.GetComponent<Player>())
        {

            canInteract = false;
        }
    }

    private void Interact()
    {
        if (!player.IsSleeping)
        {
            player.CanMove = false;
           player.EnableChildrenObjects(false);
            player.IsSleeping = true;
            sleepingbagAwake.SetActive(false);
            sleepingbagSleep.SetActive(true);
            BoxCollider collider = GetComponent<BoxCollider>();
            player.GetCharacterPlane().localEulerAngles = Vector3.zero;
            collider.size = new Vector3(collider.size.x * 1.25f, collider.size.y, collider.size.z * 1.25f);

        }
        else
        {
            player.GetCharacterPlane().localEulerAngles = Vector3.zero;
            player.CanMove = true;
            player.EnableChildrenObjects(true);
            player.IsSleeping = false;
            sleepingbagAwake.SetActive(true);
            sleepingbagSleep.SetActive(false);
            BoxCollider collider = GetComponent<BoxCollider>();
            collider.size = new Vector3(collider.size.x * .8f, collider.size.y, collider.size.z * .8f);
        }
    }

    
}
