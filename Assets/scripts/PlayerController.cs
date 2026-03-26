using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public float speed = 5f;
    public GameObject eggPrefab;
    public Transform throwPoint;
    public float throwForce = 10f;

    private Rigidbody2D rb;
    private Animator animator;
    private Vector2 moveInput;
    private Camera cam;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponentInChildren<Animator>();
        cam = Camera.main;
    }

    void FixedUpdate()
    {
        rb.linearVelocity = moveInput * speed;
    }

    void Update()
    {
        if (animator != null)
        {
            animator.SetFloat("Speed", moveInput.magnitude);
        }

        if (cam == null || Mouse.current == null) return;

        Vector3 mouseWorld = cam.ScreenToWorldPoint(Mouse.current.position.ReadValue());
        mouseWorld.z = 0f;

        if (mouseWorld.x > transform.position.x)
        {
            transform.localScale = new Vector3(1f, 1f, 1f);
        }
        else if (mouseWorld.x < transform.position.x)
        {
            transform.localScale = new Vector3(-1f, 1f, 1f);
        }
    }

    public void OnMove(InputValue value)
    {
        moveInput = value.Get<Vector2>();
    }

    public void OnThrow(InputValue button)
    {
        if (!button.isPressed) return;
        if (eggPrefab == null || throwPoint == null || cam == null || Mouse.current == null) return;

        if (animator != null)
        {
            animator.SetTrigger("Throw");
        }

        Vector3 mouseWorld = cam.ScreenToWorldPoint(Mouse.current.position.ReadValue());
        mouseWorld.z = 0f;

        Vector2 start = throwPoint.position;
        Vector2 target = new Vector2(mouseWorld.x, mouseWorld.y);
        Vector2 direction = (target - start).normalized;

        GameObject eggObject = Instantiate(eggPrefab, throwPoint.position, Quaternion.identity);
        Egg egg = eggObject.GetComponent<Egg>();

        if (egg != null)
        {
            egg.SetDirection(direction, throwForce);
        }
    }
}