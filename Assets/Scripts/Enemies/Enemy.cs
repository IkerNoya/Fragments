using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    [SerializeField] float dissolveSpeed = 0;

    [SerializeField] MeshRenderer meshRenderer;
    Material deathEffect;
    float deathValue = 0;
    bool isDead = false;

    [SerializeField] AudioSource source;
    [SerializeField] AudioClip deathSound;

    [SerializeField] float health;
    [SerializeField] float speed;
    [SerializeField] float damage;
    [SerializeField] NavMeshAgent navMesh;
    PlayerController player;
    Rigidbody rb;

    bool gamePaused = false;

    private void Awake() {
        PauseController.SetPause += SetGamePause;
    }

    void Start()
    {
        player = FindObjectOfType<PlayerController>();
        navMesh.SetDestination(player.transform.position);
        deathEffect = meshRenderer.material;
        rb = GetComponent<Rigidbody>();
    }

    private void OnDestroy() {
        PauseController.SetPause -= SetGamePause;
    }
    private void OnDisable() {
        PauseController.SetPause -= SetGamePause;
    }

    private void Update() {
        if (gamePaused) {
            navMesh.enabled = false;
            return;
        }

        navMesh.enabled = true;

    }

    private void FixedUpdate() {
        if (gamePaused)
            return;

        if (isDead)
            return;
        if(navMesh.CalculatePath(player.transform.position, navMesh.path))
            navMesh.SetDestination(player.transform.position);
    }

    public void Hit(float dmg) {
        if (isDead)
            return;
        health -= dmg;
        if(health <= 0) {
            navMesh.enabled = false;
            rb.isKinematic = false;
            rb.useGravity = true;
            source.PlayOneShot(deathSound);
            isDead = true;
            Destroy(this.gameObject, 10f);
            meshRenderer.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off; // solucion temporal hasta lograr que se casteen sombras del shader
            StartCoroutine(DissolveEffect());
        }
    }

    IEnumerator DissolveEffect() {
        deathEffect.SetFloat("_DissolveY", transform.position.y + 10);
        deathValue = deathEffect.GetFloat("_DissolveY");
        while (deathValue > -1) {
            deathValue -= Time.deltaTime * dissolveSpeed;
            deathEffect.SetFloat("_DissolveY", deathValue);
            yield return null;
        }

        yield return null;
    }

    void SetGamePause(bool value) {
        gamePaused = value;
    }

    public float GetHealth()
    {
        return health;
    }

    public Rigidbody GetRigidBody() // cambiar nombre despues
    {
        return rb;
    }
}
