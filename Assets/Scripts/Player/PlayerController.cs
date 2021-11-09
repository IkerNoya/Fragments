using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

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

    [Header("Missions")]
    [SerializeField] List<Mission> missions;

    [Header("Health")]
    [SerializeField] float actualHealth;
    [SerializeField] float maxHealth;
    [SerializeField] float healingPerSecond;
    [SerializeField] float timeToStartHealing;
    bool alive = true;
    bool healing = false;
    float timerHealing = 0;

    FPSController fPSController;
    bool gamePaused = false;

    public static event Action ShowObjective;
    public static event Action PlayerDead;
    public UnityEvent EndGame;

    private void Awake() {
        Weapon_Base.AmmoChanged += WeaponAmmoChanged;
        PauseController.SetPause += SetGamePause;
        Console.ConsolePause += SetGamePause;
    }

    private void Start() {
        fPSController = GetComponent<FPSController>();
        hud.ChangeAmmoText(weapon.GetActualAmmo(), weapon.GetAmmoPerMagazine(), weapon.GetMaxAmmo());

        actualHealth = maxHealth;
        alive = true;
        healing = false;
        timerHealing = 0;
    }

    private void OnDisable() {
        Weapon_Base.AmmoChanged -= WeaponAmmoChanged;
        PauseController.SetPause -= SetGamePause;
        Console.ConsolePause -= SetGamePause;
    }

    private void OnDestroy() {
        Weapon_Base.AmmoChanged -= WeaponAmmoChanged;
        PauseController.SetPause -= SetGamePause;
    }

    void Update() {
        if (!alive)
            return;

        if (!fPSController.GetCanMove())
            return;

        if (actualHealth != maxHealth) {
            if (healing) {
                actualHealth += healingPerSecond * Time.deltaTime;
                if (actualHealth >= maxHealth)
                    actualHealth = maxHealth;
                hud.UpdateHealthRedScreen(actualHealth, maxHealth);
            }
            else {
                timerHealing += Time.deltaTime;
                if (timerHealing >= timeToStartHealing)
                    healing = true;
            }
        }

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
        if (Input.GetKeyDown(KeyCode.J))
        {
            ShowObjective?.Invoke();
        }

        if (Input.GetKeyDown(KeyCode.Return))
            Hit(10);

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
                            d.SetLockedSound();
                            for (int i = 0; i < keyListAux.Count; i++)
                                if (d.TryOpenDoor(keyListAux[i])) {
                                    d.SetUnlockedDoorSound();
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

        if (Physics.Raycast(ray, out hit, 5)) {
            if (layerKeys == (layerKeys | (1 << hit.transform.gameObject.layer))) {
                hud.SetInteractBool(true);
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
        {
            hud.SetPickupTextActive(false);
            hud.SetInteractBool(false);
        }
    }

    void WeaponAmmoChanged() {
        hud.ChangeAmmoText(weapon.GetActualAmmo(), weapon.GetAmmoPerMagazine(), weapon.GetTotalAmmo());
    }
    public void AddAmmo(int value) {
        weapon.AddAmmo(value);
        WeaponAmmoChanged();
    }
    public Weapon_Base GetActualWeapon() {
        return weapon;
    }
    void SetGamePause(bool value) {
        gamePaused = value;
    }

    public void AddMission(Mission mission)
    {
        missions.Add(mission);
    }
    public bool GetAlive() {
        return alive;
    }
    public void Hit(float damage) {
        healing = false;
        timerHealing = 0;

        actualHealth -= damage;
        if (actualHealth <= 0) {
            Debug.Log("Mas muerto que muertin");
            actualHealth = 0;
            alive = false;
            PlayerDead?.Invoke();
            weapon.gameObject.SetActive(false);
            hud.SetUIActive(false);
            StartCoroutine(WaitToLoose(1.5f));
        }
        hud.UpdateHealthRedScreen(actualHealth, maxHealth);
    }


    IEnumerator WaitToLoose(float time)
    {
        yield return new WaitForSeconds(time);
        EndGame?.Invoke();
        yield return null;
    }
}
