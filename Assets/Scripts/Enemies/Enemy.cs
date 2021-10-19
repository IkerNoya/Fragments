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
    [SerializeField] ParticleSystem hitParticles;

    [SerializeField] float health;
    [SerializeField] float speed;
    [SerializeField] float damage;
    [SerializeField] NavMeshAgent navMesh;
    [SerializeField] bool canMove = true;
    PlayerController player;
    Rigidbody rb;

    bool gamePaused = false;
    bool initialCutsceneEnded = false;

    private void Awake() {
        PauseController.SetPause += SetGamePause;
        InitialCutscene.endInitialCutscene += InitialCutsceneEnded;
    }

    void InitialCutsceneEnded(bool value)
    {
        initialCutsceneEnded = true;
        canMove = value;
    }

    void Start()
    {
        player = FindObjectOfType<PlayerController>();
        deathEffect = meshRenderer.material;
        rb = GetComponent<Rigidbody>();
    }

    private void OnDestroy() {
        PauseController.SetPause -= SetGamePause;
        InitialCutscene.endInitialCutscene -= InitialCutsceneEnded;
    }
    private void OnDisable() {
        PauseController.SetPause -= SetGamePause;
        InitialCutscene.endInitialCutscene -= InitialCutsceneEnded;
    }

    private void Update() {

        if (Vector3.Distance(player.transform.position, transform.position) < 2)
            canMove = false;
        else
        {
            if(!gamePaused && !isDead && initialCutsceneEnded)
                canMove = true;
        }

        if (gamePaused || !canMove) {
            navMesh.enabled = false;
            return;
        }

        navMesh.enabled = true;

    }

    private void FixedUpdate() {
        if (gamePaused || !canMove || isDead)
            return;

        
        if(navMesh.path != null)
        {
            if(navMesh.isOnNavMesh)
                navMesh.SetDestination(player.transform.position);
        }
        else
        {
            navMesh.path = new NavMeshPath();
            if (navMesh.isOnNavMesh)
                navMesh.SetDestination(player.transform.position);
        }
    }

    public void Hit(float dmg, Vector3 hitPos, Vector3 attackerPos) {
        hitParticles.transform.position = hitPos;
        hitParticles.transform.LookAt(attackerPos);
        hitParticles.Play();

        if (isDead)
            return;
        health -= dmg;


        if(health <= 0) {
            navMesh.enabled = false;
            rb.isKinematic = false;
            rb.useGravity = true;
            source.PlayOneShot(deathSound);
            isDead = true;
            Destroy(this.gameObject, 6.73f);
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

    public bool GetIsDead()
    {
        return isDead;
    }

    public Rigidbody GetRigidBody() // cambiar nombre despues
    {
        return rb;
    }
}
