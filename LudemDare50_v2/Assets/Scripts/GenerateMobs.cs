using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenerateMobs : MonoBehaviour
{
    [SerializeField] GameObject mobToSpawn;
    [SerializeField] private float xPosRange;
    [SerializeField] private float zPosRange;
    [SerializeField] private int maxMobCount;
    [SerializeField] LayerMask whatIsGround;
    [SerializeField] private float timeBetweenSpawns;

    private Vector3 spawnPoint;

    private int mobCount;
    private bool spawnPointSet;

    private void Start()
    {
       
    }

    private void Update()
    {
        if (!spawnPointSet)
        {
            SearchSpawnPoint();
        }
    }


    IEnumerator MobSpawn()
    {
        while (mobCount < maxMobCount)
        {
            Instantiate(mobToSpawn, spawnPoint, Quaternion.identity);
            yield return new WaitForSeconds(timeBetweenSpawns);
            spawnPointSet = false;
            mobCount++;
        }
    }

    private void SearchSpawnPoint()
    {
        float randomZ = Random.Range(-zPosRange, zPosRange);
        float randomX = Random.Range(-xPosRange, xPosRange);

        spawnPoint= new Vector3(randomX, 1, randomZ);

        if (Physics.Raycast(spawnPoint, Vector3.down, 2f, whatIsGround))
        {
            spawnPointSet = true;

        }

    }

    public void SpawnMobs()
    {
        StartCoroutine(MobSpawn());
    }


}
