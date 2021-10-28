using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzleDetector : MonoBehaviour
{
    [SerializeField] List<GameObject> eyes;
    [SerializeField] LayerMask puzzleLayer = 0;
    [SerializeField] float emissionIntensity = 0;

    List<Renderer> objectRenderers;

    float minimumEmissionIntensity = 0;

    RaycastHit hit;

    bool foundPuzzle = false;

    void Start()
    {
        objectRenderers = new List<Renderer>();
        for (int i = 0; i < eyes.Count; i++)
        {
            if (eyes[i] != null)
            {
                objectRenderers.Add(eyes[i].GetComponent<Renderer>());
            }
        }
    }

    void Update()
    {
        foundPuzzle = FindPuzzle();

        foreach (Renderer renderer in objectRenderers)
        {
            if (renderer != null)
            {
                if (foundPuzzle)
                {
                      renderer.material.SetColor("_EmissionColor", new Vector4(Color.red.r, Color.red.g, Color.red.b, 1) * emissionIntensity);
                }
                else
                {
                      renderer.material.SetColor("_EmissionColor", new Vector4(Color.white.r, Color.white.g, Color.white.b, 1));
                }
            }
        }
    }

    bool FindPuzzle()
    {
        return Physics.Raycast(transform.position, transform.forward, 10, puzzleLayer);
    }
}
