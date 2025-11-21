using UnityEngine;
using UnityEngine.SceneManagement; // For scene management
using UnityEngine.UI; // For UI button references

public class MenuScript : MonoBehaviour
{
    public GameObject mainMenuPanel; // The main menu panel (e.g., Start, About, Quit buttons)
    public Button ballSlidingStartbutton;      // Ball Sliding Start Button
    // public GameObject aboutPanel;    // The About panel with game information
    // public GameObject gameUI;       // Game UI (e.g., HUD, timer, etc.)
    // public GameObject pauseMenuPanel; // Pause menu for in-game

    void Start()
    {
        mainMenuPanel.SetActive(true); // Show the main menu at the start
        ballSlidingStartbutton.gameObject.SetActive(false); // Show the Ball Sliding Start Button
        // Ensure the game UI is hidden at the start, only showing the main menu
        // gameUI.SetActive(false);
        // aboutPanel.SetActive(false); // Hide the About panel initially
        // pauseMenuPanel.SetActive(false); // Hide the pause menu initially
    }

    // Called when the Start button is clicked
    public void OnStartButtonClicked()
    {
        mainMenuPanel.SetActive(false);  // Hide the main menu
        ballSlidingStartbutton.gameObject.SetActive(true); // Show the Ball Sliding Start Button
        // gameUI.SetActive(true);          // Show the in-game UI (gameplay HUD)
        // Load the first scene of the game (make sure the name matches your game scene name)
        // SceneManager.LoadScene("GameScene");
    }

    // Called when the About button is clicked
    // public void OnAboutButtonClicked()
    // {
    //     aboutPanel.SetActive(true);     // Show the About panel
    //     mainMenuPanel.SetActive(false); // Hide the main menu
    // }

    // Called when the Close About button is clicked
    // public void OnCloseAboutButtonClicked()
    // {
    //     aboutPanel.SetActive(false);    // Hide the About panel
    //     mainMenuPanel.SetActive(true);  // Show the main menu again
    // }

    // Called when the Quit button is clicked
    public void OnQuitButtonClicked()
    {
        // Close the game
        Application.Quit();
        Debug.Log("Game is quitting...");
    }

    // Called when the Restart button is clicked (in case of restarting the scene)
    public void OnRestartButtonClicked()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name); // Reload the current scene
    }

    // Called when the Pause button is clicked during gameplay
    // public void OnPauseButtonClicked()
    // {
    //     Time.timeScale = 0f; // Pause the game
    //     pauseMenuPanel.SetActive(true); // Show the pause menu
    // }

    // Called when the Resume button is clicked in the Pause Menu
    // public void OnResumeButtonClicked()
    // {
    //     Time.timeScale = 1f; // Resume the game
    //     pauseMenuPanel.SetActive(false); // Hide the pause menu
    // }
}
