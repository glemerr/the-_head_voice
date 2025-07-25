using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PauseMenuManager : MonoBehaviour
{
    public GameObject pausePanel;

    private bool isPaused = false;
    public SettingsPanelController settingsController;
    private FirstPersonLook lookScript;
    private FirstPersonMovement moveScript;
    private GunManager gunManager;
    public GameObject player; // Reference to the player GameObject, if needed
    private void Start()
    {
        if (pausePanel == null)
        {
            Debug.LogWarning("Pause panel is not assigned!");
            return;
        }

        pausePanel.SetActive(false);
        gunManager = player.GetComponentInChildren<GunManager>();
        lookScript = player.GetComponentInChildren<FirstPersonLook>();
        moveScript = player.GetComponentInChildren<FirstPersonMovement>();
        SetCursorState(false);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            TogglePause();
        }
    }

    public void TogglePause()
    {
        isPaused = !isPaused;

        Time.timeScale = isPaused ? 0f : 1f;
        pausePanel.SetActive(isPaused);
        SetCursorState(isPaused);
        gunManager.isPaused = isPaused; // Pause gun manager if needed
        lookScript.canLook = !isPaused;
        moveScript.canMove = !isPaused;
    }

    void SetCursorState(bool showCursor)
    {
        Cursor.visible = showCursor;
        Cursor.lockState = showCursor ? CursorLockMode.None : CursorLockMode.Locked;
    }

    public void OnResume()
    {
        if (isPaused)
        {
            TogglePause();
        }
    }

    public void OnMainMenu(string sceneName)
    {
        Time.timeScale = 1f; // Make sure time resumes
        SceneManager.LoadScene(sceneName);
    }

    public void OnExitGame()
    {
        Application.Quit();
    }
    
    public void OnSettings()
{
    Debug.Log("Open Settings...");
    settingsController.OpenSettings();
}
}
