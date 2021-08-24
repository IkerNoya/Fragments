using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] float dissolveSpeed = 0;

    MeshRenderer meshRenderer;
    Material deathEffect;
    float deathValue = 0;
    bool isDead = false;
    void Start()
    {
        meshRenderer = GetComponent<MeshRenderer>();
        deathEffect = GetComponent<MeshRenderer>().material;
        deathValue = deathEffect.GetFloat("_DissolveY");
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.KeypadEnter))
        {
            isDead = true;
        }
        if (isDead)
        {
            meshRenderer.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off; // solucion temporar hasta lograr que se casteen sombras del shader
            Destroy(gameObject, 5f);
            deathValue -= Time.deltaTime * dissolveSpeed;
            deathEffect.SetFloat("_DissolveY", deathValue);
        }
    }
}
