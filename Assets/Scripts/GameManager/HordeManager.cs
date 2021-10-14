using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class HordeManager : MonoBehaviour
{

    [SerializeField] List<GameObject> enemies;
    [SerializeField] int enemyCount = 1;
    bool hordeKilled = false;
    public UnityEvent Survived;
    void Start()
    {
        enemyCount = enemies.Count;
    }

    void Update()
    {

        if (enemyCount <= 0 && !hordeKilled)
        {
            hordeKilled = true;
            Survived?.Invoke();
        }
        foreach(GameObject enemy in enemies)
        {
            if (!enemy) enemies.Remove(enemy);
        }
        enemyCount = enemies.Count;
    }
}
