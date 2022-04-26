
using UnityEngine;

public class EnemyPatrolState : EnemyBaseState
{
    private bool playerInSightRange;

    private bool walkPointSet = false;
    private float atPointTimer;
    private Vector3 walkPoint;
    private float walkPointRange = 3f;
    private float maxTimeAtPoint = 7f;
    EnemyStateManager enemy;

    public override void EnterState(EnemyStateManager enemy)
    {
        this.enemy = enemy;
    }

    public override void UpdateState(EnemyStateManager enemy)
    {
        playerInSightRange = Physics.CheckSphere(enemy.transform.position, enemy.sightRange, enemy.whatIsPlayer);
      
        if (playerInSightRange)
        {
            enemy.SwitchState(enemy.ChaseState);
        }
        else
        {
            Patroling();
        }
    }

    private void Patroling()
    {
        if (!walkPointSet) SearchWalkPoint();

        if (walkPointSet)
        {
            enemy.animator.SetFloat("Speed", 1f);
            enemy.agent.SetDestination(walkPoint);
        }

        float distanceFromPoint = (enemy.transform.position - walkPoint).magnitude;

        if (distanceFromPoint < 1f)
        {
            if (atPointTimer < maxTimeAtPoint)
            {
                enemy.animator.SetFloat("Speed", 0);
                
                atPointTimer += Time.deltaTime;
            }
            else
            {
                walkPointSet = false;
                atPointTimer = 0;
            }
        }
    }

    private void SearchWalkPoint()
    {
        float randomZ = Random.Range(-walkPointRange, walkPointRange);
        float randomX = Random.Range(-walkPointRange, walkPointRange);

        walkPoint = new Vector3(enemy.transform.position.x + randomX, enemy.transform.position.y, enemy.transform.position.z + randomZ);

        if (Physics.Raycast(walkPoint, -enemy.transform.up, 2f, enemy.whatIsGround))
        {
            walkPointSet = true;
        }

    }
}



