using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class HordeManager : MonoBehaviour {

    public UnityEvent AllEnemiesDead;
    [SerializeField] List<Enemy> enemiesCreated;
    private void Awake() {
        Enemy.EnemyDead += EnemyDead;    
    }

    void Start() {
        Enemy[] ec = FindObjectsOfType<Enemy>();
        for (int i = 0; i < ec.Length; i++)
            enemiesCreated.Add(ec[i]);
    }
    private void OnDisable() {
        Enemy.EnemyDead -= EnemyDead;
    }

    void EnemyDead(Enemy enemy) {
        enemiesCreated.Remove(enemy);
        if (enemiesCreated.Count <= 0)
            if (FindObjectOfType<PlayerController>().GetAlive())
                AllEnemiesDead?.Invoke();
        
    }

}
