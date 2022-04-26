using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DayNightCycle : MonoBehaviour
{
    [SerializeField] private Light mainLight;
    private float daysPassed;
    private bool isDay = true;
    private float yFactor = 52;
    private bool isCyclingToDay = false;
    [SerializeField] GenerateMobs chickenSpawner;
    [SerializeField] GenerateMobs ratSpawner;
    [SerializeField] private float maxYRotation = 52f;
    [SerializeField] private float morningShadowMultiplier = 4f;
    [SerializeField] private float yFactorRotateSpeed = .015f;
    [SerializeField] private float toNightSpeed = 0.01f;
    [SerializeField] private float toDaySpeed = 0.01f;
    [SerializeField] private float cycleLength = 60f;
   
    //increasing the cycle length meaning you have to divide factor rotate speed and double  the shadow mult
    
    public float cycleTimer = 0f;

    private void Update()
    {
        if (cycleTimer < cycleLength)
        {
            cycleTimer += Time.deltaTime;
        }
        else
        {
            CycleTime();
        }
       
        if (isDay && yFactor > -maxYRotation)
        {
            
            yFactor -= yFactorRotateSpeed;
            //transform.localRotation = Quaternion.Euler(new Vector3(138, (Time.deltaTime * 360f) - 90f, 0));
            transform.rotation = Quaternion.Euler(138, yFactor , 0);
            
        } else if (isCyclingToDay && yFactor < maxYRotation)
        {
            yFactor += yFactorRotateSpeed * morningShadowMultiplier;
            transform.rotation = Quaternion.Euler(138, yFactor, 0);
        }

      
        

    }

    private void CycleTime()
    {
        if (isDay)
        {
           
            mainLight.intensity -= toNightSpeed * Time.deltaTime;
            if (mainLight.intensity <= 0)
            {
                ratSpawner.SpawnMobs();
                isDay = false;
                cycleTimer = 0;
                return;
            }
        }
        else
        {

            isCyclingToDay = true;
            mainLight.intensity += toDaySpeed * Time.deltaTime;
            if (mainLight.intensity > 1)
            {
                isCyclingToDay = false;
                chickenSpawner.SpawnMobs();
                isDay = true;
                daysPassed++;
                cycleTimer = 0;
                return;
            }
        }
    }

    public bool IsDay()
    {
        return isDay;
    }

    public float GetDaysPassed()
    {
        return daysPassed;
    }
}
