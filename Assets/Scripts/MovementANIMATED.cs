using UnityEngine;

public class PlayerMovementANIMATED : MonoBehaviour
{
    public float movementSpeed = 5f;
    public float jumpForce = 10f;
    public float doubleJumpForce = 7.5f;
    public float dashSpeed = 11f; // Lateral dash velocity
    public float dashDownForce = 20f; // Downward dash force
    public float dashCooldown = 0.25f; // Time between dashes
    private bool isGrounded;
    private bool doubleJumped;
    private bool canDash = true;
    private bool canDashDown = true;
    private float lastDashTime;
    private float lastDashDownTime;

    public KeyCode leftKey = KeyCode.LeftArrow;
    public KeyCode rightKey = KeyCode.RightArrow;
    public KeyCode downKey = KeyCode.DownArrow;
    public KeyCode jumpKey = KeyCode.Space;

    private Rigidbody rb;
    private Animator animator; // Add reference to Animator

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>(); // Assign the Animator component

        if (rb == null || animator == null)
        {
            Debug.LogError("Rigidbody or Animator component is missing on the Player GameObject.");
        }
    }

    float GetHorizontalMovement()
    {
        float horizontalMovement = 0f;

        if (Input.GetKey(leftKey))
        {
            horizontalMovement -= 1f;
        }

        if (Input.GetKey(rightKey))
        {
            horizontalMovement += 1f;
        }

        return horizontalMovement;
    }

    void Update()
    {
        float horizontalMovement = GetHorizontalMovement();
        Vector3 movement = new Vector3(horizontalMovement, 0f, 0f) * movementSpeed * Time.deltaTime;
        transform.Translate(movement, Space.World);

        // Set player rotation based on the horizontal movement direction
        if (horizontalMovement != 0)
        {
            transform.rotation = Quaternion.LookRotation(new Vector3(horizontalMovement, 0f, 0f));
        }

        // Lateral dash
        if ((Input.GetKeyDown(leftKey) || Input.GetKeyDown(rightKey)) && canDash)
        {
            if (Time.time - lastDashTime < dashCooldown)
            {
                DashHorizontal(horizontalMovement > 0 ? Vector3.right : Vector3.left);
            }
            else
            {
                lastDashTime = Time.time;
            }
        }

        // Dash hacia abajo
        if (Input.GetKeyDown(downKey) && canDashDown)
        {
            if (Time.time - lastDashDownTime < dashCooldown)
            {
                DashDown();
            }
            else
            {
                lastDashDownTime = Time.time;
            }
        }

        // Jump
        if ((Input.GetKeyDown(jumpKey)) && (isGrounded || !doubleJumped))
        {
            if (isGrounded)
            {
                rb.velocity = new Vector3(rb.velocity.x, 0f, 0f);
                rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
                isGrounded = false;
                SetAnimation("Jump", true);
            }
            else // Double jump
            {
                rb.velocity = new Vector3(rb.velocity.x, 0f, 0f);
                rb.AddForce(Vector3.up * doubleJumpForce, ForceMode.Impulse);
                doubleJumped = true;
            }
        }

        // Walk animation
        SetAnimation("Walk", Mathf.Abs(horizontalMovement) > 0);

        // Idle animation
        SetAnimation("Idle", Mathf.Abs(horizontalMovement) == 0 && isGrounded);

        // Punch animation
        if (Input.GetKeyDown(KeyCode.P))
        {
            SetAnimation("Punch", true);
            ResetAnimation("Punch");
        }

        // Kick animation
        if (Input.GetKeyDown(KeyCode.K))
        {
            SetAnimation("Kick", true);
            ResetAnimation("Kick");
        }
    }

    // Lateral dash
    void DashHorizontal(Vector3 direction)
    {
        rb.velocity = new Vector3(direction.x * dashSpeed, rb.velocity.y, 0f);
        canDash = false;
        Invoke("ResetDash", dashCooldown);
    }

    // Downward dash
    void DashDown()
    {
        rb.velocity = new Vector3(rb.velocity.x, 0f, 0f);
        rb.AddForce(Vector3.down * dashDownForce, ForceMode.Impulse);
        canDashDown = false;
        Invoke("ResetDashDown", dashCooldown);
    }

    // Reset lateral dash after cooldown
    void ResetDash()
    {
        canDash = true;
    }

    // Reset dash after cooldown
    void ResetDashDown()
    {
        canDashDown = true;
    }

    // Called when the player touches the ground
    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
            doubleJumped = false;
            SetAnimation("Jump", false);
        }
    }

    // Set a boolean parameter in the animator
    void SetAnimation(string parameter, bool value)
    {
        animator.SetBool(parameter, value);
    }

    // Reset an animation trigger after a delay
    void ResetAnimation(string parameter)
    {
        StartCoroutine(ResetAnimationWithDelay(parameter, 0.5f));
    }

    // Coroutine to reset animation trigger after a delay
    System.Collections.IEnumerator ResetAnimationWithDelay(string parameter, float delay)
    {
        yield return new WaitForSeconds(delay);
        SetAnimation(parameter, false);
    }
}
