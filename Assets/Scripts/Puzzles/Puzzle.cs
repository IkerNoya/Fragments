using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Puzzle : MonoBehaviour
{
    [SerializeField] List<Transform> keySpawnPositions;
    [SerializeField] GameObject Key;
    bool puzzleActivated = false;

    public UnityEvent SpawnedKey;

    public void SpawnKey()
    {
        if (!puzzleActivated)
        {
            int index = Random.Range(0, keySpawnPositions.Count);
            Key.transform.position = keySpawnPositions[index].position;
            SpawnedKey?.Invoke();
            puzzleActivated = true;
        }
    }
}
