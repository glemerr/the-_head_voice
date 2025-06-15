using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;
public class GunUIManager : MonoBehaviour
{
    [Header("Gun Display")]
    public Image weaponIcon;
    public TextMeshProUGUI weaponName;
    public TextMeshProUGUI fireRateText;
    public TextMeshProUGUI reloadTimeText;
    public TextMeshProUGUI cooldownText;
    public Image cooldownRadial;

    [Header("Ammo")]
    public TextMeshProUGUI ammoText;
    public Slider reloadSlider;

    [Header("Weapon Selection")]
    public List<Button> weaponButtons;
    private GunManager gunManager;
    void Start()
    {
        gunManager = FindFirstObjectByType<GunManager>();
        SetupWeaponButtons();
    }

    void Update()
    {
        UpdateGunUI();
    }
    void SetupWeaponButtons()
    {
        for (int i = 0; i < weaponButtons.Count; i++)
        {
            int index = i;
            weaponButtons[i].onClick.AddListener(() => gunManager.SendMessage("EquipGun", index));
            weaponButtons[i].GetComponentInChildren<Image>().color = new Color32(85, 85, 85, 255); // Gray background
            weaponButtons[i].GetComponentInChildren<Image>().sprite = gunManager.guns[i].weaponIcon;
            weaponButtons[i].GetComponentInChildren<TextMeshProUGUI>().text = gunManager.guns[i].weaponName;
            weaponButtons[i].GetComponentInChildren<TextMeshProUGUI>().fontSize = 30;
            weaponButtons[i].GetComponentInChildren<TextMeshProUGUI>().alignment = TextAlignmentOptions.Center;
            
        }
    }
    void UpdateGunUI()
    {
        var gun = gunManager.GetActiveGun();
        if (gun == null) return;

        weaponName.text = gun.weaponName;
        fireRateText.text = $"Fire Rate: {gun.fireRate:F2}s";
        reloadTimeText.text = $"Reload Time: {gunManager.GetReloadTime():F1}s";
        cooldownText.text = $"Cooldown: {gunManager.GetCooldownTime():F2}s";
        cooldownRadial.fillAmount = 1 - (gunManager.GetCooldownTime() / gun.fireRate);
        weaponIcon.sprite = gun.weaponIcon;
        weaponIcon.color = gunManager.GetCooldownTime() > 0 ? Color.red : Color.white;

        ammoText.text = $"{gunManager.GetAmmoInClip()} / {gun.clipSize}";
        reloadSlider.value = 1 - gunManager.GetReloadProgress();
    }
}
