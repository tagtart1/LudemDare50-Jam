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
    public float maxDurability;
   
    private void Start()
    {
        player = GetComponentInParent<Player>();
        
    }

    private void Update()
    {
        if (player.pressedLeftClick && canUseTool && !player.inventory.IsMenuActive())
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
            HandleDurability();
            healthToAttack.TakeDamage(damage, this);
            
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

    private void HandleDurability()
    {
        //needs to remove from inventory and clears slot
        player.inventory.HandleToolItem(GetComponent<ResourcePickup>().id);
        
    }
}
