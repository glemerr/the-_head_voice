using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using System.Collections;

public class LoadingScreenManager : MonoBehaviour
{
    [Header("UI Elements")]
    public TextMeshProUGUI titleText;
    public TextMeshProUGUI subtitleText;
    public Slider loadingBar;
    public TextMeshProUGUI loadingPercentageText;
    public TextMeshProUGUI carouselText;
    public TextMeshProUGUI statsText;
    public TextMeshProUGUI warningText;

    [Header("Carousel Settings")]
    [TextArea(3, 10)]
    [Tooltip("Textos narrados por La Abuela (se elige uno al azar).")]
    public string[] historyTexts = new string[]
    {
        "Lucy, cuando era niña, vivió el abandono de su madre como un frío abismo: largas noches en vela, preguntas sin respuesta y un vacío que nunca cicatrizó por completo.",
        "Hoy, en este mundo incierto, enfrenta monstruos nacidos de su ansiedad: cada disparo, cada salto, es un reto a su paciencia y a su propia resistencia mental.",
        "Aquí no hay dos partidas iguales: las armas brotan del aire, los enemigos surgen sin aviso y los poderes cambian con la suerte. Sólo la voluntad de Lucy puede domar este caos aleatorio.",
        "Aunque el pasado la marcó con cicatrices de soledad y miedo, en cada nivel aprende a transformar su dolor en determinación. La esperanza florece en medio del desorden.",
        "De niña, Lucy aprendió a sobrevivir al abandono de su madre, encerrándose en sus propios pensamientos mientras su padre se perdía entre excesos. Aquellas noches de llanto sigiloso forjaron en ella una resiliencia que ahora late con fuerza renovada ante cada prueba.",
        "Hoy, cada suspiro de ansiedad se convierte en un enemigo intangible que acecha sus pasos: sombras que distorsionan la realidad, voces internas que susurran dudas. Sin embargo, con cada victoria fragmenta un poco más ese miedo y avanza confiada hacia su sanación.",
        "En cada partida, la fortuna decide su destino: el arma que blande, el enemigo que enfrenta y el poder que adquiere son decretados por el caos. Esta aleatoriedad obstinada reta su ingenio y resistencia, obligándola a adaptarse sin descanso.",
        "Y sin embargo, entre fallos e imprevisibilidad, florece la esperanza. Guiada por la sabiduría de La Abuela y sostenida por su determinación, Lucy forja su propio camino. Cada ciclo fallido enseña lecciones hasta que, un día, domine este mundo de pesadilla."
    };
    public float carouselInterval = 5f;

    private float loadingProgress = 0f;
    public SceneFader sceneFader;

    void Start()
    {
        titleText.text             = "VOID";
        subtitleText.text          = "Loading nightmare...";
        warningText.text           = "";

        StartCoroutine(SimulateLoading());
        StartCoroutine(CarouselTextUpdate());
    }

    IEnumerator SimulateLoading()
    {
        while (loadingProgress < 1f)
        {
            loadingProgress += Time.deltaTime * 0.2f; // velocidad de carga simulada
            loadingBar.value = loadingProgress;
            loadingPercentageText.text = Mathf.RoundToInt(loadingProgress * 100f) + "%";
            UpdateStatsDisplay();
            yield return null;
        }

        loadingPercentageText.text = "100%";
        subtitleText.text = "Loading existential dread...";
        warningText.text  = "<color=red>WARNING: PSYCHOLOGICAL DAMAGE IMMINENT</color>";

        // Transición a la siguiente escena
        if (sceneFader != null)
            sceneFader.FadeToScene("PantanoLevel");
        else
            SceneManager.LoadScene("PantanoLevel");
    }

    IEnumerator CarouselTextUpdate()
    {
        while (true)
        {
            if (historyTexts != null && historyTexts.Length > 0)
            {
                int randomIndex = Random.Range(0, historyTexts.Length);
                carouselText.text = historyTexts[randomIndex];
            }
            yield return new WaitForSeconds(carouselInterval);
        }
    }

    void UpdateStatsDisplay()
    {
        int mem    = Mathf.Clamp(Mathf.RoundToInt(80f * loadingProgress), 0, 80);
        int sanity = Mathf.Clamp(Mathf.RoundToInt(20f * loadingProgress), 0, 20);
        int hope   = Mathf.Clamp(Mathf.RoundToInt(10f * loadingProgress), 0, 10);

        statsText.text = $"MEMORY: {mem}%   SANITY: {sanity}%   HOPE: {hope}%";
    }
}
