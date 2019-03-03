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

    GoldManager _gold;
    float _DestroyTowerTimer;
    NavMeshAgent _agent;
    NavMeshPath _moveToPath;
    PlayerLifeManager _plManager;
    Collider[] _targets;
    GameObject _gameManager;
    float _startHealth;
    RoundManager _roundManager;


    // Start is called before the first frame update
    void Awake()
    {
        _gameManager = GameObject.Find("Game Manager");
        _agent = GetComponent<NavMeshAgent>();
        _moveToPath = new NavMeshPath();
        _plManager = _gameManager.GetComponent<PlayerLifeManager>();
        _agent.speed = movementSpeed;
        _startHealth = health;
        _gold = _gameManager.GetComponent<GoldManager>();
        _roundManager = _gameManager.GetComponent<RoundManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if (health <= 0)
        {
            int rand;
            var curRound = _roundManager._currentRound;
            if(curRound < 5)
                rand = UnityEngine.Random.Range(1, 4);
            else if(curRound < 10)
                rand = UnityEngine.Random.Range(3, 7);
            else if(curRound < 15)
                rand = UnityEngine.Random.Range(6, 10);
            else
                rand = UnityEngine.Random.Range(10, 20);

            print($"dropped {rand} gold");
            _gold.AddGold(rand);
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
