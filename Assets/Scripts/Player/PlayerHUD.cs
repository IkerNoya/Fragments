using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHUD : MonoBehaviour
{
    [SerializeField] GameObject openDoorText;
    [SerializeField] GameObject pickUpText;
    [SerializeField] GameObject PauseMenu;
 
    bool pickUpEvent;
    bool doorEvent;
    bool isPaused = false;

    void Start()
    {
        PlayerPickUp.ActivateText += GetPickupEvent;
        PlayerController.ActivateText += GetDoorEvent;
        PlayerController.ActivatePause += GetPausedEvent;
    }

    void Update()
    {
        if (pickUpEvent) pickUpText.SetActive(true);
        else pickUpText.SetActive(false);

        if (doorEvent) openDoorText.SetActive(true);
        else openDoorText.SetActive(false);

        if (isPaused)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            PauseMenu.SetActive(true);
        }
        else
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            PauseMenu.SetActive(false);
        }
    }

    void GetPausedEvent()
    {
        isPaused = !isPaused;
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
        PlayerController.ActivatePause -= GetPausedEvent;
    }
}
