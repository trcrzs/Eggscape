using System.Collections;
using UnityEngine;

public class WaveManager : MonoBehaviour
{
    public GameObject enemyPrefab;
    public Transform[] spawnPoints;

    public int totalEnemiesInWave = 10;
    public float minSpawnTime = 1f;
    public float maxSpawnTime = 3f;

    private int aliveEnemies = 0;
    private int registeredEnemies = 0;
    private bool finishedSpawning = false;
    private bool roundEnded = false;

    void Start()
    {
        StartCoroutine(StartWave());
    }

    IEnumerator StartWave()
    {
        yield return new WaitForSeconds(0.2f);

        int enemiesToSpawn = totalEnemiesInWave - registeredEnemies;

        for (int i = 0; i < enemiesToSpawn; i++)
        {
            float waitTime = Random.Range(minSpawnTime, maxSpawnTime);
            yield return new WaitForSeconds(waitTime);
            SpawnEnemy();
        }

        finishedSpawning = true;
        CheckRoundClear();
    }

    void SpawnEnemy()
    {
        if (enemyPrefab == null) return;
        if (spawnPoints.Length == 0) return;

        int randomIndex = Random.Range(0, spawnPoints.Length);
        Transform spawnPoint = spawnPoints[randomIndex];

        Instantiate(enemyPrefab, spawnPoint.position, Quaternion.identity);
    }

    public void RegisterEnemy()
    {
        aliveEnemies++;
        registeredEnemies++;
    }

    public void EnemyDied()
    {
        aliveEnemies--;
        CheckRoundClear();
    }

    void CheckRoundClear()
    {
        if (roundEnded) return;

        if (finishedSpawning && aliveEnemies <= 0)
        {
            roundEnded = true;
            Debug.Log("Round Clear");
        }
    }
}