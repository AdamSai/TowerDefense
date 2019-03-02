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
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.S))
            SpawnEnemies();

    }

    void SpawnEnemies()
    {
        float newX;
        float newZ;
        for (int x = 0; x < SpawnRows; x++)
        {
            for (int z = 0; z < SpawnAmount / SpawnRows; z++)
            {
                var zOffset = z;
                if (ReverseSpawnDirection)
                {
                    newX = SpawnPoint.transform.position.x - (x * XDistance);
                    newZ = SpawnPoint.transform.position.z + (z + zOffset);

                } else
                {
                    newX = SpawnPoint.transform.position.x + (x * XDistance);
                    newZ = SpawnPoint.transform.position.z - (z + zOffset);
                }

                GameObject enemy = objectPooler.GetPooledObject();
                
                enemy.transform.position = new Vector3(newX, SpawnPoint.transform.position.y + enemy.transform.localScale.y - 0.01f, newZ);
                zOffset += ZDistance;
                enemy.SetActive(true);
            }
        }
    }
}
