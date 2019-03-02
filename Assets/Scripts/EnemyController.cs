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

    float _DestroyTowerTimer;
    NavMeshAgent _agent;
    NavMeshPath _moveToPath;
    PlayerLifeManager _plManager;
    Collider[] _targets;
    float _startHealth;


    // Start is called before the first frame update
    void Awake()
    {
        _agent = GetComponent<NavMeshAgent>();
        _moveToPath = new NavMeshPath();
        _plManager = GameObject.Find("Game Manager").GetComponent<PlayerLifeManager>();
        _agent.speed = movementSpeed;
        _startHealth = health;
    }

    // Update is called once per frame
    void Update()
    {
        if (health <= 0)
        {
            gameObject.SetActive(false);
        }


        if (gameObject.activeInHierarchy)
        {
            NavMesh.CalculatePath(transform.position, destination.position, _agent.areaMask, _moveToPath);
            StartCoroutine(MoveEnemy());
        }

        if (_DestroyTowerTimer >= timeToDestroyIfNoPath)
        {
            LookForTower();
        }

        if (_agent.pathStatus != NavMeshPathStatus.PathComplete)
        {
            _DestroyTowerTimer += Time.deltaTime;
        }
        else
        {
            _DestroyTowerTimer = 0;
        }


        if ((transform.position - destination.position).sqrMagnitude < 0.1f)
        {
            gameObject.SetActive(false);
            _plManager.DecrementLife();
        }
    }

    private void OnEnable()
    {
        health = _startHealth;
    }

    private void LookForTower()
    {
        Collider ClosestTarget = null;
        _targets = Physics.OverlapSphere(transform.position, 2f, TowerLayer, QueryTriggerInteraction.Collide);

        for (int i = 0; i < _targets.Length; i++)
        {
            if (ClosestTarget == null)
            {
                ClosestTarget = _targets[0];
            }
            if ((ClosestTarget.transform.position - destination.transform.position).sqrMagnitude < (_targets[i].transform.position - destination.transform.position).sqrMagnitude)
            {
                ClosestTarget = _targets[i];
            }
        }

        if (_targets.Length > 0)
            DestroyClosestTarget(ClosestTarget);

    }

    private void DestroyClosestTarget(Collider closestTarget)
    {
        closestTarget.gameObject.SetActive(false);
    }

    private IEnumerator MoveEnemy()
    {
        _agent.SetPath(_moveToPath);
        yield return null;
    }


}
