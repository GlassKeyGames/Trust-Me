using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed = 7f;
    public float acceleration = 45f;
    public float deceleration = 55f;

    [Header("Jump")]
    public float jumpForce = 11f;

    [Header("Ground Check")]
    public Transform groundCheck;
    public float groundCheckRadius = 0.2f;
    public LayerMask groundLayer;

    [Header("Jump Feel")]
    public float fallMultiplier = 2.5f;
    public float lowJumpMultiplier = 2f;

    [Header("Jump Forgiveness")]
    public float coyoteTime = 0.12f;
    public float jumpBufferTime = 0.12f;

    private Rigidbody2D rb;

    private float moveInput;
    private bool isGrounded;

    private float coyoteTimeCounter;
    private float jumpBufferCounter;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        moveInput = Input.GetAxisRaw("Horizontal");

        isGrounded = Physics2D.OverlapCircle(
            groundCheck.position,
            groundCheckRadius,
            groundLayer
        );

        // Coyote Time
        if (isGrounded)
        {
            coyoteTimeCounter = coyoteTime;
        }
        else
        {
            coyoteTimeCounter -= Time.deltaTime;
        }

        // Jump Buffer
        if (Input.GetButtonDown("Jump"))
        {
            jumpBufferCounter = jumpBufferTime;
        }
        else
        {
            jumpBufferCounter -= Time.deltaTime;
        }

        // Jump
        if (jumpBufferCounter > 0f && coyoteTimeCounter > 0f)
        {
            rb.linearVelocity = new Vector2(
                rb.linearVelocity.x,
                jumpForce
            );

            jumpBufferCounter = 0f;
            coyoteTimeCounter = 0f;
        }

        // Faster Falling
        if (rb.linearVelocity.y < 0)
        {
            rb.linearVelocity += Vector2.up *
                Physics2D.gravity.y *
                (fallMultiplier - 1f) *
                Time.deltaTime;
        }

        // Shorter Jump
        else if (
            rb.linearVelocity.y > 0 &&
            !Input.GetButton("Jump")
        )
        {
            rb.linearVelocity += Vector2.up *
                Physics2D.gravity.y *
                (lowJumpMultiplier - 1f) *
                Time.deltaTime;
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            RestartLevel();
        }
    }

    void FixedUpdate()
    {
        float targetSpeed = moveInput * moveSpeed;

        float speedDifference =
            targetSpeed - rb.linearVelocity.x;

        float movementRate;

        if (Mathf.Abs(targetSpeed) > 0.01f)
        {
            movementRate = acceleration;
        }
        else
        {
            movementRate = deceleration;
        }

        float movement =
            speedDifference *
            movementRate *
            Time.fixedDeltaTime;

        rb.linearVelocity = new Vector2(
            rb.linearVelocity.x + movement,
            rb.linearVelocity.y
        );
    }

    public void DisableMovement()
    {
        enabled = false;

        rb.linearVelocity = Vector2.zero;
    }

    public void PlayDeathEffect()
    {
        transform.localScale = new Vector3(
            transform.localScale.x,
            0.2f,
            transform.localScale.z
        );
    }

    private void RestartLevel()
    {
        SceneManager.LoadScene(
            SceneManager.GetActiveScene().buildIndex
        );
    }

    public void Die()
    {
        DisableMovement();
        PlayDeathEffect();
        StartCoroutine(RestartAfterDeath());
    }

    private IEnumerator RestartAfterDeath()
    {
        yield return new WaitForSeconds(0.25f);

        SceneManager.LoadScene(
            SceneManager.GetActiveScene().buildIndex
        );
    }
}
