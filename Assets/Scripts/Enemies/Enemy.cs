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
    [SerializeField] Transform player;
    Rigidbody rb;

    Vector3 lastPlayerPosition;

    bool gamePaused = false;
    bool initialCutsceneEnded = false;
    NavMeshPath path;

    private void Awake() {
        PauseController.SetPause += SetGamePause;
        Console.ConsolePause += SetGamePause;
        InitialCutscene.endInitialCutscene += InitialCutsceneEnded;
    }

    void InitialCutsceneEnded(bool value)
    {
        initialCutsceneEnded = true;
        canMove = value;
    }

    void Start()
    {
        deathEffect = meshRenderer.material;
        rb = GetComponent<Rigidbody>();
        navMesh.destination = player.position;
        path = new NavMeshPath();
    }

    private void OnDestroy() {
        PauseController.SetPause -= SetGamePause;
        Console.ConsolePause -= SetGamePause;
        InitialCutscene.endInitialCutscene -= InitialCutsceneEnded;
    }
    private void OnDisable() {
        PauseController.SetPause -= SetGamePause;
        InitialCutscene.endInitialCutscene -= InitialCutsceneEnded;
    }

    private void Update() {

        if (Vector3.Distance(player.position, transform.position) < 2)
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

        if(!navMesh.enabled)
        navMesh.enabled = true;



        navMesh.SetDestination(player.position);
        Debug.Log(navMesh.destination + " Player " + player.position);

    }

    private void FixedUpdate() {
        if (gamePaused || !canMove || isDead)
            return;

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

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(navMesh.destination, 1);
        Gizmos.DrawSphere(player.position, .5f);
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
