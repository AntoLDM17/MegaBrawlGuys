using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float movementSpeed = 5f;
    public float jumpForce = 10f;
    public float doubleJumpForce = 5f;
    public float dashSpeed = 15f;
    public float dashCooldown = 0.5f;
    private bool isGrounded;
    private bool doubleJumped;
    private bool canDash = true;
    private float lastDashTime;
    private Animator animator;

    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        float horizontalMovement = Input.GetAxis("Horizontal");
        // Adjust the movement input to the rotation axis
        Vector3 movement = new Vector3(0f, 0f, horizontalMovement) * movementSpeed * Time.deltaTime;

        if (horizontalMovement > 0)
        {
            animator.SetBool("Walk Forward", true);
            animator.SetBool("Walk Backward", false);
        }
        else if (horizontalMovement < 0)
        {
            animator.SetBool("Walk Forward", false);
            animator.SetBool("Walk Backward", true);
        }
        else
        {
            animator.SetBool("Walk Forward", false);
            animator.SetBool("Walk Backward", false);
        }
        transform.Translate(movement);

        // Lateral dash
        if ((Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.RightArrow)) && canDash)
        {
            if (Time.time - lastDashTime < dashCooldown)
            {
                // Double fast tap
                if (Mathf.Abs(horizontalMovement) > 0.1f)
                {
                    // Adjust the direction to the rotation axis
                    Dash(Vector3.forward * (horizontalMovement > 0 ? 1 : -1));
                }
            }
            else
            {
                lastDashTime = Time.time;
            }
        }

        // Jump
        if ((Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.UpArrow)) && (isGrounded || !doubleJumped))
        {
            if (isGrounded)
            {
                rb.velocity = new Vector3(rb.velocity.x, 0f, 0f);
                rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
                isGrounded = false;
            }
            else // Double jump
            {
                rb.velocity = new Vector3(rb.velocity.x, 0f, 0f);
                rb.AddForce(Vector3.up * doubleJumpForce, ForceMode.Impulse);
                doubleJumped = true;
            }
        }
    }

    // Lateral dash
    public void Dash(Vector3 direction)
    {
        // Adjust the direction to the rotation axis
        Vector3 dashDirection = transform.TransformDirection(direction);

        rb.velocity = new Vector3(dashDirection.x * dashSpeed, rb.velocity.y, dashDirection.z * dashSpeed);
        canDash = false;
        Invoke("ResetDash", dashCooldown);
    }

    // Reset dash after dashCooldown
    void ResetDash()
    {
        rb.velocity = Vector3.zero;
        canDash = true;
    }

    // Called when the player touches the ground
    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
            doubleJumped = false;
        }
    }
}
