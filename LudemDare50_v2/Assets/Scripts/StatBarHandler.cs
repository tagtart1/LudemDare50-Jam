using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StatBarHandler : MonoBehaviour
{
    [Header("Stat Bars")]
    [SerializeField] private Slider foodBar;
    [SerializeField] private Slider thirstBar;
    [SerializeField] private Slider sanityBar;
    [Header("Decrease Rates")]
    [Range(.01f,.1f)]
    [SerializeField] private float  foodBarRate;
    [Range(.01f, .1f)]
    [SerializeField] private float thirstBarRate;
    [Range(.01f, .1f)]
    [SerializeField] private float sanityBarRate;


    private void Update()
    {
        DecreaseBarValues();
    }

    private void DecreaseBarValues()
    {
        foodBar.value -= foodBarRate * Time.deltaTime;
        thirstBar.value -= thirstBarRate * Time.deltaTime;
        sanityBar.value -= sanityBarRate * Time.deltaTime;
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
        }
    }

    public void IncreaseAllRates()
    {
        foodBarRate += .05f;
        thirstBarRate += .05f;
    }

}

public enum Stat
{
    food,
    thirst,
    sanity
}
