using UnityEngine;

public class TomatoProjectile : MonoBehaviour
{

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player")) //if tomato hits player collider then it takes away 1 damage
        {
            PlayerHealth playerHealth = other.GetComponent<PlayerHealth>();

            if (playerHealth != null)
            {
                playerHealth.TakeDamage(1);
            }

            Destroy(gameObject);
        }

        if (other.CompareTag("OOB")) //if tomato hits out of bounds collider then it is destroyed
        {
            Destroy(gameObject);
        }
    }
}