using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour
{

    public Transform destination;
    public float movementSpeed;
    public float health = 1000f;
    public float timeToDestroyIfNoPath = 2f;
    public LayerMask TowerLayer;
    float DestroyTowerTimer;
    NavMeshAgent agent;
    NavMeshPath moveToPath;
    PlayerLifeManager plManager;
    Collider[] targets;
    float startHealth;


    // Start is called before the first frame update
    void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        moveToPath = new NavMeshPath();
        plManager = GameObject.Find("Game Manager").GetComponent<PlayerLifeManager>();
        agent.speed = movementSpeed;
        startHealth = health;
    }

    // Update is called once per frame
    void Update()
    {
        if (health <= 0)
        {
            gameObject.SetActive(false);
        }

        NavMesh.CalculatePath(transform.position, destination.position, agent.areaMask, moveToPath);

        if (gameObject.activeInHierarchy)
        {
            StartCoroutine(MoveEnemy());
        }

        if (DestroyTowerTimer >= timeToDestroyIfNoPath)
        {
            LookForTower();
        }

        if (agent.pathStatus != NavMeshPathStatus.PathComplete)
        {
            DestroyTowerTimer += Time.deltaTime;
        }
        else
        {
            DestroyTowerTimer = 0;
        }


        if ((transform.position - destination.position).sqrMagnitude < 0.1f)
        {
            gameObject.SetActive(false);
            plManager.DecrementLife();
        }
    }

    private void OnEnable()
    {
        health = startHealth;
    }

    private void LookForTower()
    {
        Collider ClosestTarget = null;
        targets = Physics.OverlapSphere(transform.position, 2f, TowerLayer, QueryTriggerInteraction.Collide);

        for (int i = 0; i < targets.Length; i++)
        {
            if (ClosestTarget == null)
            {
                ClosestTarget = targets[0];
            }
            if ((ClosestTarget.transform.position - destination.transform.position).sqrMagnitude < (targets[i].transform.position - destination.transform.position).sqrMagnitude)
            {
                ClosestTarget = targets[i];
            }
        }

        if (targets.Length > 0)
            DestroyClosestTarget(ClosestTarget);

    }

    private void DestroyClosestTarget(Collider closestTarget)
    {
        closestTarget.gameObject.SetActive(false);
    }

    private IEnumerator MoveEnemy()
    {
        agent.SetPath(moveToPath);
        yield return null;
    }


}
