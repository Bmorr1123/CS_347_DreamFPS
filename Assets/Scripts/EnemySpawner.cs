using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public Transform spawnPos;
    public GameObject enemyPrefab;
    public float minSpawnDelay;
    public float maxSpawnDelay;
    public float spawnChance;
    public int maxSkeletons;

    private float nextSpawnTime;
    // Start is called before the first frame update
    void Start()
    {
        nextSpawnTime = Time.timeSinceLevelLoad + maxSpawnDelay;
    }

    // Update is called once per frame
    void Update()
    {
        // Enemy Spawning
        if (nextSpawnTime < Time.timeSinceLevelLoad)
        {
            SpawnEnemy();

            nextSpawnTime = Time.timeSinceLevelLoad + Random.Range(minSpawnDelay, maxSpawnDelay);
        }
    }

    void SpawnEnemy()
    {
        if (GameManager.TotalSkeletonsOnMap >= maxSkeletons)
        {
            return;
        }
        if (Random.Range(0, 1) >= spawnChance)
        {
            return;
        }

        Instantiate(enemyPrefab, spawnPos.position, spawnPos.rotation);
    }
}
