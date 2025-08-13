using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float jumpForce = 12f;
    public Transform groundCheck;
    public LayerMask groundLayer;
    public Transform visual;          // assign the child "Visual"
    public float checkRadius = 0.2f;  // match your ground check

    private Rigidbody2D rb;
    private Animator animator;
    private bool isGrounded;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        float move = Input.GetAxis("Horizontal");
        rb.linearVelocity = new Vector2(move * moveSpeed, rb.linearVelocity.y);

        // Check if player is touching ground
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, 0.1f, groundLayer);

        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
        }

        // Ground check, jump, etc...

        animator.SetBool("isWalking", move != 0);
    }

    void LateUpdate()
    {
        // Only apply slope tilt if visual is assigned
        if (visual == null) return;

        // Raycast down to get the ground normal
        RaycastHit2D hit = Physics2D.Raycast(groundCheck.position, Vector2.down, 0.4f, groundLayer);
        if (hit.collider != null)
        {
            // Convert ground normal to an angle; rotate sprite to align with slope
            float angle = Mathf.Atan2(hit.normal.x, hit.normal.y) * Mathf.Rad2Deg;
            visual.localRotation = Quaternion.Euler(0, 0, -angle);
        }
        else
        {
            // Not grounded → reset tilt
            visual.localRotation = Quaternion.identity;
        }
    }
}


