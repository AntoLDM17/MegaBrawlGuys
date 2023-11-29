using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    public KeyCode punchKey = KeyCode.Space; // Keycode for punching
    public KeyCode kickKey = KeyCode.LeftControl; // Keycode for kicking
    public float punchingRange = 2f;
    public float kickingRange = 4f;

    private PlayerManagement playerManagement;

    void Start()
    {
        // Get a reference to the PlayerManagement script
        playerManagement = GetComponent<PlayerManagement>();
    }

    void Update()
    {
        // Check for punch input
        if (Input.GetKeyDown(punchKey))
        {
            // Perform the punch
            if (IsInRange(punchingRange))
            {
                // Deal damage for punching
                Attack(30f);
            }
        }

        // Check for kick input
        if (Input.GetKeyDown(kickKey))
        {
            // Perform the kick
            if (IsInRange(kickingRange))
            {
                // Deal damage for kicking
                Attack(50f);
            }
        }
    }

    void Attack(float damage)
    {
        // Use reflection to access the private TakeDamage method
        var takeDamageMethod = typeof(PlayerManagement).GetMethod("TakeDamage", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);

        if (takeDamageMethod != null)
        {
            // Check if there are other players in range
            Collider[] hitColliders = Physics.OverlapSphere(transform.position, kickingRange);
            foreach (Collider collider in hitColliders)
            {
                // Ignore collisions with itself
                if (collider.gameObject != gameObject)
                {
                    // Check if the other GameObject has a PlayerManagement script
                    PlayerManagement otherPlayer = collider.GetComponent<PlayerManagement>();
                    if (otherPlayer != null)
                    {
                        // Inflict damage on the other player using reflection
                        takeDamageMethod.Invoke(otherPlayer, new object[] { damage, GetAttackDirection(otherPlayer.transform.position) });
                    }
                }
            }
        }
    }

    bool IsInRange(float range)
    {
        // Check if there are other players in range
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, range);
        foreach (Collider collider in hitColliders)
        {
            // Ignore collisions with itself
            if (collider.gameObject != gameObject)
            {
                return true;
            }
        }
        return false;
    }

    Vector2 GetAttackDirection(Vector3 targetPosition)
    {
        // Calculate the direction from the attacker to the target
        return (targetPosition - transform.position).normalized;
    }
}

