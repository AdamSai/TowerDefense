using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoundManager : MonoBehaviour
{
    public int AmountOfRounds = 10;
    public float EndOfRoundTime = 30f;
    public Transform enemyContainer;

    public int _currentRound { get; private set; } = 1;
    float _endOfRoundTracker = 0f;
    bool _isEndOfRound = true;
    Transform[] _enemies;
    EnemySpawner _enemySpawner;

    private void Awake()
    {
        _enemySpawner = GetComponent<EnemySpawner>();
    }
    // Start is called before the first frame update
    void Start()
    {
        _isEndOfRound = true;
    }

    // Update is called once per frame
    void Update()
    {
        _enemies = enemyContainer.GetComponentsInChildren<Transform>(false);
        if (_isEndOfRound)
        {
            _endOfRoundTracker += Time.deltaTime;

        } if(_endOfRoundTracker >= EndOfRoundTime)
        {
            StartCoroutine(StartRound());
        }
    }


    IEnumerator StartRound()
    {
        _enemySpawner.SpawnEnemies();
        _endOfRoundTracker = 0;
        _isEndOfRound = false;
        yield return new WaitForSeconds(2);
        yield return new WaitUntil(() => _enemies.Length == 1);
        _currentRound++;
        _isEndOfRound = true;


    }
}
