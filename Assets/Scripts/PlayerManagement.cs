using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class PlayerManagement : MonoBehaviour
{
    public int startingLives = 3;
    public float maxPercentage = 300f;
    public float respawnTime = 2f;
    public float lowerScreenLimit = -10f; // Set the lower screen limit
    public TextMeshPro livesText;
    public TextMeshPro percentageText;

    private int currentLives;
    private float currentPercentage;
    private float dyingProbability = 1f;
    private bool isRespawning;
    private Rigidbody rb;

    void Start()
    {
        currentLives = Mathf.Max(0, startingLives);
        currentPercentage = 0f;
        isRespawning = false;

        // Get a reference to the Rigidbody component
        rb = GetComponent<Rigidbody>();

        UpdateUI();
    }

    void Update()
    {
        // Check if the player is below the lower screen limit
        if (transform.position.y < lowerScreenLimit)
        {
            HandleOutOfScreen();
        }
    }

    void TakeDamage(float damageAmount, Vector2 damageDirection)
    {
        if (!isRespawning)
        {
            currentPercentage += damageAmount;

            // If damage is over 100%, multiply dying probability and knockback by 1.5
            if (currentPercentage > 100f)
            {
                dyingProbability *= 1.5f;
                ApplyKnockback(5.0f + dyingProbability * 0.5f, damageDirection); // Adjust the knockback multiplier
            }
            else
            {
                ApplyKnockback(3.0f, damageDirection); // Use a default multiplier for knockback
            }

            // Update dying probability
            dyingProbability = Mathf.Min(100f, dyingProbability + (currentPercentage / maxPercentage));

            // Reset dying probability to 1% after dying
            if (currentLives > 0)
            {
                dyingProbability = 1f;
            }

            UpdateUI();
        }
    }

    void ApplyKnockback(float knockbackMultiplier, Vector2 damageDirection)
    {
        // Convert the 2D damage direction to 3D (assuming Z is fixed)
        Vector3 knockbackForce = new Vector3(damageDirection.x, damageDirection.y, 0f).normalized;

        // Adjust the knockback multiplier based on the dying probability
        float adjustedKnockbackMultiplier = knockbackMultiplier * (1f + dyingProbability * 0.5f);

        // Apply the knockback force to the Rigidbody component
        rb.AddForce(knockbackForce * adjustedKnockbackMultiplier, ForceMode.Impulse);
    }


    void Respawn()
    {
        // Disable the character temporarily during respawn
        isRespawning = true;
        gameObject.SetActive(false);

        // Reset damage and perform respawn logic after a delay
        Invoke("FinishRespawn", respawnTime);
    }


    void FinishRespawn()
    {
        // Reset specific character properties
        rb.velocity = Vector3.zero; // Reset the Rigidbody velocity
        rb.angularVelocity = Vector3.zero; // Reset the Rigidbody angular velocity

        // Reset general properties
        currentPercentage = 0f;
        isRespawning = false;

        // Reset the character to (0, 0, 0) coordinates
        transform.position = Vector3.zero;

        // Enable the character again
        gameObject.SetActive(true);

        UpdateUI();
    }

    void HandleOutOfScreen()
    {
        // We apply a penalty, such as reducing lives or restarting the game
        currentLives--;

        // Ensure lives do not go negative
        currentLives = Mathf.Max(0, currentLives);

        // Check if the character has more lives
        if (currentLives > 0)
        {
            // If there are lives left, initiate respawn
            Respawn();
        }
        else
        {
            // Game over logic (you can customize this based on your game design)
            Debug.Log("Game Over");
            RestartGame();
        }
    }

    void UpdateUI()
    {
        // Update TextMeshProUGUI components with current information
        livesText.text = "Lives: " + currentLives;
        percentageText.text = "Damage: " + Mathf.Round(currentPercentage) + "%";
    }

    void RestartGame()
    {
        // Restart the game (reload the current scene)
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
