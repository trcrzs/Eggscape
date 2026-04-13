using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class WaveManager : MonoBehaviour
{
    public GameObject[] enemyPrefabs;
    public Transform[] spawnPoints;

    public int totalEnemiesInWave = 10;
    public float minSpawnTime = 1f;
    public float maxSpawnTime = 3f;

    private int registeredEnemies = 0;
    private bool finishedSpawning = false;
    private bool roundEnded = false;
    private int enemiesDefeated = 0;
    private int enemiesRemaining;

    private TMP_Text enemiesText;

    void Start()
    {
        enemiesText = FindFirstObjectByType<TMP_Text>();

        StartCoroutine(StartWave());
        UpdateUI();
    }

    IEnumerator StartWave()
    {
        yield return new WaitForSeconds(0.2f);

        int enemiesToSpawn = totalEnemiesInWave - registeredEnemies; //counts number of enemies to spawn as it spawns them

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
        if (enemyPrefabs == null || enemyPrefabs.Length == 0) return;
        if (spawnPoints == null || spawnPoints.Length == 0) return;

        int randomSpawnIndex = Random.Range(0, spawnPoints.Length); //picks random spawn point from array of spawn points
        Transform spawnPoint = spawnPoints[randomSpawnIndex];

        int randomEnemyIndex = Random.Range(0, enemyPrefabs.Length); //picks random enemy to spawn from array of enemies
        GameObject enemyToSpawn = enemyPrefabs[randomEnemyIndex];

        if (enemyToSpawn == null) return;

        Instantiate(enemyToSpawn, spawnPoint.position, Quaternion.identity); //uses above to spawn enemy in randomized place
    }

    public void RegisterEnemy() //count number of enemies so we don't go over the wave count, use to update UI
    {
        registeredEnemies++;
        UpdateUI();
    }

    public void EnemyDied() //counts dead enemies, used to update UI
    {
        enemiesDefeated++;
        UpdateUI();
        CheckRoundClear();
    }

    void UpdateUI() //shows enemies remaining in UI for player
    {
        if (enemiesText != null)
        {
            enemiesRemaining = totalEnemiesInWave - enemiesDefeated;
            enemiesText.text = "Enemies Remaining: " + enemiesRemaining;
        }
    }

    void CheckRoundClear() //checks if all enemies have been defeated before starting end round sequence
    {
        if (roundEnded) return;

        if (finishedSpawning && enemiesDefeated >= totalEnemiesInWave)
        {
            roundEnded = true;

            if (enemiesText != null)
            {
                enemiesText.text = "Round Clear";
            }

            Debug.Log("Round Clear");

            StartCoroutine(EndRoundSequence());
        }
    }

    IEnumerator EndRoundSequence() //kills level music and plays win sound then delays and loads the next game scene
    {
        AudioSource[] audioSources = Camera.main.GetComponents<AudioSource>();

        if (audioSources.Length > 0)
        {
            audioSources[0].Stop();
        }

        if (audioSources.Length > 1)
        {
            audioSources[1].Play();
        }

        yield return new WaitForSeconds(2f);

        int currentIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentIndex + 1);
    }
}