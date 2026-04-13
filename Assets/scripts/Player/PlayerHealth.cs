using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public int maxHealth = 3;
    public GameObject deathUI;
    public GameObject[] hearts;
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip deathSound;

    private int currentHealth;
    private bool isDead;

    void Start()
    {
        currentHealth = maxHealth;
        UpdateHearts();
    }

    public void TakeDamage(int amount) //run this whenever the player's health needs to take a hit with damage as parameter
    {
        if (isDead) return;

        currentHealth -= amount;

        if (currentHealth < 0)
        {
            currentHealth = 0;
        }

        UpdateHearts();

        if (currentHealth == 0)
        {
            Die(); //calls die below
        }
    }

    void UpdateHearts()
    {
        for (int i = 0; i < hearts.Length; i++)
        {
            hearts[i].SetActive(i < currentHealth); //updates the hearts array for health in the UI display
        }
    }

    void Die() //disables any existing gameplay gameobject and audio and displays the death screen UI
    {
        isDead = true;

        if (deathUI != null)
        {
            deathUI.SetActive(true);
        }

        if (audioSource != null && deathSound != null)
        {
            audioSource.PlayOneShot(deathSound);
        }

        EnemyFollow[] enemies = FindObjectsByType<EnemyFollow>(FindObjectsSortMode.None);
        foreach (EnemyFollow enemy in enemies)
        {
            enemy.enabled = false;
        }

        EnemyProjectile[] projectiles = FindObjectsByType<EnemyProjectile>(FindObjectsSortMode.None);
        foreach (EnemyProjectile projectile in projectiles)
        {
            projectile.enabled = false;
        }

        TomatoProjectile[] tomatoes = FindObjectsByType<TomatoProjectile>(FindObjectsSortMode.None);
        foreach (TomatoProjectile tomato in tomatoes)
        {
            Destroy(tomato.gameObject);
        }

        AudioListener.pause = true;
        Time.timeScale = 0f;
    }
}