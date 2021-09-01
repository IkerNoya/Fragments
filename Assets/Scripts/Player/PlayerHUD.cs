using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHUD : MonoBehaviour
{
    [SerializeField] GameObject openDoorText;
    [SerializeField] GameObject pickUpText;
    [SerializeField] GameObject blackScreen;

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
}
