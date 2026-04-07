using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class WaveManager : MonoBehaviour
{
    public GameObject enemyPrefab;
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
        registeredEnemies++;
        UpdateUI();
    }

    public void EnemyDied()
    {
        enemiesDefeated++;
        UpdateUI();
        CheckRoundClear();
    }

    void UpdateUI()
    {
        if (enemiesText != null)
        {
            enemiesRemaining = totalEnemiesInWave - enemiesDefeated;
            enemiesText.text = "Enemies Remaining: " + enemiesRemaining;
        }
    }

    void CheckRoundClear()
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

    IEnumerator EndRoundSequence()
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

        int currentIndex = UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex;
        UnityEngine.SceneManagement.SceneManager.LoadScene(currentIndex + 1);
    }

    IEnumerator LoadNextWave()
    {
        yield return new WaitForSeconds(3f);

        int currentIndex = UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex;
        UnityEngine.SceneManagement.SceneManager.LoadScene("Menu");
    }
}