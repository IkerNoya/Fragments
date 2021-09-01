using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class PlayerHUD : MonoBehaviour
{
    [SerializeField] GameObject openDoorText;
    [SerializeField] GameObject pickUpText;

    [SerializeField] TextMeshProUGUI ammoText;
    [SerializeField] GameObject lowAmmoText;
    [SerializeField] GameObject noAmmoText;
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
            lowAmmoText.SetActive(true);
            if (actualAmmo <= 0) {
                crosshair.SetActive(false);
                noAmmoText.SetActive(true);
            }
        }
        else {
            lowAmmoText.SetActive(false);
            crosshair.SetActive(true);
            noAmmoText.SetActive(false);
        }
    }

}
