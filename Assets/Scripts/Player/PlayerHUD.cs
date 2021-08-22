using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHUD : MonoBehaviour
{
    [SerializeField] GameObject openDoorText;
    [SerializeField] GameObject pickUpText;

    bool pickUpEvent;
    bool doorEvent;

    void Start()
    {
        PlayerPickUp.ActivateText += GetPickupEvent;
        PlayerController.ActivateText += GetDoorEvent;
    }

    void Update()
    {
        if (pickUpEvent) pickUpText.SetActive(true);
        else pickUpText.SetActive(false);

        if (doorEvent) openDoorText.SetActive(true);
        else openDoorText.SetActive(false);
    }

    void GetPickupEvent(bool value)
    {
        pickUpEvent = value;
    }

    void GetDoorEvent(bool value)
    {
        doorEvent = value;
    }

    private void OnDisable()
    {
        PlayerPickUp.ActivateText -= GetPickupEvent;
        PlayerController.ActivateText -= GetDoorEvent;
    }
}
