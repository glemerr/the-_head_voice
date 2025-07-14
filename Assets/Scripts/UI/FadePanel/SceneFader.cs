using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;

public class SceneFader : MonoBehaviour
{
    public CanvasGroup fadeCanvas;
    public float fadeDuration = 1f;

    void Start()
    {
        if (fadeCanvas != null)
            StartCoroutine(FadeIn());
    }

    public void FadeToScene(string sceneName)
    {
        StartCoroutine(FadeOut(sceneName));
    }

    IEnumerator FadeIn()
    {
        fadeCanvas.alpha = 1;
        float timer = 0;
        while (timer < fadeDuration)
        {
            timer += Time.deltaTime;
            fadeCanvas.alpha = 1 - timer / fadeDuration;
            yield return null;
        }
        fadeCanvas.alpha = 0;
    }

    IEnumerator FadeOut(string sceneName)
    {
        fadeCanvas.blocksRaycasts = true;
        float timer = 0;
        while (timer < fadeDuration)
        {
            timer += Time.deltaTime;
            fadeCanvas.alpha = timer / fadeDuration;
            yield return null;
        }
        fadeCanvas.alpha = 1;
        SceneManager.LoadScene(sceneName);
    }
}
