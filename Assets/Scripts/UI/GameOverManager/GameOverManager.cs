using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using System.Collections;

public class GameOverManager : MonoBehaviour
{
    public GameObject gameOverUI;

    public TextMeshProUGUI storyText;       // Reference to a single UI text element
    public string[] storyChunks;            // Each chunk is a string
    private int index = 0;
    private float timer = 0f;
    public float delay = 6f;
    private bool isPaused = false;

    [Header("References to player")]
    private FirstPersonController playerController; // Reference to the player controller to pause movement
    private GunManager gunManager; // Reference to the gun manager, if needed
    public GameObject player; // Reference to the player GameObject, if needed
    

    [Header("Scene Fader")]
    public SceneFader sceneFader;

    void Update()
    {
        //Debug.Log("IsPause" + isPaused);
        if (!gameOverUI.activeSelf) return;

        timer += Time.unscaledDeltaTime;
        isPaused = true;
        gunManager.isPaused= isPaused; // Pause gun manager if needed
        //Debug.Log("GameOverManager Update called. IsPaused: " + gunManager.isPaused);
        SetCursorState(isPaused);
        if (isPaused)
        {
            playerController.cameraCanMove=false; // Disable player movement
        }
        else         {
            playerController.cameraCanMove=true; // Enable player movement
        }

        if (timer >= delay)
        {
            timer = 0;
            index = (index + 1) % storyChunks.Length;
            if (storyText != null && storyChunks.Length > 0)
            {
                storyText.text = storyChunks[index];
            }
        }
    }

    private void Start()
    {
        playerController = player.GetComponent<FirstPersonController>();
        gunManager = player.GetComponentInChildren<GunManager>();
        if (gunManager == null)
        {
            //Debug.LogError("GunManager not found in player GameObject. Please assign it in the inspector.");
        }
        //Debug.Log("GameOverManager Start called. PlayerController: " + playerController + ", GunManager: " + gunManager);
        if (gameOverUI != null)
            gameOverUI.SetActive(false);

        if (storyText != null && storyChunks.Length > 0)
            storyText.text = storyChunks[0];

        if (EventSystem.current == null)
            Debug.LogWarning("No EventSystem found. Buttons won't work.");
    }

    public void ShowGameOverScreen()
    {
        if (gameOverUI != null)
            gameOverUI.SetActive(true);

        SetCursorState(true);
        StartCoroutine(DelayPause());
    }

    private IEnumerator DelayPause()
    {
        yield return new WaitForEndOfFrame(); // Let UI display first
        Time.timeScale = 0f;
    }

    public void OnRestart(string sceneName)
    {
        Debug.Log("OnRestart button pressed for scene: " + sceneName);
        Time.timeScale = 1f;

        if (sceneFader != null)
        {
            sceneFader.FadeToScene(sceneName);
        }
        else
        {
            SceneManager.LoadScene(sceneName);
        }
    }

    public void OnMainMenu(string sceneName)
    {
        Debug.Log("OnMainMenu button pressed for scene: " + sceneName);
        Time.timeScale = 1f;

        if (sceneFader != null)
        {
            sceneFader.FadeToScene(sceneName);
        }
        else
        {
            SceneManager.LoadScene(sceneName);
        }
    }

    public void OnExit()
    {
        Debug.Log("Exiting application.");
        Application.Quit();
    }

    void SetCursorState(bool showCursor)
    {
        Cursor.visible = showCursor;
        Cursor.lockState = showCursor ? CursorLockMode.None : CursorLockMode.Locked;
    }
}