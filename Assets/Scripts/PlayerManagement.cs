using UnityEngine;
using TMPro;

public class SmashBrosCharacter : MonoBehaviour
{
    public int startingLives = 3;
    public float maxPercentage = 100f;
    public float respawnTime = 2f;
    public TextMeshPro livesText;
    public TextMeshPro percentageText;

    private int currentLives;
    private float currentPercentage;
    private bool isRespawning;

    void Start()
    {
        currentLives = startingLives;
        currentPercentage = 0f;
        isRespawning = false;

        UpdateUI();
    }

    void Update()
    {
        // Example: Input for taking damage (you can replace this with your own damage logic)
        if (Input.GetKeyDown(KeyCode.D))
        {
            TakeDamage(20f); // Taking 20% damage for demonstration purposes
        }

        // Example: Input for respawning (you can replace this with your own respawn logic)
        if (Input.GetKeyDown(KeyCode.R))
        {
            Respawn();
        }
    }

    void TakeDamage(float damageAmount)
    {
        if (!isRespawning)
        {
            currentPercentage += damageAmount;

            // Check if the character is KO'd
            if (currentPercentage >= maxPercentage)
            {
                currentLives--;

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
                }
            }

            UpdateUI();
        }
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
        // Reset character properties
        currentPercentage = 0f;
        isRespawning = false;

        // Enable the character again
        gameObject.SetActive(true);

        UpdateUI();
    }

    void UpdateUI()
    {
        // Update TextMeshProUGUI components with current information
        livesText.text = "Lives: " + currentLives;
        percentageText.text = "Damage: " + Mathf.Round(currentPercentage) + "%";
    }
}
