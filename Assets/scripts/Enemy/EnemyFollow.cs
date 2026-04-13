using UnityEngine;

public class EnemyFollow : MonoBehaviour
{
    public float moveSpeed = 3f;
    public float stopDistance = 0.2f;
    public AudioClip enemyDeath;

    private AudioSource audioSource;
    private Transform player;
    private WaveManager waveManager;
    private EnemyProjectile projectileScript;
    private Rigidbody2D rb;

    void Start()
    {
        GameObject playerObject = GameObject.FindGameObjectWithTag("Player");

        audioSource = GetComponent<AudioSource>();
        rb = GetComponent<Rigidbody2D>();
        projectileScript = GetComponent<EnemyProjectile>();

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

    void FixedUpdate()
    {
        if (player == null || rb == null) return;

        Vector2 direction = ((Vector2)player.position - rb.position);
        float distance = direction.magnitude;

        if (projectileScript != null)
        {
            projectileScript.TryThrow(player); //uses player transform to throw projectile only if enemy has projectile script attached
        }

        if (distance > stopDistance) //moves enemy rigidbody toward player
        {
            direction.Normalize();
            Vector2 newPosition = rb.position + direction * moveSpeed * Time.fixedDeltaTime;
            rb.MovePosition(newPosition);
        }
    }

    void OnTriggerEnter2D(Collider2D other) //enemy dies when egg hits them
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
    }

    void OnCollisionEnter2D(Collision2D collision) //on collision enemy takes away one health from player and then deletes itself
    //since this uses collision, trigger should not be set to IsTrigger or this won't fire
    {
        if (collision.collider.CompareTag("PlayerAttack"))
        {
            PlayerHealth playerHealth = collision.collider.GetComponent<PlayerHealth>();

            if (playerHealth == null)
            {
                playerHealth = collision.collider.GetComponentInParent<PlayerHealth>();
            }

            if (playerHealth != null)
            {
                playerHealth.TakeDamage(1);
            }

            Destroy(gameObject);
        }
    }
}