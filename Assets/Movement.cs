using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float movementSpeed = 5f;
    public float jumpForce = 10f;
    public float doubleJumpForce = 5f;
    public float dashSpeed = 15f; // Velocidad para el dash lateral
    public float dashCooldown = 0.5f; // Tiempo de espera entre dashes
    private bool isGrounded;
    private bool doubleJumped;
    private bool canDash = true;
    private float lastDashTime;

    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        // Horizontal movement
        float horizontalMovement = Input.GetAxis("Horizontal");
        Vector3 movement = new Vector3(horizontalMovement, 0f, 0f) * movementSpeed * Time.deltaTime;
        transform.Translate(movement);

        // Dash lateral
        if ((Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.RightArrow)) && canDash)
        {
            if (Time.time - lastDashTime < dashCooldown)
            {
                // Doble pulsación rápida
                if (Mathf.Abs(horizontalMovement) > 0.1f)
                {
                    Dash(horizontalMovement > 0 ? Vector3.right : Vector3.left);
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
            else // Doble salto
            {
                rb.velocity = new Vector3(rb.velocity.x, 0f, 0f);
                rb.AddForce(Vector3.up * doubleJumpForce, ForceMode.Impulse);
                doubleJumped = true;
            }
        }
    }

    // Dash lateral
    void Dash(Vector3 direction)
    {
        rb.velocity = new Vector3(direction.x * dashSpeed, rb.velocity.y, 0f);
        canDash = false;
        Invoke("ResetDash", dashCooldown);
    }

    // Resetear el dash después del tiempo de espera
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
