using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyChaseState : EnemyBaseState
{
    private bool playerInAttackRange;
    private bool playerInSightRange;
    
    public override void EnterState(EnemyStateManager enemy)
    {
        
    }

    public override void UpdateState(EnemyStateManager enemy)
    {
        playerInAttackRange  = Physics.CheckSphere(enemy.transform.position, enemy.attackRange, enemy.whatIsPlayer);
        playerInSightRange = Physics.CheckSphere(enemy.transform.position, enemy.sightRange, enemy.whatIsPlayer);

        if (playerInAttackRange)
        {
            enemy.agent.SetDestination(enemy.transform.position);
            enemy.SwitchState(enemy.AttackState);
        }
        else if (!playerInSightRange)
        {
            enemy.SwitchState(enemy.PatrolState);
        }
        else
        {
            enemy.anim.SetFloat("Speed", 1);
            enemy.agent.SetDestination(enemy.player.transform.position);
        }



    }
}
