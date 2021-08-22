﻿using System;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

    
    [SerializeField] Inventory playerInventory;
    [SerializeField] PlayerPickUp playerPickUpController;
    [Space]
    [SerializeField] KeyCode keyToPickUpItem;
    [SerializeField] KeyCode keyToInteractWithEnviroment;
    [SerializeField] KeyCode KeyToEscape;
    [SerializeField] LayerMask layerDoors;

    public static event Action<bool> ActivateText;
    public static event Action ActivatePause;

    bool isPaused = false;


    void Update() {
        if (Input.GetKeyDown(KeyToEscape))
        {
            isPaused = !isPaused;
            ActivatePause.Invoke();
            if (!isPaused) Time.timeScale = 1;
        }

        if (isPaused)
        {
            Time.timeScale = 0;
            return;
        }

        playerPickUpController.TryPickUpObject(keyToPickUpItem);
        TryInteractWithDoor(keyToInteractWithEnviroment);


    }

    void TryInteractWithDoor(KeyCode key) {
        RaycastHit hit;
        Vector3 mousePos = Input.mousePosition;
        Ray ray = Camera.main.ScreenPointToRay(mousePos);

        if (Physics.Raycast(ray, out hit, 3))
        {
            if (layerDoors == (layerDoors | (1 << hit.transform.gameObject.layer)))
            {
                ActivateText.Invoke(true);
                Door_Base d = hit.transform.GetComponent<Door_Base>();
                if (d != null && Input.GetKeyDown(key))
                {
                    List<Door_Key> keyListAux = playerInventory.GetInventoryKeysList();
                    for (int i = 0; i < keyListAux.Count; i++)
                        if (d.TryOpenDoor(keyListAux[i]))
                        {
                            d.OpenDoor();
                            keyListAux[i].UseKey();
                            keyListAux.RemoveAt(i);
                            break;
                        }
                }
            }
        }
        else
            ActivateText.Invoke(false);
    }
}