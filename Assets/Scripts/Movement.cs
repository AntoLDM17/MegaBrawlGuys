using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float movementSpeed = 5f;
    public float jumpForce = 10f;
    public float doubleJumpForce = 7.5f;
    public float dashSpeed = 11f; // Velocidad para el dash lateral
    public float dashDownForce = 20f; // Fuerza para el dash hacia abajo
    public float dashCooldown = 0.25f; // Tiempo de espera entre dashes
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

    void Start()
    {
        rb = GetComponent<Rigidbody>();
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
        // Horizontal movement
        float horizontalMovement = GetHorizontalMovement();
        Vector3 movement = new Vector3(horizontalMovement, 0f, 0f) * movementSpeed * Time.deltaTime;
        transform.Translate(movement);

        // Dash lateral
        if ((Input.GetKeyDown(leftKey) || Input.GetKeyDown(rightKey)) && canDash)
        {
            if (Time.time - lastDashTime < dashCooldown)
            {
                // Doble pulsación rápida lateral
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
                // Doble pulsación rápida hacia abajo
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
    void DashHorizontal(Vector3 direction)
    {
        rb.velocity = new Vector3(direction.x * dashSpeed, rb.velocity.y, 0f);
        canDash = false;
        Invoke("ResetDash", dashCooldown);
    }

    // Dash hacia abajo
    void DashDown()
    {
        rb.velocity = new Vector3(rb.velocity.x, 0f, 0f); // Reinicia la velocidad vertical
        rb.AddForce(Vector3.down * dashDownForce, ForceMode.Impulse);
        canDashDown = false;
        Invoke("ResetDashDown", dashCooldown);
    }

    // Resetear el dash lateral después del tiempo de espera
    void ResetDash()
    {
        canDash = true;
    }

    // Resetear el dash hacia abajo después del tiempo de espera
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
        }
    }
}
