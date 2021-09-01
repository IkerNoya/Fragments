using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
            if (!source.isPlaying)
                source.PlayOneShot(noAmmoSound);
            Debug.Log("No Ammo");
            return;
        }

        shootParticles.Play();

        source.PlayOneShot(shootSounds[Random.Range(0, shootSounds.Count)]);

        Vector3 mousePos = Input.mousePosition;
        Ray ray = Camera.main.ScreenPointToRay(mousePos);
        RaycastHit hit;
        Debug.Log("Pum cachipum");
        if (Physics.Raycast(ray, out hit, range)) {
            if (hit.collider.CompareTag("Enemy")) {
                Debug.Log("Hitted enemy");
                Enemy e = hit.transform.GetComponent<Enemy>();
                if (e != null)
                    e.Hit(damage);
                else
                    Debug.LogError("Enemy hitted dont have component Enemy");
            }
            else if (hit.collider.CompareTag("Map")) {
                GameObject hole = Instantiate(shootImpactHole, hit.point + hit.normal * 0.001f, Quaternion.LookRotation(hit.normal));
                Destroy(hole, 5f);
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
        source.PlayOneShot(reloadingSound);
        reloading = true;
        Debug.Log("Starting reloading");
    }

}
