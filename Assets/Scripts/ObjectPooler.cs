using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPooler : MonoBehaviour
{
    // public static ObjectPooler current;
    public GameObject pooledObject;
    public Transform projectileContainer;
    public int pooledAmount = 20;
    public bool willGrow = true;

    List<GameObject> pooledObjects;

    private void Awake()
    {
        //    current = this;
    }
    // Start is called before the first frame update
    void Start()
    {
        pooledObjects = new List<GameObject>();
        for (int i = 0; i < pooledAmount; i++)
        {
            pooledObjects.Add(Instantiate(pooledObject));
            pooledObjects[i].SetActive(false);
            pooledObjects[i].transform.SetParent(projectileContainer);
        }
    }

    public GameObject GetPooledObject()
    {
        for (int i = 0; i < pooledObjects.Count; i++)
        {
            if (!pooledObjects[i].activeInHierarchy)
            {
                return pooledObjects[i];
            }
        }

        if (willGrow)
        {
            GameObject obj = Instantiate(pooledObject);
            obj.transform.SetParent(projectileContainer);
            pooledObjects.Add(obj);
            return obj;
        }

        return null;
    }
}
