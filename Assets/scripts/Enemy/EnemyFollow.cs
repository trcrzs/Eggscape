using UnityEngine;

public class EnemyFollow : MonoBehaviour
{
    public float moveSpeed = 3f;
    public float stopDistance = 0.2f;
    public AudioClip enemyDeath;
    AudioSource audioSource;

    private Transform player;
    private WaveManager waveManager;

    void Start()
    {
        GameObject playerObject = GameObject.FindGameObjectWithTag("Player");


        audioSource = GetComponent<AudioSource>();

        if (playerObject != null)
        {
            player = playerObject.transform;
        }

        waveManager = FindFirstObjectByType<WaveManager>();

        if (waveManager != null)
        {
            waveManager.RegisterEnemy();
        }
    }

    void Update()
    {
        if (player == null) return;

        Vector3 direction = player.position - transform.position;
        float distance = direction.magnitude;

        if (distance > stopDistance)
        {
            direction.Normalize();
            transform.position += direction * moveSpeed * Time.deltaTime;
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Egg"))
        {

            if (waveManager != null)
            {
                waveManager.EnemyDied();
            }


            AudioSource.PlayClipAtPoint(enemyDeath, transform.position, 0.75f);
            Destroy(gameObject);
        }

        if (other.CompareTag("Player"))
        {
            Debug.Log("Player Loses");
        }
    }
}