using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public Transform SpawnPoint;
    public int SpawnAmount = 30;
    public int SpawnRows = 3;
    public ObjectPooler objectPooler;
    public int XDistance = 1;
    public int ZDistance = 1;
    public bool ReverseSpawnDirection = false;

    int _spawnCounter;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.S))
            SpawnEnemies();
    }

    public void SpawnEnemies()
    {
        float newX;
        float newZ;
        _spawnCounter = 0;

        for (int x = 0; x < SpawnRows; x++)
        {
            for (int z = 0; z < (double)SpawnAmount / SpawnRows; z++)
            {
                if (_spawnCounter == SpawnAmount)
                {
                    break;
                }

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

                GameObject enemy = objectPooler.GetPooledObject();                                                       //minus with 0.01 so the enemy hits the ground & navmeshagent will work
                enemy.transform.position = new Vector3(newX, SpawnPoint.transform.position.y + enemy.transform.localScale.y - 0.01f, newZ);
                enemy.SetActive(true);
                _spawnCounter++;
            }
        }
    }
}
