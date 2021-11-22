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
    [SerializeField] AudioClip attackSound;
    [SerializeField] ParticleSystem hitParticles;

    [SerializeField] float health;
    [SerializeField] float speed;
    [SerializeField] float damage;
    [SerializeField] NavMeshAgent navMesh;
    [SerializeField] bool canMove = false;
    [SerializeField] PlayerController player;
    [SerializeField] float distanceToAttack;
    Rigidbody rb;

    Vector3 lastPlayerPosition;

    bool gamePaused = false;
    bool initialCutsceneEnded = false;
    //NavMeshPath path;

    public static Action<Enemy> EnemyDead;

    SpriteRenderer sprite;


    [SerializeField] AmmoBox ammoBox;

    private void Awake() {
        PauseController.SetPause += SetGamePause;
        Console.ConsolePause += SetGamePause;
        InitialCutscene.endInitialCutscene += InitialCutsceneEnded;
        PlayerController.PlayerDead += StopMovement;
    }

    void InitialCutsceneEnded(bool value)
    {
        initialCutsceneEnded = true;
        canMove = value;
    }

    void Start()
    {
        player = FindObjectOfType<PlayerController>();

        if(meshRenderer) deathEffect = meshRenderer.material;
        rb = GetComponent<Rigidbody>();
        if(navMesh && navMesh.enabled) navMesh.destination = player.transform.position;
       // path = new NavMeshPath();
        anim = GetComponent<Animator>();
        sprite = GetComponentInChildren<SpriteRenderer>();


    }

    private void OnDestroy() {
        PauseController.SetPause -= SetGamePause;
        Console.ConsolePause -= SetGamePause;
        InitialCutscene.endInitialCutscene -= InitialCutsceneEnded;
        PlayerController.PlayerDead -= StopMovement;
    }

    private void Update() {
        if (isDead)
            return;

        if (Vector3.Distance(player.transform.position, transform.position) < distanceToAttack) {
            Attack();
        }
        else {
            if (!gamePaused && !isDead && initialCutsceneEnded)
                canMove = true;
        }

        if (gamePaused || !canMove) {
            if(navMesh) navMesh.enabled = false;
            return;
        }

        if (!isDead) {
            if (navMesh && !navMesh.enabled)
                navMesh.enabled = true;

            if (navMesh && navMesh.enabled)
                navMesh.SetDestination(player.transform.position);
        }
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
            Die(1);
            //meshRenderer.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off; // solucion temporal hasta lograr que se casteen sombras del shader
            //StartCoroutine(DissolveEffect());
        }
    }

    void Attack() {
        source.PlayOneShot(attackSound);
        player.Hit(damage);
        if (navMesh)
            navMesh.enabled = false;
        isDead = true;
        transform.localScale = Vector3.zero;
        GetComponent<BoxCollider>().enabled = false;

        EnemyDead?.Invoke(this);
        Destroy(this.gameObject, attackSound.length);
    }

    void Die(float timeToDissapear) {
        if (navMesh)
            navMesh.enabled = false;
        source.PlayOneShot(deathSound);
        isDead = true;
        EnemyDead?.Invoke(this);
        Destroy(this.gameObject, timeToDissapear);
        anim.SetTrigger("Die");

        if (UnityEngine.Random.Range(0, 2) != 0)
            Instantiate(ammoBox, transform.position + Vector3.down, Quaternion.identity);
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

    void StopMovement() {
        canMove = false;
        navMesh.enabled = false;
    }

    public void SetCanMove(bool value)
    {
        StartCoroutine(ActivateEnemy());
    }
    IEnumerator ActivateEnemy()
    {
        navMesh.enabled = false;
        yield return new WaitForSeconds(2f);
        navMesh.enabled = true;
        canMove = true;
        yield return null;
    }

    public void SetSpeed(float speed)
    {
        this.speed = speed;
    }
}
