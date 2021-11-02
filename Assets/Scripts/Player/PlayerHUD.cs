using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class PlayerHUD : MonoBehaviour
{
    [Header("Interactions")]
    [SerializeField] GameObject openDoorText;
    [SerializeField] GameObject pickUpText;
    [SerializeField] GameObject blackScreen;
    [Header("Ammo")]
    [SerializeField] TextMeshProUGUI ammoText;
    [SerializeField] GameObject lowAmmoTextObj;
    [SerializeField] TextMeshProUGUI lowAmmoText;
    [SerializeField] GameObject ReloadText;
    [SerializeField] GameObject crosshair;

    [Header("")]
    [SerializeField] Image healthPanel;
    [SerializeField] Color healthPanelColor;
    [SerializeField] float maxAlpha;


    PlayerController player; // TEMPORAL; QUITAR DESPUES PORFAVOR ES URGENTE EL QUITAR ESTO; NO DEJAR ACA

    bool isAboutToInteract = false;
    private void Start() {
        SetDoorTextActive(false);
        SetPickupTextActive(false);
    }

    public void SetPickupTextActive(bool value) {
        pickUpText.SetActive(value);
    }

    public void SetDoorTextActive(bool value) {
        openDoorText.SetActive(value);
    }

    public void ChangeAmmoText(int actualAmmo, int ammoPerMagazine, int maxAmmo) {
        ammoText.text = actualAmmo + " / " + maxAmmo;
        if (((float)actualAmmo / (float)ammoPerMagazine) <= 0.25f) {
            lowAmmoTextObj.SetActive(true);
            lowAmmoText.text = "LOW AMMO";
            if (actualAmmo <= 0) {
                crosshair.SetActive(false);
                if (!isAboutToInteract)
                {
                    ReloadText.SetActive(true);
                }
                lowAmmoText.text = "NO AMMO";
            }
        }
        else {
            lowAmmoTextObj.SetActive(false);
            crosshair.SetActive(true);
            ReloadText.SetActive(false);
        }
    }
    public bool GetInteractBool()
    {
        return isAboutToInteract;
    }
    public void SetInteractBool(bool value)
    {
        isAboutToInteract = value;
    }

    public void UpdateHealthRedScreen(float actualHP, float maxHP) {
        float alpha = (maxHP - actualHP) / maxHP;
        if (alpha >= maxAlpha)
            alpha = maxAlpha;

        healthPanel.color  = new Color(healthPanelColor.r, healthPanelColor.g, healthPanelColor.b, alpha);
    }

}
