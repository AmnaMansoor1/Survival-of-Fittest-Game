using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class EnemySpawner : MonoBehaviour
{
    [Header("Enemy Prefab")]
    public GameObject enemyPrefab;

    [Header("Spawn Points")]
    public Transform[] spawnPoints;

    [Header("Spawn Settings")]
    public int totalEnemiesToSpawn = 5;
    public float firstSpawnDelay = 5f;
    public float timeBetweenSpawns = 20f;
    public int maxEnemiesAlive = 2;

    private int spawnedCount = 0;
    private int aliveEnemies = 0;

    void Start()
    {
        StartCoroutine(SpawnEnemyRoutine());
    }

    IEnumerator SpawnEnemyRoutine()
    {
        yield return new WaitForSeconds(firstSpawnDelay);

        while (spawnedCount < totalEnemiesToSpawn)
        {
            if (aliveEnemies < maxEnemiesAlive)
            {
                SpawnEnemy();
            }

            yield return new WaitForSeconds(timeBetweenSpawns);
        }

        Debug.Log("All enemies spawned.");
    }

    void SpawnEnemy()
    {
        if (enemyPrefab == null)
        {
            Debug.LogError("Enemy prefab is not assigned.");
            return;
        }

        if (spawnPoints == null || spawnPoints.Length == 0)
        {
            Debug.LogError("No spawn points assigned.");
            return;
        }

        Transform selectedSpawn = spawnPoints[Random.Range(0, spawnPoints.Length)];

        Vector3 spawnPosition = selectedSpawn.position;

        NavMeshHit hit;

        if (NavMesh.SamplePosition(spawnPosition, out hit, 5f, NavMesh.AllAreas))
        {
            spawnPosition = hit.position;
        }
        else
        {
            Debug.LogWarning("Spawn point is not near NavMesh: " + selectedSpawn.name);
            return;
        }

        GameObject newEnemy = Instantiate(enemyPrefab, spawnPosition, selectedSpawn.rotation);

        NavMeshAgent agent = newEnemy.GetComponent<NavMeshAgent>();

        if (agent != null)
        {
            agent.Warp(spawnPosition);
        }

        EnemyAI enemyAI = newEnemy.GetComponent<EnemyAI>();

        if (enemyAI != null)
        {
            enemyAI.spawner = this;
        }

        spawnedCount++;
        aliveEnemies++;

        Debug.Log("Spawned enemy " + spawnedCount + "/" + totalEnemiesToSpawn);
    }

    public void EnemyDied()
    {
        aliveEnemies--;

        if (aliveEnemies < 0)
        {
            aliveEnemies = 0;
        }

        Debug.Log("Enemy died. Alive enemies: " + aliveEnemies);
    }
}