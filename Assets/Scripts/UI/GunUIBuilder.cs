using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GunUIBuilder : MonoBehaviour
{
    public Font defaultFont;
    public Sprite defaultWeaponIcon;
    public Sprite defaultButtonSprite;

    void Start()
    {
        BuildUI();
    }

    void BuildUI()
    {
        // Create Canvas
        GameObject canvasGO = new GameObject("GunUI_Canvas", typeof(Canvas), typeof(CanvasScaler), typeof(GraphicRaycaster));
        Canvas canvas = canvasGO.GetComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;

        CanvasScaler scaler = canvasGO.GetComponent<CanvasScaler>();
        scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
        scaler.referenceResolution = new Vector2(1920, 1080);

        // --- GunPanel ---
        GameObject gunPanel = CreateUIObject("GunPanel", canvasGO.transform);
        RectTransform gunRect = gunPanel.AddComponent<RectTransform>();
        gunRect.anchorMin = new Vector2(0.02f, 0.7f);
        gunRect.anchorMax = new Vector2(0.3f, 0.98f);
        gunRect.offsetMin = gunRect.offsetMax = Vector2.zero;
        gunPanel.AddComponent<Image>().color = new Color(0, 0, 0, 0.4f);

        // Weapon Icon
        Image icon = CreateImage("WeaponIcon", gunPanel.transform, defaultWeaponIcon);
        SetAnchored(icon.rectTransform, new Vector2(0.5f, 0.9f), new Vector2(100, 100));

        // Weapon Name
        TMP_Text nameText = CreateTMP("WeaponName", gunPanel.transform, "Weapon Name");
        SetAnchored(nameText.rectTransform, new Vector2(0.5f, 0.75f), new Vector2(300, 40));

        // --- Stats Panel ---
        GameObject statsPanel = CreateUIObject("StatsPanel", gunPanel.transform);
        SetAnchored(statsPanel.GetComponent<RectTransform>(), new Vector2(0.5f, 0.6f), new Vector2(300, 100));
        VerticalLayoutGroup layout = statsPanel.AddComponent<VerticalLayoutGroup>();
        layout.childControlHeight = layout.childControlWidth = true;
        layout.spacing = 10;

        CreateTMP("FireRateText", statsPanel.transform, "Fire Rate: 0.5s");
        CreateTMP("ReloadTimeText", statsPanel.transform, "Reload Time: 1.5s");
        CreateTMP("CooldownTimeText", statsPanel.transform, "Cooldown: 0.2s");

        // CooldownBar (radial)
        Image cooldownBar = CreateImage("CooldownBar", gunPanel.transform, null);
        cooldownBar.type = Image.Type.Filled;
        cooldownBar.fillMethod = Image.FillMethod.Radial360;
        cooldownBar.fillAmount = 1;
        SetAnchored(cooldownBar.rectTransform, new Vector2(0.9f, 0.9f), new Vector2(50, 50));
        cooldownBar.color = Color.cyan;

        // --- AmmoPanel ---
        GameObject ammoPanel = CreateUIObject("AmmoPanel", canvasGO.transform);
        SetAnchored(ammoPanel.GetComponent<RectTransform>(), new Vector2(0.8f, 0.1f), new Vector2(300, 100));

        TMP_Text ammoText = CreateTMP("AmmoText", ammoPanel.transform, "5 / 10");
        SetAnchored(ammoText.rectTransform, new Vector2(0.5f, 0.7f), new Vector2(200, 40));

        Slider reloadSlider = CreateSlider("ReloadProgressBar", ammoPanel.transform);
        SetAnchored(reloadSlider.GetComponent<RectTransform>(), new Vector2(0.5f, 0.3f), new Vector2(200, 20));

        // --- Weapon Selection Panel ---
        GameObject selectionPanel = CreateUIObject("WeaponSelectionPanel", canvasGO.transform);
        SetAnchored(selectionPanel.GetComponent<RectTransform>(), new Vector2(0.5f, 0.05f), new Vector2(600, 100));
        HorizontalLayoutGroup hGroup = selectionPanel.AddComponent<HorizontalLayoutGroup>();
        hGroup.spacing = 20;

        for (int i = 1; i <= 3; i++)
        {
            Button btn = CreateButton($"GunButton{i}", selectionPanel.transform, defaultButtonSprite, $"Gun {i}");
            btn.GetComponentInChildren<TMP_Text>().fontSize = 20;
        }
    }

    // ---------- UI Creation Helpers ----------

    GameObject CreateUIObject(string name, Transform parent)
    {
        GameObject obj = new GameObject(name, typeof(RectTransform));
        obj.transform.SetParent(parent, false);
        return obj;
    }

    TMP_Text CreateTMP(string name, Transform parent, string text)
    {
        GameObject go = new GameObject(name, typeof(RectTransform));
        go.transform.SetParent(parent, false);
        TMP_Text tmp = go.AddComponent<TextMeshProUGUI>();
        tmp.text = text;
        tmp.fontSize = 24;
        tmp.alignment = TextAlignmentOptions.Center;
        return tmp;
    }

    Image CreateImage(string name, Transform parent, Sprite sprite)
    {
        GameObject go = new GameObject(name, typeof(RectTransform));
        go.transform.SetParent(parent, false);
        Image img = go.AddComponent<Image>();
        img.sprite = sprite;
        img.preserveAspect = true;
        return img;
    }

    Slider CreateSlider(string name, Transform parent)
    {
        GameObject go = new GameObject(name, typeof(RectTransform));
        go.transform.SetParent(parent, false);
        Slider slider = go.AddComponent<Slider>();
        slider.value = 1;
        return slider;
    }

    Button CreateButton(string name, Transform parent, Sprite background, string buttonText)
    {
        GameObject go = new GameObject(name, typeof(RectTransform));
        go.transform.SetParent(parent, false);
        Image img = go.AddComponent<Image>();
        img.sprite = background;
        Button btn = go.AddComponent<Button>();

        TMP_Text txt = CreateTMP("Text", go.transform, buttonText);
        txt.alignment = TextAlignmentOptions.Center;
        txt.rectTransform.anchorMin = Vector2.zero;
        txt.rectTransform.anchorMax = Vector2.one;
        txt.rectTransform.offsetMin = txt.rectTransform.offsetMax = Vector2.zero;

        return btn;
    }

    void SetAnchored(RectTransform rect, Vector2 anchor, Vector2 size)
    {
        rect.anchorMin = rect.anchorMax = anchor;
        rect.sizeDelta = size;
        rect.anchoredPosition = Vector2.zero;
    }
}
