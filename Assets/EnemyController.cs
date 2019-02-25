using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour
{

    public List<Node> FinalPath;
    public Transform destination;
    public float movementSpeed;
    public int i;
    private NavMeshAgent agent;
    bool shouldMove = false;
    float Counter;
    NavMeshPath moveToPath;

    // Start is called before the first frame update
    void Start()
    {
        FinalPath = new List<Node>();
        agent = GetComponent<NavMeshAgent>();
        shouldMove = true;
        moveToPath = new NavMeshPath();


    }

    // Update is called once per frame
    void Update()
    {
        Counter += Time.deltaTime;
        NavMesh.CalculatePath(transform.position, destination.position, agent.areaMask, moveToPath);

        StartCoroutine(MoveEnemy());
        if (agent.pathStatus != NavMeshPathStatus.PathComplete)
        {
          //  print("no path");

        }
    }


    private IEnumerator MoveEnemy()
    {
            agent.SetPath(moveToPath);
            yield return null;
    }


}
