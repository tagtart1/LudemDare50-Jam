using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tool : MonoBehaviour
{
  
    public Killable canAttack;
    [SerializeField] float damage;
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
        healthToAttack.TakeDamage(damage, this);
       
    }

    public void InRange(Health health)
    {
        healthToAttack = health;
    }

    public void OutOfRange()
    {
        healthToAttack = null;
    }
}
