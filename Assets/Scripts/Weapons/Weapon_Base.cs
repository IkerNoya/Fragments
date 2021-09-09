using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class Weapon_Base : MonoBehaviour {

    [Header("Shoot")]
    [SerializeField] float range;
    [SerializeField] float damage;
    [SerializeField] ParticleSystem shootParticles;
    [SerializeField] GameObject shootImpactHole;

    [Header("Ammo")]
    [SerializeField] float timeToReload;
    float timerReloading;
    [SerializeField] int totalAmmo;
    [SerializeField] int ammoPerMagazine;
    bool reloading;
    int actualAmmo;

    [Header("Audio")]
    [SerializeField] AudioSource source;
    [SerializeField] List<AudioClip> shootSounds;
    [SerializeField] AudioClip noAmmoSound;
    [SerializeField] AudioClip reloadingSound;

    [Header("Animations")]
    [SerializeField] Animator animator;

    public static Action AmmoChanged;

    FPSController fpsController;

    protected virtual void Start() {
        actualAmmo = ammoPerMagazine;
        reloading = false;
        timerReloading = 0f;
        fpsController = GetComponentInParent<FPSController>();
    }

    protected virtual void Update() {
        if (!reloading || !fpsController.GetPauseState())
            return;

        timerReloading += Time.deltaTime;
        if (timerReloading >= timeToReload) {
            timerReloading = 0;
            reloading = false;

            int diff = ammoPerMagazine - actualAmmo;
            totalAmmo -= diff;

            if (totalAmmo <= 0) {
                totalAmmo += ammoPerMagazine;
                actualAmmo = totalAmmo;
                totalAmmo = 0;
            }
            else 
                actualAmmo = ammoPerMagazine;

            AmmoChanged?.Invoke();
        }

    }

    public virtual void Shoot() {
        if (reloading || fpsController.GetPauseState())
            return;

        if (actualAmmo <= 0) {
            source.PlayOneShot(noAmmoSound);
            return;
        }

        shootParticles.Play();

        source.PlayOneShot(shootSounds[UnityEngine.Random.Range(0, shootSounds.Count)]);

        Vector3 mousePos = Input.mousePosition;
        Ray ray = Camera.main.ScreenPointToRay(mousePos);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, range)) {
            if (hit.collider.CompareTag("Enemy")) {
                Enemy e = hit.transform.GetComponent<Enemy>();
                if (e != null)
                    e.Hit(damage);
            }
            else if (hit.collider.CompareTag("Map")) {
                GameObject hole = Instantiate(shootImpactHole, hit.point + hit.normal * 0.001f, Quaternion.LookRotation(hit.normal));
                Destroy(hole, 5f);
            }
        }

        actualAmmo--;
        AmmoChanged?.Invoke();
    }

    public virtual void StartReload() {
        if (reloading || actualAmmo == ammoPerMagazine || totalAmmo <= 0)
            return;

        animator.Play("Reload");
        source.PlayOneShot(reloadingSound);
        reloading = true;
    }

    public int GetActualAmmo() {
        return actualAmmo;
    }
    public int GetMaxAmmo() {
        return totalAmmo;
    }
    public int GetAmmoPerMagazine() {
        return ammoPerMagazine;
    }

}
