using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class DamageEffects : MonoBehaviour
{
    [Header("Damage Effects")]
    public Image damageOverlay;
    public float flashDuration = 0.3f;
    public ParticleSystem bloodEffect;
    [Range(0f, 2f)] public float transparentCenterRadius = 0.9f; // 20% del tamaño
    [Range(0f, 1f)] public float edgeFadeWidth = 0.1f;          // cuánto tarda en degradar de 0 a 1
    
    private LifeSystem lifeSystem;
    public Camera playerCamera;
    void Start()
    {
        lifeSystem = GetComponent<LifeSystem>();
        lifeSystem.OnValueChanged.AddListener(OnValueChangedHandler);

        // Creamos y asignamos la textura radial
        var tex = CreateRadialAlphaTexture(
            (int)damageOverlay.rectTransform.rect.width,
            (int)damageOverlay.rectTransform.rect.height,
            transparentCenterRadius,
            edgeFadeWidth);
        var sprite = Sprite.Create(tex, new Rect(0, 0, tex.width, tex.height), Vector2.one * 0.5f);
        damageOverlay.sprite = sprite;
        damageOverlay.color = new Color(0, 0, 0, 0); // empezamos transparente
    }

    private void OnValueChangedHandler(float damage)
    {
        ShowDamageEffects();
    }

    public void ShowDamageEffects()
    {
        StartCoroutine(FlashDamage());
        if (bloodEffect != null) bloodEffect.Play();
    }

    IEnumerator FlashDamage()
    {
        // subimos la opacidad al máximo
        damageOverlay.color = new Color(0, 0, 0, 0.6f);
        yield return new WaitForSeconds(flashDuration);
        damageOverlay.color = Color.clear;
    }

    Texture2D CreateRadialAlphaTexture(int w, int h, float innerR, float fadeW)
    {
        Texture2D tex = new Texture2D(w, h, TextureFormat.ARGB32, false);
        tex.wrapMode = TextureWrapMode.Clamp;

        Vector2 center = new Vector2(w, h) * 0.5f;
        float maxDist = Mathf.Min(w, h) * 0.5f;

        for (int y = 0; y < h; y++)
        {
            for (int x = 0; x < w; x++)
            {
                float dist = Vector2.Distance(new Vector2(x, y), center);
                float t = (dist / maxDist - innerR) / fadeW;
                // t<0 -> dentro del círculo transparente; t>1 -> borde completamente opaco
                float alpha = Mathf.Clamp01(t);
                tex.SetPixel(x, y, new Color(0, 0, 0, alpha));
            }
        }
        tex.Apply();
        return tex;
    }

    // void ShowDamageDirection(Vector3 damageSourcePosition)
    // {
    //     if(directionIndicatorPrefab == null) return;
        
    //     Canvas canvas = FindObjectOfType<Canvas>();
    //     if(canvas == null) 
    //     {
    //         Debug.LogError("No Canvas found in scene!");
    //         return;
    //     }
        
    //     GameObject indicator = Instantiate(directionIndicatorPrefab, canvas.transform);
    //     DamageIndicator indicatorScript = indicator.GetComponent<DamageIndicator>();
    //     if(indicatorScript != null)
    //     {
    //         indicatorScript.Initialize(playerCamera, damageSourcePosition, indicatorDuration);
    //     }
    // }

}