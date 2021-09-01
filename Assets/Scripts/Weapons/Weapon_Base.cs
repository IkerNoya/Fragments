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
    bool reloading;
    int actualAmmo;

    [Header("Audio")]
    [SerializeField] AudioSource source;
    [SerializeField] List<AudioClip> shootSounds;
    [SerializeField] AudioClip noAmmoSound;
    [SerializeField] AudioClip reloadingSound;

    public static Action AmmoChanged;

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
            AmmoChanged?.Invoke();
        }

    }

    public virtual void Shoot() {
        if (reloading)
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
        if (reloading)  
            return;
        if (actualAmmo == totalAmmo) 
            return;
        
        source.PlayOneShot(reloadingSound);
        reloading = true;
    }

    public int GetActualAmmo() {
        return actualAmmo;
    }

    public int GetMaxAmmo() {
        return totalAmmo;
    }

}
