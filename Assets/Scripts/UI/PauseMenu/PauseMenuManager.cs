using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PauseMenuManager : MonoBehaviour
{
    public GameObject pausePanel;

    private bool isPaused = false;
    public SettingsPanelController settingsController;

    private void Start()
    {
        if (pausePanel == null)
        {
            Debug.LogWarning("Pause panel is not assigned!");
            return;
        }

        pausePanel.SetActive(false);
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
