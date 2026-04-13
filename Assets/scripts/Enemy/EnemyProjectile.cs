using System.Collections;
using UnityEngine;

public class EnemyProjectile : MonoBehaviour
{
    [SerializeField] private Animator enemyAnim;
    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private Transform throwPoint;
    [SerializeField] private float speed = 5f;
    [SerializeField] private float projectileLifetime = 3f;
    [SerializeField] private float throwRange = 5f;
    [SerializeField] private float throwCooldown = 2f;
    [SerializeField] private float throwDelay = 0.35f;

    private float nextThrowTime;
    private bool isThrowing;

    public void TryThrow(Transform playerTransform)
    {
        if (playerTransform == null || projectilePrefab == null || throwPoint == null || isThrowing) //if this is off check assignments
            return;

        if (Vector2.Distance(transform.position, playerTransform.position) > throwRange) //should always throw if throwRange is 5f or above
            return;

        if (Time.time < nextThrowTime)
            return;

        StartCoroutine(Throw(playerTransform));
    }

    private IEnumerator Throw(Transform playerTransform)
    {
        isThrowing = true;
        nextThrowTime = Time.time + throwCooldown; //ensure player isn't bombarded with tomatoes

        if (enemyAnim != null)
            enemyAnim.SetTrigger("Throw");

        yield return new WaitForSeconds(throwDelay);

        if (playerTransform != null)
        {
            Vector2 direction = ((Vector2)playerTransform.position - (Vector2)throwPoint.position).normalized; //tracks player position so tomato is thrown directly at player

            GameObject proj = Instantiate(projectilePrefab, throwPoint.position, Quaternion.identity);
            Rigidbody2D rb = proj.GetComponent<Rigidbody2D>();

            if (rb != null)
                rb.linearVelocity = direction * speed;

            Destroy(proj, projectileLifetime);
        }

        isThrowing = false;
    }
}