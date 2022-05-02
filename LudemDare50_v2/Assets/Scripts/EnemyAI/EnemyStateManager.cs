using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyStateManager : MonoBehaviour
{
    public float sightRange;
    public float attackRange;
    public float attackDamage;
    public LayerMask whatIsPlayer;
    public NavMeshAgent agent;
    public LayerMask whatIsGround;
    public Player player;
    private bool isFlipping;
    private bool isFacingRight;
    private float xVelocity;
    private float lastXPosition;
    
    private EnemyBaseState currentState;
    public EnemyPatrolState PatrolState = new EnemyPatrolState();
    public EnemyChaseState ChaseState = new EnemyChaseState();
    public EnemyAttackState AttackState = new EnemyAttackState();
   

    private void Awake()
    {
      
        player = FindObjectOfType<Player>();
        agent = GetComponent<NavMeshAgent>();
    }

    private void Start()
    {
        currentState = PatrolState;
        currentState.EnterState(this);
    }
    private void Update()
    {
        xVelocity = (transform.position.x - lastXPosition) / Time.deltaTime;
        lastXPosition = transform.position.x;
        HandleSpriteFlip();
        
        currentState.UpdateState(this);
    }

    public void SwitchState(EnemyBaseState state)
    {
        currentState = state;
        state.EnterState(this);
    }

    private void HandleSpriteFlip()
    {
        if (xVelocity < 0 && isFacingRight && !isFlipping)
        {
            StopAllCoroutines();
            StartCoroutine(Flip());
        }
        else if (xVelocity > 0 && !isFacingRight && !isFlipping)
        {
            StopAllCoroutines();
            StartCoroutine(Flip());
        }
    }

    private IEnumerator Flip()
    {
        float timeElapsed = 0;
        Vector3 targetScale = transform.localScale;
        targetScale.x *= -1;
        while (timeElapsed < .4f)
        {
            isFlipping = true;
            timeElapsed += Time.deltaTime;
            transform.localScale = Vector3.Lerp(transform.localScale, targetScale, timeElapsed / 1.6f);
            yield return null;
        }
        isFlipping = false;
        transform.localScale = targetScale;
        isFacingRight = !isFacingRight;

    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, sightRange);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}
