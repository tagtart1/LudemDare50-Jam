using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class FriendlyAI : MonoBehaviour
{
    private NavMeshAgent agent;
    [SerializeField] private Animator animator;
    [SerializeField] private float maxTimeAtPoint;
    float lastXPosition;
    public float xVelocity;
    private float atPointTimer;
    private bool isFlipping;
    private bool isFacingRight;
    public LayerMask whatIsGround;

 

    //Patrolling

    public Vector3 walkPoint;
    private bool walkPointSet;
    public float walkPointRange;



    private void Awake()
    {
        
        agent = GetComponent<NavMeshAgent>();
       
    }

    private void Update()
    {

        xVelocity = (transform.position.x - lastXPosition) / Time.deltaTime; 
        lastXPosition = transform.position.x;

        Patroling();
        HandleSpriteFlip();

    }

  
    private void Patroling()
    {
        if (!walkPointSet) SearchWalkPoint();

        if (walkPointSet)
        {
            animator.SetFloat("Speed", 1f);
            agent.SetDestination(walkPoint);
        }

        float distanceFromPoint = (transform.position - walkPoint).magnitude;

        if (distanceFromPoint < 1f)
        {        
            
            if (atPointTimer < maxTimeAtPoint)
            {
                animator.SetFloat("Speed", 0f);
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

        walkPoint = new Vector3(transform.position.x + randomX, transform.position.y, transform.position.z + randomZ);
        
        if (Physics.Raycast(walkPoint, -transform.up, 2f, whatIsGround))
        {
            walkPointSet = true;
            
        }

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

    public void FleeBehavior()
    {
        walkPointSet = false;
        agent.speed = 15f;
        maxTimeAtPoint = .5f;
    }


}
