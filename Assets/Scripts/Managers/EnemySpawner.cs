using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemySpawner : MonoBehaviour
{
    public Transform SpawnPoint;
    public int SpawnAmount = 30;
    public int SpawnRows = 3;
    public ObjectPooler regularEnemy;
    public ObjectPooler regularBoss;
    public ObjectPooler flyingEnemy;
    public ObjectPooler flyingBoss;
    public int XDistance = 1;
    public int ZDistance = 1;
    public bool ReverseSpawnDirection = false;
    public RoundManager RoundManager;
    int _spawnCounter;
    int _regularSpawnAmount;
    NavMeshAgent agent;

    private void Awake()
    {
        _regularSpawnAmount = SpawnAmount;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.M) && Debug.isDebugBuild)
            SpawnEnemies();
    }

    public void SpawnEnemies()
    {
        ObjectPooler pooler;
        GameObject enemy;                                                       
        float newY = SpawnPoint.transform.position.y;
        var curRound = RoundManager._currentRound;

        float newX;
        float newZ;
        _spawnCounter = 0;

        if (RoundManager.isFlyingRound && RoundManager.isBossRound)
        {
            pooler = flyingBoss;
            SpawnAmount = 1;
            newY = transform.position.y + 5;
        }
        else if (RoundManager.isFlyingRound)
        {
            pooler = flyingEnemy;
            SpawnAmount = _regularSpawnAmount;
            newY = transform.position.y + 5;

        }
        else if (RoundManager.isBossRound)
        {
            pooler = regularBoss;
            SpawnAmount = 1;

        }
        else
        {
            pooler = regularEnemy;
            SpawnAmount = _regularSpawnAmount;
        }

        for (int x = 0; x < SpawnRows; x++)
        {
            for (int z = 0; z < (double)SpawnAmount / SpawnRows; z++)
            {
                if (_spawnCounter == SpawnAmount)
                {
                    break;
                }
                enemy = pooler.GetPooledObject();
                var zOffset = z;

                if (ReverseSpawnDirection)
                {
                    newX = SpawnPoint.transform.position.x - (x * XDistance);
                    newZ = SpawnPoint.transform.position.z + (z + zOffset);

                }
                else
                {
                    newX = SpawnPoint.transform.position.x + (x * XDistance);
                    newZ = SpawnPoint.transform.position.z - (z + zOffset);
                }
                //agent = enemy.GetComponent<NavMeshAgent>();
                                             //minus with 0.01 so the enemy hits the ground & navmeshagent will work
                enemy.transform.position = new Vector3(newX, newY + enemy.transform.localScale.y - 0.01f, newZ);
                enemy.SetActive(true);
                _spawnCounter++;
            }
        }
    }

    public void SpawnFlyingBoss()
    {
     
        StartCoroutine(WaitForX());
        

    }
    public void SpawnBoss()
    {
        StartCoroutine(WaitForX());


    }
    public IEnumerator WaitForX()
    {
        RoundManager.isBossRound = true;

        var enemy = flyingBoss.GetPooledObject();
        enemy.transform.position = new Vector3(SpawnPoint.transform.position.x, SpawnPoint.transform.position.y + 10 + enemy.transform.localScale.y - 0.01f, SpawnPoint.transform.position.z);
        enemy.SetActive(true);
        yield return new WaitForSeconds(2f);
        RoundManager.isBossRound = false;

    }

}
