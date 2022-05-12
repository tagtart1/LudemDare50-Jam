using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tool : MonoBehaviour
{
    [SerializeField] private float damage;
   
    public Killable canAttack;
    
    [SerializeField] private float useCooldown;
    private bool canUseTool;
    private float cooldownTimer;
    private Health healthToAttack;
    private Player player;
    public bool isInRangeOfObject;
   
   
    private void Start()
    {
        player = GetComponentInParent<Player>();
        
    }

    private void Update()
    {
        if (player.PressedLeftClick && canUseTool && !player.inventory.IsMenuActive())
        {
            UseTool();
        }

        if (cooldownTimer > 0) cooldownTimer -= Time.deltaTime;
        else canUseTool = true;
        
    }

    public void UseTool()
    {
        
        cooldownTimer = useCooldown;
        canUseTool = false;
        if (healthToAttack != null)
        {
           
            if (healthToAttack.TakeDamage(damage, this))
            {
                HandleDurability();
                player.Attack();
            }
        }
       
        
    }

    public void SetToolStats(float damage)
    {
       
        this.damage = damage;
    }

    public void InRange(Health health)
    {
        healthToAttack = health;
    }

    public void OutOfRange()
    {
        healthToAttack = null;
    }

    private void HandleDurability() //checks the durability to be above 1 and also removes 1 durability for each use 
    {
        //needs to remove from inventory and clears slot
        player.inventory.HandleToolItem(GetComponent<ResourcePickup>().id, 1f);
        
    }
}
