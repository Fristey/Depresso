using UnityEngine;

public class PauseMenu : MonoBehaviour
{

    [SerializeField] private GameObject pauseMenuUI;
    private bool isPaused;

    CamSwapManager camSwapManager;
    LookAround lookAround;

    private void Start()
    {
        camSwapManager = FindFirstObjectByType<CamSwapManager>();
        lookAround = FindFirstObjectByType<LookAround>();
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && !isPaused)
        {
            pauseMenuUI.SetActive(true);
            Time.timeScale = 0f; // Pause the game
            isPaused = true;
            Debug.Log("work");
            camSwapManager.enabled = false; // Disable camera swap manager
            lookAround.enabled = false; // Disable look around functionality
            Cursor.lockState = CursorLockMode.None; // Unlock the cursor
            Cursor.visible = true;
        }
        else if (Input.GetKeyDown(KeyCode.Space) && isPaused)
        {
            ResumeGame();
        }
    }

    public void ResumeGame()
    {
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f; // Resume the game
        isPaused = false;
        Debug.Log("Rusume Game");
        camSwapManager.enabled = true;
        lookAround.enabled = true;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    public void QuitGame()
    {
        Application.Quit(); // Quit the application
    }
}
