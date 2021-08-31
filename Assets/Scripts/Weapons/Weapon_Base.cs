using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon_Base : MonoBehaviour {

    [Header("Shoot")]
    [SerializeField] float range;
    [SerializeField] float damage;
    [SerializeField] LayerMask enemyLayer;

    [Header("Ammo")]
    [SerializeField] float timeToReload;
    float timerReloading;
    [SerializeField] int totalAmmo;
    bool reloading ;
    int actualAmmo;

    protected virtual void Start() {
        actualAmmo = totalAmmo;
        reloading = false;
        timerReloading = 0f;
    }

    protected virtual void Update() {
        if (!reloading)
            return;

        timerReloading += Time.deltaTime;
        if (timerReloading >= timeToReload) {
            timerReloading = 0;
            reloading = false;
            actualAmmo = totalAmmo;
            Debug.Log("Reloading completed");
        }

    }

    public virtual void Shoot() {
        if (actualAmmo <= 0) {
            Debug.Log("No Ammo");
            return;
        }

        Vector3 mousePos = Input.mousePosition;
        Ray ray = Camera.main.ScreenPointToRay(mousePos);
        RaycastHit hit;
            Debug.Log("Pum cachipum");
        if (Physics.Raycast(ray, out hit, range)) {
            if (enemyLayer == (enemyLayer | (1 << hit.transform.gameObject.layer))) {
                Debug.Log("Hitted enemy");
                Enemy e = hit.transform.GetComponent<Enemy>();
                if (e != null)
                    e.Hit(damage);
                else
                    Debug.LogError("Enemy hitted dont have component Enemy");
            }
        }

        actualAmmo--;
    }

    public virtual void StartReload() {
        if (reloading) {
            Debug.Log("Already reloading");
            return;
        }

        if (actualAmmo == totalAmmo) {
            Debug.Log("Max ammo, no reload");
            return;
        }

        reloading = true;
        Debug.Log("Starting reloading");
    }
}
