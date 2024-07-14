using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class LevelController : MonoBehaviour
{
    public Action LevelCountIncrease;

    public static LevelController Instance { get; private set; }

    //settings
    [SerializeField] float _timeBetweenBugs = 3;

    //state
    int _levelCount = 0;
    public int LevelCount => _levelCount;

    float _timeUntilNextBug;


    private void Awake()
    {
        Instance = this;
        _timeUntilNextBug = Mathf.Infinity;
    }
    private void Start()
    {
        GameController.Instance.RunStarted += HandleRunStarted;
    }

    private void HandleRunStarted()
    {
        _timeUntilNextBug = _timeBetweenBugs;
        _levelCount = 0;
        IncreaseLevelCount();
    }

    public void IncreaseLevelCount()
    {
        _levelCount++;
    }

    private void Update()
    {
        _timeUntilNextBug -= Time.deltaTime;
        if (_timeUntilNextBug <= 0)
        {
            BugController.Instance.SpawnBug(BugHandler.BugTypes.Test);
            _timeUntilNextBug = _timeBetweenBugs;
        }
    }

}
