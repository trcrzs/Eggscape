using UnityEngine;

public class Egg : MonoBehaviour
{
    public float moveSpeed = 10f;
    public float destroyDelay = 1f;
    public AudioClip crack;

    private Collider2D col;
    private Animator eggAnim;
    private AudioSource splatSound;
    private Rigidbody2D rb;

    private bool hasCracked = false;
    private Vector2 moveDirection = Vector2.zero;

    void Awake()
    {
        col = GetComponent<Collider2D>();
        eggAnim = GetComponentInChildren<Animator>();
        splatSound = GetComponent<AudioSource>();
        rb = GetComponent<Rigidbody2D>();

        if (rb != null)
        {
            rb.bodyType = RigidbodyType2D.Kinematic;
            rb.gravityScale = 0f;
            rb.linearVelocity = Vector2.zero;
            rb.angularVelocity = 0f;
        }
    }

    void FixedUpdate()
    {
        if (hasCracked) return;

        if (rb != null)
        {
            rb.MovePosition(rb.position + moveDirection * moveSpeed * Time.fixedDeltaTime);
        }
    }

    public void SetDirection(Vector2 direction, float speed)
    {
        moveDirection = direction.normalized;
        moveSpeed = speed;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (hasCracked) return;

        if (other.CompareTag("Enemy") || other.CompareTag("OOB"))
        {
            CrackEgg();
        }
    }

    void CrackEgg()
    {
        hasCracked = true;

        if (rb != null)
        {
            rb.linearVelocity = Vector2.zero;
            rb.angularVelocity = 0f;
        }

        if (col != null)
        {
            col.enabled = false;
        }

        if (eggAnim != null)
        {
            eggAnim.SetTrigger("Cracked");
        }

        if (splatSound != null && crack != null)
        {
            splatSound.PlayOneShot(crack);
        }

        Destroy(gameObject, destroyDelay);
    }
}