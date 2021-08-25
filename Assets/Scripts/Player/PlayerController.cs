﻿using System;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {


    [SerializeField] Inventory playerInventory;
    [Space]
    [SerializeField] KeyCode keyToPickUpItem;
    [SerializeField] KeyCode keyToInteractWithEnviroment;
    [SerializeField] LayerMask layerDoors;
    [SerializeField] LayerMask layerKeys;

    [Header("UI")]
    [SerializeField] UI_Inventory inventoryUI;
    [SerializeField] PlayerHUD hud;

    void Update() {
        if (Input.GetKeyDown(KeyCode.Tab))
            inventoryUI.gameObject.SetActive(!inventoryUI.gameObject.activeSelf);

        TryPickUpObject();
        TryInteractWithDoor();
    }

    void TryInteractWithDoor() {
        RaycastHit hit;
        Vector3 mousePos = Input.mousePosition;
        Ray ray = Camera.main.ScreenPointToRay(mousePos);

        if (Physics.Raycast(ray, out hit, 3)) {
            if (layerDoors == (layerDoors | (1 << hit.transform.gameObject.layer))) {
                hud.SetDoorTextActive(true);
                if (Input.GetKeyDown(keyToInteractWithEnviroment)) {
                    Door_Base d = hit.transform.GetComponent<Door_Base>();

                    if (d != null) {
                        List<Door_Key> keyListAux = playerInventory.GetInventoryKeysList();
                        for (int i = 0; i < keyListAux.Count; i++)
                            if (d.TryOpenDoor(keyListAux[i])) {
                                d.OpenDoor();
                                keyListAux[i].UseKey();
                                keyListAux.RemoveAt(i);
                                break;
                            }
                        hud.SetDoorTextActive(false);
                    }

                }
            }
        }
        else
            hud.SetDoorTextActive(false);
    }

    void TryPickUpObject() {
        RaycastHit hit;
        Vector3 mousePos = Input.mousePosition;
        Ray ray = Camera.main.ScreenPointToRay(mousePos);

        if (Physics.Raycast(ray, out hit, 3)) {
            if (layerKeys == (layerKeys | (1 << hit.transform.gameObject.layer))) {
                hud.SetPickupTextActive(true);

                if (Input.GetKeyDown(keyToPickUpItem)) {
                    Door_Key item = hit.collider.GetComponent<Door_Key>();
                    if (item != null && item.GetCanPickUp()) {
                        playerInventory.AddKeyToInventory(item.PickUpItem());
                        hud.SetPickupTextActive(false);
                    }
                }

            }
        }
        else
            hud.SetPickupTextActive(false);
    }

}
