using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
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
    [SerializeField] GameObject ReloadButtonText;
    [SerializeField] GameObject crosshair;

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
                ReloadText.SetActive(true);
                ReloadButtonText.SetActive(true);
                lowAmmoText.text = "NO AMMO";
            }
        }
        else {
            lowAmmoTextObj.SetActive(false);
            crosshair.SetActive(true);
            ReloadText.SetActive(false);
            ReloadButtonText.SetActive(false);
        }
    }

}
