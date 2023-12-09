using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public GameObject player1;
    public GameObject player2;
    public GameObject start1vs1Button;
    public GameObject start1vsAIButton;
    public GameObject sceneObjects;
    public GameObject lifes1;
    public GameObject lifes2;
    public GameObject damage1;
    public GameObject damage2;
    public GameObject menuImage;




    void Start()
    {
        // Initially, show the buttons and hide the players
        ShowButtons();
        HidePlayers();
    }

    public void Start1vs1()
    {
        ShowPlayers();
        // Enable components for 1vs1
        EnablePlayerComponents(player1, true);
        EnablePlayerComponents(player2, true);

        // Hide buttons
        HideButtons();


    }

    public void Start1vsAI()
    {
        ShowPlayers();
        // Enable components for 1vsAI
        EnablePlayerComponents(player1, true);
        EnableAIComponents(player2, true);

        // Hide buttons
        HideButtons();

    }

    void EnablePlayerComponents(GameObject player, bool enableAI)
    {
        PlayerManagement playerManagement = player.GetComponent<PlayerManagement>();
        PlayerMovement playerMovement = player.GetComponent<PlayerMovement>();
        PlayerAttack playerAttack = player.GetComponent<PlayerAttack>();

        if (playerManagement != null)
            playerManagement.enabled = true;

        if (playerMovement != null)
            playerMovement.enabled = true;

        if (playerAttack != null)
            playerAttack.enabled = true;
    }

    void EnableAIComponents(GameObject player, bool enableAI)
    {
        AIAttack aiAttack = player.GetComponent<AIAttack>();
        AIController aiController = player.GetComponent<AIController>();
        PlayerManagement playerManagement = player.GetComponent<PlayerManagement>();
        PlayerMovement playerMovement = player.GetComponent<PlayerMovement>();
        PlayerAttack playerAttack = player.GetComponent<PlayerAttack>();

        // Enable AI components and disable/enable player components as needed
        if (aiAttack != null)
            aiAttack.enabled = enableAI;

        if (aiController != null)
            aiController.enabled = enableAI;

        if (playerManagement != null)
            playerManagement.enabled = enableAI;

        if (playerMovement != null)
            playerMovement.enabled = !enableAI;

        if (playerAttack != null)
            playerAttack.enabled = !enableAI;
    }


    void ShowButtons()
    {
        start1vs1Button.SetActive(true);
        start1vsAIButton.SetActive(true);
        menuImage.SetActive(true);
    }

    void ShowPlayers()
    {
        player1.SetActive(true);
        player2.SetActive(true);
        sceneObjects.SetActive(true);
        lifes1.SetActive(true);
        lifes2.SetActive(true);
        damage1.SetActive(true);
        damage2.SetActive(true);
    }

    void HideButtons()
    {
        start1vs1Button.SetActive(false);
        start1vsAIButton.SetActive(false);
        menuImage.SetActive(false);
    }

    void HidePlayers()
    {
        player1.SetActive(false);
        player2.SetActive(false);
        sceneObjects.SetActive(false);
        lifes1.SetActive(false);
        lifes2.SetActive(false);
        damage1.SetActive(false);
        damage2.SetActive(false);


    }


    public void ShowButtonsOnGameOver()
    {
        // Show buttons when the game is over
        ShowButtons();
    }
}
