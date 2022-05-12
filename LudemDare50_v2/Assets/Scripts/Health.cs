using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{

    

    [SerializeField] private float healthPoints;
    [SerializeField] public Killable objectType;
    [SerializeField] private ParticleSystem hitFX;
    [Header("Item Drop")]
    [SerializeField] GameObject pickUpPrefab;
    [SerializeField] float minYieldAmount;
    [SerializeField] float maxYieldAmount;
    [SerializeField] AudioClip hitSFX;
    [SerializeField] AudioClip deathSFX;

    private float startHealthPoints;
    private void Start()
    {
        startHealthPoints = healthPoints;
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

    public bool TakeDamage(float damage, Tool weapon) // for enemies
    {

        if (weapon.canAttack == objectType)
        {
            SoundManager.PlayEffectSound_Static(hitSFX);
            if (objectType == Killable.enemy) CinemachineShake.Instance.ShakeCamera(5f, .1f);
            PlayHitEffect();
            
            healthPoints -= damage;
            Debug.Log("took damage");
            if (TryGetComponent<FriendlyAI>(out FriendlyAI friendlyAI))
            {
                friendlyAI.FleeBehavior();
            }
            if (GetComponentInChildren<Animator>() != null)
            {
                GetComponentInChildren<Animator>().SetTrigger("Damaged");
            }
            if (healthPoints <= 0)
            {
                Die();
            }
            return true;
        }
        else
            return false;
    }

    public void TakeDamage(float damage) // for player
    {
        SoundManager.PlayEffectSound_Static(hitSFX);
        PlayHitEffect();
     
        healthPoints -= damage;
        GetComponentInChildren<Animator>().SetTrigger("Damaged");
        if (healthPoints <= 0)
        {
            GetComponent<Player>().SetPlayerDead(true);
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
        float dropAmount = UnityEngine.Random.Range(minYieldAmount, maxYieldAmount);
        for(int i = 0; i < dropAmount; i++)
        {
            if (objectType == Killable.tree || objectType == Killable.boulder) CinemachineShake.Instance.ShakeCamera(7f, .15f);
            SoundManager.PlayEffectSound_Static(deathSFX);
            int n = UnityEngine.Random.Range(-1, 1);
            
            GameObject itemDrop = Instantiate(pickUpPrefab, transform.position, pickUpPrefab.transform.rotation);
            itemDrop.GetComponent<Rigidbody>().velocity = Vector3.zero;
            itemDrop.GetComponent<Rigidbody>().AddForce(new Vector3(UnityEngine.Random.Range(3,7), UnityEngine.Random.Range(1, 3), UnityEngine.Random.Range(-2,2)) * (n < 0 ? -1 : 1) , ForceMode.Impulse);
            
        }
        Destroy(gameObject);
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

    public float GetHealthPoints()
    {
        return healthPoints;
    }

    public void AddHealthPoints(float amount)
    {    
        
        healthPoints += amount;
        if (healthPoints > 100)
        {
            healthPoints = 100f;
        }
        if (healthPoints <= 0)
        {
            GetComponent<Player>().SetPlayerDead(true);
            Die();
        }
    }
}
