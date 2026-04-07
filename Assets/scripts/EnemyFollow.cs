using UnityEngine;

public class EnemyFollow : MonoBehaviour
{
    public float moveSpeed = 3f;
    public float stopDistance = 0.2f;

    private Transform player;
    private WaveManager waveManager;

    void Start()
    {
        GameObject playerObject = GameObject.FindGameObjectWithTag("Player");

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
        if (other.CompareTag("PlayerAttack"))
        {
            if (waveManager != null)
            {
                waveManager.EnemyDied();
            }

            Destroy(gameObject);
        }

        if (other.CompareTag("Player"))
        {
            Debug.Log("Player Loses");
        }
    }
}