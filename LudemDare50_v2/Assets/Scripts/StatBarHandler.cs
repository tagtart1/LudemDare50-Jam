using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StatBarHandler : MonoBehaviour
{
    private Health playerHealth;

    [Header("Stat Bars")]
    [SerializeField] private Slider healthBar;
    [SerializeField] private Slider foodBar;
    [SerializeField] private Slider thirstBar;
    [SerializeField] private Slider sanityBar;
    [SerializeField] private Slider tempBar;

    

    [Header("Temperature")]
    [SerializeField] private DayNightCycle dayNightCycle;
    [SerializeField] Image tempStatImage;
    [SerializeField] Image fillBar;
    [SerializeField] Sprite heatIcon;
    [SerializeField] Sprite coldIcon;


    [Header("Decrease Rates")]
    [SerializeField] private float healthBarRate;
    
    [SerializeField] private float  foodBarRate;
    
    [SerializeField] private float thirstBarRate;
    
    [SerializeField] private float sanityBarRate;
 
    [SerializeField] private float coldBarRate;
    [SerializeField] private float heatBarRate;


    [SerializeField] private AudioClip healSFX;
    [SerializeField] private AudioClip replenishSFX;
 
    private void Start()
    {
        playerHealth = GetComponent<Health>();
        tempBar.value = .25f;
    }



    private void Update()
    {
        healthBar.value = playerHealth.GetHealthPoints() / 100;

        DecreaseBarValues();
        IncreaseTemperatureValues();
    }

    private void DecreaseBarValues()
    {
        foodBar.value -= foodBarRate * Time.deltaTime;
        thirstBar.value -= thirstBarRate * Time.deltaTime;
        sanityBar.value -= sanityBarRate * Time.deltaTime;

        if (foodBar.value <= 0|| thirstBar.value <= 0 || sanityBar.value <= 0 || tempBar.value >= 1 || tempBar.value <= 0)
        {
            playerHealth.AddHealthPoints(-healthBarRate * Time.deltaTime);
            //play a heartbeat sound
        }
        
    }

    private void IncreaseTemperatureValues()
    {
        if (dayNightCycle.IsDay())
        {
            tempBar.value += heatBarRate * Time.deltaTime;
        }
        else
        {
            tempBar.value -= coldBarRate * Time.deltaTime;
        }

        if (tempBar.value > .5f)
        {
            tempStatImage.sprite = heatIcon;
            fillBar.color = new Color32(244, 98, 9, 255);
        }  
        else
        {
            tempStatImage.sprite = coldIcon;
            fillBar.color = new Color32(0, 205, 249, 255);
        }

       //if (tempBar.value > .49f && tempBar.value < .505f)
       // {
       //     halfwayFX.Play();
       // }
    }

    public void IncrementStatBar(float amount, Stat stat)
    {
        switch(stat)
        {
            case Stat.health: playerHealth.AddHealthPoints(amount);
                SoundManager.PlayEffectSound_Static(healSFX);
                break;
            case Stat.food: foodBar.value += (amount / 100);
                SoundManager.PlayEffectSound_Static(replenishSFX);
                break;
            case Stat.thirst: thirstBar.value += (amount / 100);
                SoundManager.PlayEffectSound_Static(replenishSFX);
                break;
            case Stat.sanity: sanityBar.value += (amount / 100);
                break;
            case Stat.temperature: tempBar.value += (amount / 100);
                break;
        }
    }

    public void IncreaseAllRates()
    {
        foodBarRate += .005f;
        thirstBarRate += .005f;
        sanityBarRate += .005f;
        healthBarRate += .01f;
        coldBarRate += .005f;
        heatBarRate += .005f;
    }

}

public enum Stat
{
    health,
    food,
    thirst,
    sanity,
    temperature
}
