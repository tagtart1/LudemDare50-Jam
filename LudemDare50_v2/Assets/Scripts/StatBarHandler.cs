using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StatBarHandler : MonoBehaviour
{
    [SerializeField] private DayNightCycle dayNightCycle;
    [Header("Stat Bars")]
    [SerializeField] private Slider foodBar;
    [SerializeField] private Slider thirstBar;
    [SerializeField] private Slider sanityBar;
    [SerializeField] private Slider tempBar;
    

    [Header("Temperature")]
    [SerializeField] Image tempStatImage;
    [SerializeField] Image fillBar;
    [SerializeField] Sprite heatIcon;
    [SerializeField] Sprite coldIcon;
    [SerializeField] ParticleSystem halfwayFX;

    [Header("Decrease Rates")]
    
    [Range(.01f,.1f)]
    [SerializeField] private float  foodBarRate;
    [Range(.01f, .1f)]
    [SerializeField] private float thirstBarRate;
    [Range(.01f, .1f)]
    [SerializeField] private float sanityBarRate;
    [Range(.01f, .1f)]
    [SerializeField] private float coldBarRate;
    [SerializeField] private float heatBarRate;




    private void Start()
    {
        tempBar.value = .51f;
    }



    private void Update()
    {
        DecreaseBarValues();
        IncreaseTemperatureValues();
    }

    private void DecreaseBarValues()
    {
        foodBar.value -= foodBarRate * Time.deltaTime;
        thirstBar.value -= thirstBarRate * Time.deltaTime;
        sanityBar.value -= sanityBarRate * Time.deltaTime;
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

       if (tempBar.value > .49f && tempBar.value < .505f)
        {
            halfwayFX.Play();
        }
    }

    public void IncrementStatBar(float amount, Stat stat)
    {
        switch(stat)
        {
            case Stat.food: foodBar.value += (amount / 100);
                break;
            case Stat.thirst: thirstBar.value += (amount / 100);
                break;
            case Stat.sanity: sanityBar.value += (amount / 100);
                break;
            case Stat.temperature: tempBar.value += (amount / 100);
                break;
        }
    }

    public void IncreaseAllRates()
    {
        foodBarRate += .01f;
        thirstBarRate += .01f;
    }

}

public enum Stat
{
    food,
    thirst,
    sanity,
    temperature
}
