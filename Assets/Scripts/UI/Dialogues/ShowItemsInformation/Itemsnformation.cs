using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

public class ItemsInformation : MonoBehaviour
{
    [Header("Referencias")]
    public ItemInventory itemInventory;         // Tu componente de inventario
    public GameObject infoPanel;               // Panel raíz
    public List<Button> weaponButtons;         // Botones de inventario en la parte inferior

    [Header("Central Panel")]
    public Image centralItemIcon;              // Icono grande del ítem seleccionado
    public TextMeshProUGUI titleText;          // Nombre del ítem / mensajes genéricos
    public TextMeshProUGUI subtitleText;       // Descripción del ítem

    [Header("Control de Cámara")]
    public Camera playerCamera;                // La cámara principal

    // Estado interno
    private bool isPaused = false;
    private int selectedIndex = 0;
    private Vector3 camSavedPosition;
    private Quaternion camSavedRotation;

    private void Awake()
    {
        infoPanel.SetActive(false);
        if (itemInventory == null)
            itemInventory = FindFirstObjectByType<ItemInventory>();
        if (playerCamera == null)
            playerCamera = Camera.main;
    }

    private void Update()
    {
        // Toggle pausa con I
        if (Input.GetKeyDown(KeyCode.I))
        {
            if (isPaused) Resume();
            else Pause();
        }

        if (!isPaused) return;

        // Navegar con D / A
        if (Input.GetKeyDown(KeyCode.D)|| Input.GetKeyDown(KeyCode.RightArrow)) NextItem();
        if (Input.GetKeyDown(KeyCode.A)|| Input.GetKeyDown(KeyCode.LeftArrow)) PreviousItem();

        // Mantener fija la cámara
        if (playerCamera != null)
        {
            playerCamera.transform.position = camSavedPosition;
            playerCamera.transform.rotation = camSavedRotation;
        }
    }

    private void Pause()
    {
        isPaused = true;
        Time.timeScale = 0f;
        infoPanel.SetActive(true);

        // Guardar posición y rotación de cámara
        if (playerCamera != null)
        {
            camSavedPosition = playerCamera.transform.position;
            camSavedRotation = playerCamera.transform.rotation;
        }

        ShowItemsInformation();
    }

    private void Resume()
    {
        isPaused = false;
        Time.timeScale = 1f;
        infoPanel.SetActive(false);
    }

    private void ShowItemsInformation()
    {
        var list = itemInventory.itemCountsList;
        if (list == null || list.Count == 0)
        {
            // Inventario vacío
            centralItemIcon.gameObject.SetActive(false);
            titleText.text    = "No tienes ningún ítem";
            subtitleText.text = "Tu inventario está vacío.";
            HideAllButtons();
            return;
        }

        // Hay ítems: elegir primero por defecto
        selectedIndex = 0;
        SetupWeaponButtons();
        UpdateCentralDisplay();
    }

    private void SetupWeaponButtons()
    {
        var list = itemInventory.itemCountsList;
        int count = Mathf.Min(weaponButtons.Count, list.Count);

        for (int i = 0; i < count; i++)
        {
            var btn  = weaponButtons[i];
            var data = list[i];
            var img  = btn.GetComponentInChildren<Image>();

            // Icono de cada botón
            img.sprite = data.item.itemIcon;
            btn.gameObject.SetActive(true);

            // Click selecciona ítem
            int index = i;
            btn.onClick.RemoveAllListeners();
            btn.onClick.AddListener(() =>
            {
                selectedIndex = index;
                UpdateCentralDisplay();
            });
        }

        // Desactivar botones sobrantes
        for (int i = count; i < weaponButtons.Count; i++)
            weaponButtons[i].gameObject.SetActive(false);
    }

    private void HideAllButtons()
    {
        foreach (var btn in weaponButtons)
            btn.gameObject.SetActive(false);
    }

    private void UpdateCentralDisplay()
    {
        var data = itemInventory.itemCountsList[selectedIndex];
        var item = data.item;

        // Icono grande
        centralItemIcon.gameObject.SetActive(true);
        centralItemIcon.sprite = item.itemIcon;

        // Textos
        titleText.text    = item.itemName;
        subtitleText.text = item.itemDescription;

        // Visualizar botones inferiores:
        // - El seleccionado más grande y opaco (alpha = 1)
        // - Los no seleccionados más pequeños y semitransparentes (alpha = 0.5)
        for (int i = 0; i < weaponButtons.Count; i++)
        {
            var btn      = weaponButtons[i];
            var img      = btn.GetComponentInChildren<Image>();
            var rt       = img.rectTransform;
            bool isSel   = (i == selectedIndex);

            // Escala
            rt.localScale = isSel ? Vector3.one * 1.2f : Vector3.one * 0.8f;
            // Opacidad
            var c = img.color;
            c.a = isSel ? 1f : 0.8f;
            img.color = c;
        }
    }

    private void NextItem()
    {
        var list = itemInventory.itemCountsList;
        selectedIndex = (selectedIndex + 1) % list.Count;
        UpdateCentralDisplay();
    }

    private void PreviousItem()
    {
        var list = itemInventory.itemCountsList;
        selectedIndex = (selectedIndex - 1 + list.Count) % list.Count;
        UpdateCentralDisplay();
    }
}