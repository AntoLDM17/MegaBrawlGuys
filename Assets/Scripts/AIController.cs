using UnityEngine;

public class AIController : MonoBehaviour
{
    public float detectionRange = 5f;
    public float punchRange = 2f;
    public float kickRange = 4f;
    public float stopDistance = 1.5f;
    public float attackCooldown = 2f;
    public float jumpThreshold = 2f; // Height difference to trigger a jump

    private GameObject player;
    private PlayerMovement playerMovement;
    private AIAttack aiAttack;
    private float lastAttackTime;
    private bool isGrounded;
    private Rigidbody rb;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        playerMovement = player.GetComponent<PlayerMovement>();
        aiAttack = GetComponent<AIAttack>();
        rb = GetComponent<Rigidbody>(); // Assign the Rigidbody component

        if (rb == null)
        {
            Debug.LogError("Rigidbody component is missing on the AI GameObject.");
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
                }
                else if (distanceToPlayer < kickRange)
                {
                    // Perform kick attack
                    aiAttack.Attack(50f);
                    lastAttackTime = Time.time;
                }
            }

            // Check if the player is above the AI and jump
            float heightDifference = player.transform.position.y - transform.position.y;
            if (heightDifference > 0 && heightDifference > jumpThreshold && isGrounded)
            {
                Jump();
            }
        }
    }

    void MoveTowardsPlayer()
    {
        // Check if the player is within the scenario limits
        if (IsPlayerWithinLimits())
        {
            Vector3 directionToPlayer = (player.transform.position - transform.position).normalized;
            float aiMovementSpeed = playerMovement.MovementSpeed;

            transform.Translate(directionToPlayer * aiMovementSpeed * Time.deltaTime);
        }
    }

    bool IsPlayerWithinLimits()
    {
        // Define your scenario limits here
        float minXLimit = -8f;
        float maxXLimit = 8f;

        // Check if the player's X position is within the limits
        float playerX = player.transform.position.x;
        return playerX >= minXLimit && playerX <= maxXLimit;
    }


    void Jump()
    {
        // Apply upward force for jumping
        float jumpForce = playerMovement.JumpForce;

        // Adjust the jump force to limit the jump height because if not the AI will skyrocket
        float limitedJumpForce = Mathf.Min(jumpForce, Mathf.Abs(transform.position.y - player.transform.position.y) * 0.1f);

        rb.velocity = new Vector3(rb.velocity.x, 0f, 0f);
        rb.AddForce(Vector3.up * limitedJumpForce, ForceMode.Impulse);
    }


    // Called when the AI touches the ground
    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
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
}
