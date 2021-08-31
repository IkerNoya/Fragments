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

    [SerializeField] float health;
    [SerializeField] float speed;
    [SerializeField] float damage;
    [SerializeField] NavMeshAgent navMesh;
    PlayerController player;

    void Start()
    {
        player = FindObjectOfType<PlayerController>();
        navMesh.SetDestination(player.transform.position);
        deathEffect = meshRenderer.material;
        deathValue = deathEffect.GetFloat("_DissolveY");
    }

    private void FixedUpdate() {
        navMesh.SetDestination(player.transform.position);
    }

    public void Hit(float dmg) {
        if (isDead)
            return;

        health -= dmg;
        if(health <= 0) {
            isDead = true;
            Destroy(this.gameObject, 10f);
            meshRenderer.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off; // solucion temporar hasta lograr que se casteen sombras del shader
            StartCoroutine(DissolveEffect());
        }
    }

    IEnumerator DissolveEffect() {
        while (deathValue >= 0) {
            deathValue -= Time.deltaTime * dissolveSpeed;
            deathEffect.SetFloat("_DissolveY", deathValue);
            yield return null;
        }

        yield return null;
    }
}
