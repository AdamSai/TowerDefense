﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class RoundManager : MonoBehaviour
{
    public int AmountOfRounds = 10;
    public float EndOfRoundTime = 30f;
    public Transform enemyContainer;
    public int _currentRound  = 1;
    public TextMeshProUGUI roundText;
    public TextMeshProUGUI timerText;
    public bool isFlyingRound  = false;
    public bool isBossRound  = false;
    public GameObject startEarlybutton;
    float _endOfRoundTracker = 0f;
    bool _isEndOfRound = true;
    Transform[] _enemies;
    EnemySpawner _enemySpawner;
    GoldManager goldManager;

    private void Awake()
    {
        _enemySpawner = GetComponent<EnemySpawner>();
        goldManager = GetComponent<GoldManager>();
    }
    // Start is called before the first frame update
    void Start()
    {
        _isEndOfRound = true;
        roundText.text = $"Round: 0{_currentRound}";
    }

    // Update is called once per frame
    void Update()
    {
       
        _enemies = enemyContainer.GetComponentsInChildren<Transform>(false);
        if (_isEndOfRound)
        {
            timerText.text = $"Round starting: {(EndOfRoundTime - _endOfRoundTracker).ToString("F2")}";
            _endOfRoundTracker += Time.deltaTime;

        }
        if (_endOfRoundTracker >= EndOfRoundTime)
        {
            StartCoroutine(StartRound());
        }
        if (_isEndOfRound && Time.timeScale > 0)
            startEarlybutton.SetActive(true);
        else
            startEarlybutton.SetActive(false);

    }


    IEnumerator StartRound()
    {
        timerText.text = "";
        if (_currentRound % 7 == 0)
        {
            isFlyingRound = true;
        }
        else
        {
            isFlyingRound = false;
        }
        if (_currentRound % 5 == 0)
        {
            isBossRound = true;
        }
        else
        {
            isBossRound = false;
        }
        _enemySpawner.SpawnEnemies();
        _isEndOfRound = false;
        _endOfRoundTracker = 0;
        yield return new WaitForSeconds(2);
        yield return new WaitUntil(() => _enemies.Length == 1);
        goldManager.AddGold(_currentRound * 11);
        _currentRound++;
        if (_currentRound >= 2)
            EndOfRoundTime = 15f;
        _isEndOfRound = true;
        roundText.text = $"Round: {((_currentRound < 10) ? $"0{_currentRound}" : _currentRound.ToString())}";
    }

    public void StartRoundEarly()
    {
        _endOfRoundTracker = 0.01f;
        StartCoroutine(StartRound());
    }
}
