using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
public class HordeManager : MonoBehaviour {

    public UnityEvent SpawnKeyAfterRound;
    [SerializeField] List<Enemy> enemiesCreated;
    [SerializeField] List<Transform> spawners;
    [SerializeField] GameObject enemy;
    [Header("Horde")]
    [SerializeField] int initialEnemyCount = 0;
    [SerializeField] int maxEnemyCount = 0;
    [SerializeField] int enemyAdditionAmmount = 0;
    public UnityEvent PlayAudioQueue;

    [Header("Rounds")]
    [SerializeField] int currentRound = 0;
    [SerializeField] int roundToSpawnObjective = 0;
    [SerializeField] float timeBetweenRounds = 0;

    [Header("Mission Data")]
    public UnityEvent StartRound;
    public UnityEvent EndRoundBeforeKey;
    public UnityEvent EndRoundAfterKey;
    public UnityEvent EndRoundBeforeEscape; // Buscar mejor manera de usar estos eventos

    bool canSpawnEnemies = true;
    bool hasSpawnedKey = false;
    bool playerPickedUpKey = false;

    float spawnTimer = 0;

    int enemyCount = 0;

    FPSController player;

    void Awake() {
        Enemy.EnemyDead += EnemyDead;    
    }

    void Start() {
        player = FindObjectOfType<FPSController>();

        enemyCount = initialEnemyCount;

    }
    void OnDisable() {
        Enemy.EnemyDead -= EnemyDead;
    }
    void Update()
    {
        if (!canSpawnEnemies)
            return;

        if(spawnTimer >= timeBetweenRounds)
        {
            SpawnHorde();
            spawnTimer = 0;
            canSpawnEnemies = false;
        }
        spawnTimer += Time.deltaTime;    
    }

    void EnemyDead(Enemy enemy) {
        enemiesCreated.Remove(enemy);
        if (enemiesCreated.Count <= 0 && currentRound >= roundToSpawnObjective && !hasSpawnedKey)
        {
            if (FindObjectOfType<PlayerController>().GetAlive())
                SpawnKeyAfterRound?.Invoke();
            hasSpawnedKey = true;
            if (!playerPickedUpKey)
                EndRoundAfterKey?.Invoke();
            canSpawnEnemies = true;
        }
        else if(enemiesCreated.Count <= 0)
        {
            currentRound++;
            if (!hasSpawnedKey)
                EndRoundBeforeKey?.Invoke();
            else if (hasSpawnedKey && !playerPickedUpKey)
                EndRoundAfterKey?.Invoke();
            else if(hasSpawnedKey && playerPickedUpKey)
                EndRoundBeforeEscape?.Invoke();
            canSpawnEnemies = true;
        }

    }
    public void SpawnHorde()
    {
        for(int i = 0; i < enemyCount; i++)
        {
            if (enemiesCreated.Count < enemyCount)
            {
                int index = Random.Range(0, spawners.Count);
                GameObject en = Instantiate(enemy);
                en.transform.position = spawners[index].position;
                Enemy enemyScript = en.GetComponent<Enemy>();
                enemyScript.SetCanMove(true);   
                enemiesCreated.Add(enemyScript);
            }
        }


        PlayAudioQueue?.Invoke();
        StartRound?.Invoke();

        if (enemyCount < maxEnemyCount)
            enemyCount += enemyAdditionAmmount;
        else
            enemyCount = maxEnemyCount;
    }

    public void PlayerPickedUpKey()
    {
        playerPickedUpKey = true;
        if (spawnTimer > 1)
            EndRoundBeforeEscape?.Invoke();
    }
}
