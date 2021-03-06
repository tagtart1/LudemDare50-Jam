using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttackState : EnemyBaseState
{
    private bool playerInAttackRange;
    private float attackCooldown = 1.5f;
    private float attackTimer;
    private EnemyStateManager enemy;
    public override void EnterState(EnemyStateManager enemy)
    {
        
        attackTimer = 0;
        this.enemy = enemy;
        enemy.anim.SetFloat("Speed", 0);
    }

    public override void UpdateState(EnemyStateManager enemy)
    {
        playerInAttackRange = Physics.CheckSphere(enemy.transform.position, enemy.attackRange, enemy.whatIsPlayer);

        if (!playerInAttackRange)
        {
            enemy.SwitchState(enemy.ChaseState);
        }
        else
        {
            ChargeAttack();
        }
    }

    private void ChargeAttack()
    {
        if (attackTimer < attackCooldown)
        {
            attackTimer += Time.deltaTime;
        }
        else
        {
            attackTimer = 0;
            LaunchAttack();
        }
    }

    private void LaunchAttack()
    {
        enemy.anim.SetTrigger("Attack");
        enemy.player.GetComponent<Health>().TakeDamage(enemy.attackDamage);
    }

}
