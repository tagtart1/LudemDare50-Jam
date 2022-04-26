using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{

    public static event Action OnDamageTaken;

    [SerializeField] private float healthPoints;
    [SerializeField] public Killable objectType;
    [SerializeField] private ParticleSystem hitFX;
    [Header("Item Drop")]
    [SerializeField] GameObject pickUpPrefab;
    [SerializeField] float yieldAmount;


    private void Start()
    {
        switch(objectType)
        {
            case Killable.enemy: hitFX = GameObject.Find("BloodParticle").GetComponent<ParticleSystem>();
                break;
            case Killable.tree: hitFX = GameObject.Find("TreeParticle").GetComponent<ParticleSystem>();
                break;
            case Killable.boulder: hitFX = GameObject.Find("RockParticle").GetComponent<ParticleSystem>();
                break;
        }

        
    }

    public void TakeDamage(float damage, Tool weapon) // for enemies
    {
     
        if (weapon.canAttack == objectType)
        {
            if (objectType == Killable.enemy) CinemachineShake.Instance.ShakeCamera(5f, .1f);
            PlayHitEffect();
            OnDamageTaken?.Invoke();
            healthPoints -= damage;
            Debug.Log("took damage");
            if (TryGetComponent<FriendlyAI>(out FriendlyAI friendlyAI))
            {
                friendlyAI.FleeBehavior();
            }
            if (healthPoints <= 0)
            {
                Die();
            }
        }
    }

    public void TakeDamage(float damage) // for player
    {
        PlayHitEffect();
        OnDamageTaken?.Invoke();
        healthPoints -= damage;
        Debug.Log("took damage");
        if (healthPoints <= 0)
        {
            Die();
        }
    }

    private void PlayHitEffect()
    {
        hitFX.transform.position = transform.position;
        hitFX.Play();
    }

    public void Die()
    {
        for(int i = 0; i < yieldAmount; i++)
        {
            int n = UnityEngine.Random.Range(-1, 1);
            
            GameObject itemDrop = Instantiate(pickUpPrefab, transform.position, pickUpPrefab.transform.rotation);
            itemDrop.GetComponent<Rigidbody>().velocity = Vector3.zero;
            itemDrop.GetComponent<Rigidbody>().AddForce(new Vector3(UnityEngine.Random.Range(3,7), UnityEngine.Random.Range(1, 3), UnityEngine.Random.Range(-2,2)) * (n < 0 ? -1 : 1) , ForceMode.Impulse);
            
        }
        Destroy(this.gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<Tool>())
        {
            other.GetComponent<Tool>().InRange(this);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.GetComponent<Tool>())
        {
            other.GetComponent<Tool>().OutOfRange();
        }
    }
}
