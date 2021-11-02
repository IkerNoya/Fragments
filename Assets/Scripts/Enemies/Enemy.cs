using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using System;

public class Enemy : MonoBehaviour
{
    [SerializeField] float dissolveSpeed = 0;

    [SerializeField] MeshRenderer meshRenderer;
    Material deathEffect;
    float deathValue = 0;
    bool isDead = false;

    Animator anim;

    [SerializeField] AudioSource source;
    [SerializeField] AudioClip deathSound;
    [SerializeField] ParticleSystem hitParticles;

    [SerializeField] float health;
    [SerializeField] float speed;
    [SerializeField] float damage;
    [SerializeField] NavMeshAgent navMesh;
    [SerializeField] bool canMove = false;
    [SerializeField] Transform player;
    Rigidbody rb;

    Vector3 lastPlayerPosition;

    bool gamePaused = false;
    bool initialCutsceneEnded = false;
    //NavMeshPath path;

    public static Action<Enemy> EnemyDead;

    SpriteRenderer sprite;

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
        player = FindObjectOfType<PlayerController>().transform;

        if(meshRenderer) deathEffect = meshRenderer.material;
        rb = GetComponent<Rigidbody>();
        if(navMesh) navMesh.destination = player.position;
       // path = new NavMeshPath();
        anim = GetComponent<Animator>();
        sprite = GetComponentInChildren<SpriteRenderer>();

    }

    private void OnDestroy() {
        PauseController.SetPause -= SetGamePause;
        Console.ConsolePause -= SetGamePause;
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
            if(navMesh) navMesh.enabled = false;
            return;
        }

        if(navMesh && !navMesh.enabled)
            navMesh.enabled = true;

        if(navMesh) 
            navMesh.SetDestination(player.position);

    }

    public void Hit(float dmg, Vector3 hitPos, Vector3 attackerPos) {

        if(sprite.color.a > 0.5f) // checks if an enemy is visible
        {
            hitParticles.transform.position = hitPos;
            hitParticles.transform.LookAt(attackerPos);
            hitParticles.Play();
        }

        if (isDead)
            return;
        health -= dmg;


        if(health <= 0) {
            if (navMesh)
                navMesh.enabled = false;
            source.PlayOneShot(deathSound);
            isDead = true;
            EnemyDead?.Invoke(this);
            Destroy(this.gameObject, 1);
            anim.SetTrigger("Die");
            //meshRenderer.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off; // solucion temporal hasta lograr que se casteen sombras del shader
            //StartCoroutine(DissolveEffect());
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
