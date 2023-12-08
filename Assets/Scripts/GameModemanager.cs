// GameModeManager.cs
using UnityEngine;

public class GameModeManager : MonoBehaviour
{
    public static GameModeManager Instance;

    public enum GameMode
    {
        PlayerVsPlayer,
        PlayerVsAI
    }

    public GameMode currentGameMode;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        // You can remove the following line since you want the configuration to be done in the Unity Editor

    }

    // Make this method public so you can call it from other scripts or from Unity events
    public void SetGameMode(GameMode newGameMode)
    {
        currentGameMode = newGameMode;

        // Perform actions based on the game mode
        switch (currentGameMode)
        {
            case GameMode.PlayerVsPlayer:
                // Disable AI components
                DisableAI();
                break;
            case GameMode.PlayerVsAI:
                // Enable AI components
                EnableAI();
                break;
            default:
                break;
        }
    }

    private void DisableAI()
    {
        // Disable AI components
        AIController[] aiControllers = FindObjectsOfType<AIController>();
        foreach (var aiController in aiControllers)
        {
            aiController.enabled = false;
        }

        AIAttack[] aiAttacks = FindObjectsOfType<AIAttack>();
        foreach (var aiAttack in aiAttacks)
        {
            aiAttack.enabled = false;
        }
    }

    private void EnableAI()
    {
        // Enable AI components
        AIController[] aiControllers = FindObjectsOfType<AIController>();
        foreach (var aiController in aiControllers)
        {
            aiController.enabled = true;
        }

        AIAttack[] aiAttacks = FindObjectsOfType<AIAttack>();
        foreach (var aiAttack in aiAttacks)
        {
            aiAttack.enabled = true;
        }
    }
}
