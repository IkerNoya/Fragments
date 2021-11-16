using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class HordeManager : MonoBehaviour {

    public UnityEvent AllEnemiesDead;
    [SerializeField] List<Enemy> enemiesCreated;
    [SerializeField] List<Transform> spawners;
    [SerializeField] GameObject enemy;
    bool hasSpawnedKey = false;

    FPSController player;
    private void Awake() {
        Enemy.EnemyDead += EnemyDead;    
    }

    void Start() {
        player = FindObjectOfType<FPSController>();

        Enemy[] ec = FindObjectsOfType<Enemy>();
        for (int i = 0; i < ec.Length; i++)
            enemiesCreated.Add(ec[i]);


    }
    private void OnDisable() {
        Enemy.EnemyDead -= EnemyDead;
    }

    void EnemyDead(Enemy enemy) {
        enemiesCreated.Remove(enemy);
        if (enemiesCreated.Count <= 0 && !hasSpawnedKey)
        {
            if (FindObjectOfType<PlayerController>().GetAlive())
                AllEnemiesDead?.Invoke();
            hasSpawnedKey = true;
        }
        
    }
    public void SpawnHorde()
    {
        for(int i = 0; i < 5; i++)
        {
            if (enemiesCreated.Count < 5)
            {
                int index = Random.Range(0, spawners.Count);
                GameObject en = Instantiate(enemy);
                en.transform.position = spawners[index].position;
                Enemy enemyScript = en.GetComponent<Enemy>();
                enemyScript.SetCanMove(true);   
                enemiesCreated.Add(enemyScript);
            }
        }
    }

}
