using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GunUIStyler : MonoBehaviour
{
    [Header("Gun UI Elements")]
    public Image panelBackground;
    public Image weaponIcon;
    public TextMeshProUGUI weaponName;
    public TextMeshProUGUI fireRateText;
    public TextMeshProUGUI reloadTimeText;
    public TextMeshProUGUI cooldownText;
    public Image cooldownRadial;
    public TextMeshProUGUI ammoText;
    public Slider reloadSlider;
    public Button[] weaponButtons;

    // Color palette
    private Color neonCyan = new Color32(0, 246, 255, 255);
    private Color neonMagenta = new Color32(255, 0, 212, 255);
    private Color neonYellow = new Color32(255, 235, 59, 255);
    private Color neonRed = new Color32(255, 76, 76, 255);
    private Color backgroundDark = new Color32(10, 15, 31, 128); // Semi-transparent
    private Color white = Color.white;
    private Color gray = new Color32(85, 85, 85, 255);

    void Start()
    {
        ApplyStyles();
    }

    public void ApplyStyles()
    {
        // Background Panel
        if (panelBackground)
        {
            panelBackground.color = backgroundDark;
        }

        // Weapon Icon
        if (weaponIcon)
        {
            weaponIcon.preserveAspect = true;
            weaponIcon.color = white;
        }

        // Texts
        SetTextStyle(weaponName, neonCyan, 30);
        SetTextStyle(fireRateText, white, 20);
        SetTextStyle(reloadTimeText, white, 20);
        SetTextStyle(cooldownText, neonRed, 20);
        SetTextStyle(ammoText, white, 26);

        // Cooldown radial
        if (cooldownRadial)
        {
            cooldownRadial.type = Image.Type.Filled;
            cooldownRadial.fillMethod = Image.FillMethod.Radial360;
            cooldownRadial.fillOrigin = (int)Image.Origin360.Top;
            cooldownRadial.color = neonRed;
        }

        // Reload Slider
        if (reloadSlider)
        {
            var fill = reloadSlider.fillRect?.GetComponent<Image>();
            var bg = reloadSlider.GetComponentInChildren<Image>();

            if (fill) fill.color = neonMagenta;
            if (bg) bg.color = gray;
        }

        // Weapon Buttons
        foreach (var btn in weaponButtons)
        {
            var img = btn.GetComponent<Image>();
            var txt = btn.GetComponentInChildren<TextMeshProUGUI>();
            if (img) img.color = new Color32(20, 20, 20, 255);
            if (txt) SetTextStyle(txt, neonCyan, 18);
        }
    }

    void SetTextStyle(TextMeshProUGUI text, Color color, float size)
    {
        if (text == null) return;
        text.color = color;
        text.fontSize = size;
        text.fontStyle = FontStyles.UpperCase;
        text.textWrappingMode = TextWrappingModes.NoWrap;
    }
}
