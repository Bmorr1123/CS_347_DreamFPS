using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public Transform spawnPos;
    public GameObject enemyPrefab;
    public float spawnChance;
    public float minSpawnDelay;
    public float maxSpawnDelay;

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
        Instantiate(enemyPrefab, spawnPos);
    }
}
