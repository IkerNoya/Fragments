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

    public void ChangeAmmoText(int actualAmmo, int maxAmmo) {
        ammoText.text = actualAmmo + " / " + maxAmmo;
        if( ((float)actualAmmo / (float)maxAmmo ) <= 0.25f)
            lowAmmoText.SetActive(true);
        else
            lowAmmoText.SetActive(false);
    }

}
