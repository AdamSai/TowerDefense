using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour
{
    public NavMeshSurface surface;
    public string enemyName = "Enemy";
    public Transform destination;
    public float movementSpeed;
    public float health = 1000f;
    public float timeToDestroyIfNoPath = 2f;
    public LayerMask TowerLayer;
    public float _maxHealth { get; private set; }
    public Animator animator;
    GoldManager _gold;
    float _DestroyTowerTimer;
    NavMeshAgent _agent;
    NavMeshPath _moveToPath;
    PlayerLifeManager _plManager;
    Collider[] _targets;
    GameObject _gameManager;
    RoundManager _roundManager;
    int _curRound;


    // Start is called before the first frame update
    void Awake()
    {
        _gameManager = GameObject.Find("Game Manager");
        _agent = GetComponent<NavMeshAgent>();
        _moveToPath = new NavMeshPath();
        _plManager = _gameManager.GetComponent<PlayerLifeManager>();
        _agent.speed = movementSpeed;
        _maxHealth = health;
        _gold = _gameManager.GetComponent<GoldManager>();
        _roundManager = _gameManager.GetComponent<RoundManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if (_agent.velocity.magnitude > 2)
        {
            animator.SetBool("isRunning", true);
            animator.SetBool("isWalking", false);
        }
        else if (_agent.velocity.magnitude > 0.1f && _agent.velocity.magnitude <= 2)
        {
            animator.SetBool("isRunning", false);
            animator.SetBool("isWalking", true);
        }
        else
        {
            animator.SetBool("isWalking", false);
            animator.SetBool("isRunning", false);
        }
        _curRound = _roundManager._currentRound;
        if (health <= 0)
        {
            int rand;
            if(_curRound < 5)
                rand = UnityEngine.Random.Range(1, 4);
            else if(_curRound < 15)
                rand = UnityEngine.Random.Range(3, 7);
            else if(_curRound < 25)
                rand = UnityEngine.Random.Range(6, 10);
            else
                rand = UnityEngine.Random.Range(10, 20);
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
        var endPos = new Vector3(destination.position.x, transform.position.y, destination.position.z);
        if ((transform.position - endPos).sqrMagnitude < 0.1f)
        {
            gameObject.SetActive(false);
            _plManager.DecrementLife();
        }
    }

    private void OnEnable()
    {
        _maxHealth += _curRound * 13;
        health = _maxHealth;
        print("health: " + (_maxHealth + _curRound * 15));
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
        surface.BuildNavMesh();
    }

    private IEnumerator MoveEnemy()
    {
        _agent.SetPath(_moveToPath);
        yield return null;
    }


}
