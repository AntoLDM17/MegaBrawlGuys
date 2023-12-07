using UnityEngine;

public class AIControllerANIMATED : MonoBehaviour
{
    public float detectionRange = 10f;
    public float punchRange = 2f;
    public float kickRange = 4f;
    public float stopDistance = 1.9f;
    public float attackCooldown = 2f;
    public float jumpThreshold = 2f; // Height difference to trigger a jump
    public float dashCooldown = 15f; // Cooldown for lateral dash

    private GameObject player;
    private PlayerMovement playerMovement;
    private AIAttack aiAttack;
    private Animator animator; // Add reference to Animator
    private float lastAttackTime;
    private bool isGrounded;
    private bool canDoubleJump = true;
    private bool canDash = true; // Variable to track if AI can dash
    private Rigidbody rb;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        playerMovement = player.GetComponent<PlayerMovement>();
        aiAttack = GetComponent<AIAttack>();
        rb = GetComponent<Rigidbody>(); // Assign the Rigidbody component
        animator = GetComponent<Animator>(); // Assign the Animator component

        if (rb == null || animator == null)
        {
            Debug.LogError("Rigidbody or Animator component is missing on the AI GameObject.");
        }
    }

    void Update()
    {
        float distanceToPlayer = Vector3.Distance(transform.position, player.transform.position);

        if (distanceToPlayer < detectionRange)
        {
            if (distanceToPlayer > stopDistance)
            {
                MoveTowardsPlayer();
            }

            if (Time.time - lastAttackTime >= attackCooldown)
            {
                if (distanceToPlayer < punchRange)
                {
                    // Perform punch attack
                    aiAttack.Attack(30f);
                    lastAttackTime = Time.time;

                    // Trigger Punch animation
                    animator.SetBool("Punch", true);
                }
                else if (distanceToPlayer < kickRange)
                {
                    // Perform kick attack
                    aiAttack.Attack(50f);
                    lastAttackTime = Time.time;

                    // Trigger Kick animation
                    animator.SetBool("Kick", true);
                }
            }

            // Check if the player is above the AI and jump
            float heightDifference = player.transform.position.y - transform.position.y;
            if (heightDifference > 0 && heightDifference > jumpThreshold && isGrounded)
            {
                Jump();

                // Trigger Jump animation
                animator.SetBool("Jump", true);
            }
        }

        // Check if the AI is out of the scenario and double jump with lateral dash to get back in
        if (!AmIWithinLimits() && canDoubleJump)
        {
            DoubleJumpAndLateralDash();
        }
    }

    void MoveTowardsPlayer()
    {
        // Check if the player is within the scenario limits
        if (IsPlayerWithinLimits())
        {
            Vector3 directionToPlayer = (player.transform.position - transform.position).normalized;
            float aiMovementSpeed = playerMovement.MovementSpeed;
            Vector3 directionToPlayerX = new Vector3(directionToPlayer.x, 0f, 0f).normalized;

            // Use Space.World to move in the world space
            transform.Translate(directionToPlayer * aiMovementSpeed * Time.deltaTime, Space.World);

            // Make the AI face the direction it is heading (only in the x-z plane)
            transform.LookAt(transform.position + directionToPlayerX, Vector3.up);
        }
    }

    void Jump()
    {
        float doubleJumpForce = playerMovement.DoubleJumpForce;
        float jumpForce = isGrounded ? playerMovement.JumpForce : doubleJumpForce;

        rb.velocity = new Vector3(rb.velocity.x, 0f, 0f);
        // Limit the jump force based on the height difference between the AI and the player
        float limitedJumpForce = Mathf.Min(jumpForce, Mathf.Abs(transform.position.y - player.transform.position.y) * 0.1f);
        rb.AddForce(Vector3.up * limitedJumpForce, ForceMode.Impulse);

        // Disable double jump after the first jump
        canDoubleJump = false;
    }

    void DoubleJumpAndLateralDash()
    {
        float lateralDashForce = playerMovement.DashSpeed;

        // Double jump
        Jump();

        // Check if the AI can dash
        if (canDash)
        {
            // Calculate lateral dash direction based on the difference between the AI's position and the origin
            Vector3 lateralDashDirection = -transform.position.normalized;

            // Apply lateral force for dashing
            rb.velocity = new Vector3(lateralDashDirection.x * lateralDashForce, rb.velocity.y, 0f);

            // Set canDash to false and start dash cooldown
            canDash = false;
            Invoke("ResetDash", dashCooldown);
        }

        canDoubleJump = true; // Reset double jump after the lateral dash
    }

    void ResetDash()
    {
        canDash = true; // Reset canDash after the dash cooldown
    }

    // Called when the AI touches the ground
    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
            canDoubleJump = true; // Reset double jump on landing
            animator.SetBool("Jump", false); // Reset Jump animation
        }
    }

    // Called when the AI leaves the ground
    void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = false;
        }
    }

    // Check if the player is within the scenario limits
    bool IsPlayerWithinLimits()
    {
        float minXLimit = -10f; // Adjust as needed
        float maxXLimit = 10f; // Adjust as needed
        float playerX = player.transform.position.x;
        return playerX >= minXLimit && playerX <= maxXLimit;
    }

    bool AmIWithinLimits()
    {
        float minXLimit = -9f; // Adjust as needed
        float maxXLimit = 9f; // Adjust as needed
        float aiX = transform.position.x;
        return aiX >= minXLimit && aiX <= maxXLimit;
    }
}

