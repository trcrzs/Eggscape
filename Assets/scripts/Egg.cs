using UnityEngine;

public class Egg : MonoBehaviour
{
    public float moveSpeed = 10f;
    public float destroyDelay = 0.5f;
    public AudioClip crack;

    private Collider2D col;
    private Animator eggAnim;
    private AudioSource splatSound;

    private bool hasCracked = false;
    private Vector2 moveDirection = Vector2.zero;

    void Awake()
    {
        col = GetComponent<Collider2D>();
        eggAnim = GetComponent<Animator>();
        splatSound = GetComponent<AudioSource>();
    }

    void Update()
    {
        if (hasCracked) return;

        Vector3 movement = new Vector3(moveDirection.x, moveDirection.y, 0f) * moveSpeed * Time.deltaTime;
        transform.position += movement;
    }

    public void SetDirection(Vector2 direction, float speed)
    {
        moveDirection = direction.normalized;
        moveSpeed = speed;
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (hasCracked) return;

        if (collision.gameObject.CompareTag("Enemy"))
        {
            CrackEgg();
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (hasCracked) return;

        if (other.CompareTag("OOB"))
        {
            CrackEgg();
        }
    }

    void CrackEgg()
    {
        hasCracked = true;

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