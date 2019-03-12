using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

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
    public ObjectPooler healthbarPooler;
    GoldManager _gold;
    float _DestroyTowerTimer;
    NavMeshAgent _agent;
    NavMeshPath _moveToPath;
    PlayerLifeManager _plManager;
    Collider[] _targets;
    GameObject _gameManager;
    RoundManager _roundManager;
    Slider healthbar;
    int _curRound;
    bool lookingForTower = false;


    // Start is called before the first frame update
    void Awake()
    {
        _gameManager = GameObject.Find("Game Manager");
        _agent = GetComponent<NavMeshAgent>();
        if (_agent)
        {
            _moveToPath = new NavMeshPath();
            _agent.speed = movementSpeed;

        }
        _plManager = _gameManager.GetComponent<PlayerLifeManager>();
        _maxHealth = health;
        _gold = _gameManager.GetComponent<GoldManager>();
        _roundManager = _gameManager.GetComponent<RoundManager>();
        _curRound = _roundManager._currentRound;
        healthbar = healthbarPooler.GetPooledObject().GetComponent<Slider>();
        healthbar.transform.SetParent(GameObject.Find("Canvas").GetComponent<Transform>(), false);
        lookingForTower = false;
    }

    // Update is called once per frame
    void Update()
    {
        HandleDeath();
        HealthbarController();

        var endPos = new Vector3(destination.position.x, transform.position.y, destination.position.z);
        //Only use these animations if it is not a flying round
        if (_agent && _curRound % 7 != 0)
        {
            if (_curRound % 5 == 0)
            {
                animator.SetBool("isWalking", true);
                animator.SetBool("isRunning", false);
            }
            else
            {
                animator.SetBool("isWalking", false);
                animator.SetBool("isRunning", true);
            }

            if (gameObject.activeInHierarchy)
            {
                NavMesh.CalculatePath(transform.position, destination.position, _agent.areaMask, _moveToPath);
                StartCoroutine(MoveEnemy());
            }

            if (_DestroyTowerTimer >= timeToDestroyIfNoPath && gameObject.activeInHierarchy)
            {
                if ((transform.position - _agent.destination).sqrMagnitude < 10f)
                {

                    if (!lookingForTower)
                    {
                        StartCoroutine(LookForTower());
                    }
                }
            }
            if (_agent.pathStatus != NavMeshPathStatus.PathComplete)
            {
                _DestroyTowerTimer += Time.deltaTime;
            }
            else
            {
                _DestroyTowerTimer = 0;
            }

        }
        else
        {
            transform.LookAt(endPos);
            transform.position = Vector3.MoveTowards(transform.position, endPos, movementSpeed * Time.deltaTime);
        }
        if ((transform.position - endPos).sqrMagnitude < 0.1f)
        {
            gameObject.SetActive(false);
            _plManager.DecrementLife();
        }
    }

    private void HealthbarController()
    {
        if (healthbar.IsActive())
        {
            healthbar.value = health;
            if (_curRound % 7 == 0)
                healthbar.transform.position = Camera.main.WorldToScreenPoint(new Vector3(transform.position.x, transform.position.y, transform.position.z));
            else
                healthbar.transform.position = Camera.main.WorldToScreenPoint(new Vector3(transform.position.x, transform.position.y, transform.position.z));
        }
    }

    private void HandleDeath()
    {
        if (health <= 0)
        {
            int rand;
            if (_curRound < 10)
                rand = UnityEngine.Random.Range(1, 4);
            else if (_curRound < 20)
                rand = UnityEngine.Random.Range(3, 7);
            else if (_curRound < 30)
                rand = UnityEngine.Random.Range(6, 10);
            else
                rand = UnityEngine.Random.Range(10, 20);
            if (_curRound % 5 == 0)
                rand += 50 * (_curRound / 5);
            if (_curRound % 7 == 0)
                rand *= 2;
            _gold.AddGold(rand);

            gameObject.SetActive(false);
            healthbar.gameObject.SetActive(false);
        }
    }

    private void OnEnable()
    {
        healthbar.gameObject.SetActive(true);
        _curRound = _roundManager._currentRound;

        if (_roundManager.isBossRound)
        {
            _maxHealth = _curRound * 1100;
        }
        else if (_curRound == 1)
        {
            _maxHealth = 10;
        }
        else
        {
            _maxHealth = 12 * _curRound;
        }
        if (_curRound > 5)
        {
            if (_curRound > 10 && _curRound % 7 != 0)
                _maxHealth *= Mathf.Floor(_curRound / 5);
            else if(_curRound % 7 != 0)
                _maxHealth *= 2;
        }
        health = _maxHealth;
        healthbar.maxValue = _maxHealth;
        print("health: " + (health));
    }

    private IEnumerator LookForTower()
    {
        lookingForTower = true;
        Collider ClosestTarget = null;
        _targets = Physics.OverlapSphere(transform.position, 5f, TowerLayer, QueryTriggerInteraction.Collide);

        for (int i = 0; i < _targets.Length; i++)
        {
            if (ClosestTarget == null)
            {
                ClosestTarget = _targets[0];
            }
            if ((ClosestTarget.transform.position - destination.transform.position).sqrMagnitude > (_targets[i].transform.position - destination.transform.position).sqrMagnitude)
            {
                ClosestTarget = _targets[i];
            }
        }

        if (_targets.Length > 0)
            DestroyClosestTarget(ClosestTarget);

        yield return new WaitForSeconds(0.1f);
        lookingForTower = false;

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
