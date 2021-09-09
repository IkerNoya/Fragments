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
    [SerializeField] float fireRate;
    [SerializeField] float horizontalRecoil;
    [SerializeField] float verticalRecoil;
    [SerializeField] bool isSemiAutomatic;

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
    MouseLook recoil;

    bool canShoot = true;
    float shootTimer = 0;

    protected virtual void Start() {
        actualAmmo = ammoPerMagazine;
        reloading = false;
        timerReloading = 0f;
        fpsController = GetComponentInParent<FPSController>();
        recoil = GetComponentInParent<MouseLook>();
        AmmoChanged?.Invoke();
    }

    protected virtual void Update() {

        if (shootTimer < fireRate)
        {
            canShoot = false;
            shootTimer += Time.deltaTime;
        }
        else
        {
            canShoot = true;
        }


        if (!reloading)
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

            shootTimer = fireRate;
            AmmoChanged?.Invoke();
        }

    }

    public virtual void Shoot() {
        if (reloading || fpsController.GetPauseState() || !canShoot)
            return;

        shootTimer = 0;

        if (actualAmmo > 0)
        {
            animator.SetTrigger("Shoot");
            if (recoil)
                recoil.AddRecoil(verticalRecoil, UnityEngine.Random.Range(-horizontalRecoil, horizontalRecoil));
        }

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

        animator.SetTrigger("Reload");
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
    public bool GetIsSemiAutomatic()
    {
        return isSemiAutomatic;
    }

}
