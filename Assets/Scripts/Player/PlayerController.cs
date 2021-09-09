using System;
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


    [Header("Weapons")]
    [SerializeField] Weapon_Base weapon;
    [SerializeField] KeyCode keyToShoot;
    [SerializeField] KeyCode keyToReload;

    FPSController fPSController;
    bool gamePaused = false;

    private void Awake() {
        Weapon_Base.AmmoChanged += WeaponAmmoChanged;
        PauseController.SetPause += SetGamePause;
    }

    private void Start() {
        fPSController = GetComponent<FPSController>();
        hud.ChangeAmmoText(weapon.GetActualAmmo(),weapon.GetAmmoPerMagazine(), weapon.GetMaxAmmo());
    }

    private void OnDisable() {
        Weapon_Base.AmmoChanged -= WeaponAmmoChanged;
        PauseController.SetPause -= SetGamePause;
    }

    private void OnDestroy() {
        Weapon_Base.AmmoChanged -= WeaponAmmoChanged;
        PauseController.SetPause -= SetGamePause;
    }

    void Update() {
        if (!fPSController.GetCanMove())
            return;

        if (Input.GetKeyDown(KeyCode.Tab))
            inventoryUI.gameObject.SetActive(!inventoryUI.gameObject.activeSelf);
        if (weapon)
        {
            if (weapon.GetIsSemiAutomatic())
            {
                if (Input.GetKeyDown(keyToShoot))
                    weapon.Shoot();
            }
            else
            {
                if (Input.GetKey(keyToShoot))
                    weapon.Shoot();
            }

            if (Input.GetKeyDown(keyToReload))
                weapon.StartReload();
        }

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
                        if (d.GetClosedDoor()) {
                            for (int i = 0; i < keyListAux.Count; i++)
                                if (d.TryOpenDoor(keyListAux[i])) {
                                    d.OpenDoor();
                                    keyListAux[i].UseKey();
                                    keyListAux.RemoveAt(i);
                                    break;
                                }
                            if (d.GetClosedDoor()) d.PlayLockedDoorSound();
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

    void WeaponAmmoChanged() {
        hud.ChangeAmmoText(weapon.GetActualAmmo(), weapon.GetAmmoPerMagazine(),weapon.GetMaxAmmo());
    }

    void SetGamePause(bool value) {
        gamePaused = value;
    }

}
