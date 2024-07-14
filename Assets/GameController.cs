using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public static GameController Instance { get; private set; }
    public Action RunStarted;
   
    [SerializeField] PlayerDataHolder _playerPrefab; 

    //settings
    int _limiterSpawnThreshold = 2;

    //state
    public PlayerDataHolder Player { get; private set; }

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        NodeController.Instance.CurrentNodesUpdated += CheckForLimiterSpawn;
    }



    public void InitializeNewPlayer()
    {
        if (Player == null)
        {
            Player = Instantiate(_playerPrefab);
        }
        else
        {
            Debug.LogWarning("Player already existed; deleted old player.");
            Destroy(Player);
            Player = Instantiate(_playerPrefab);
        }

        SetupNewRun();
    }

    private void SetupNewRun()
    {
        RunStarted?.Invoke();
    }

    private void CheckForLimiterSpawn(int nodesAscended)
    {
        if (nodesAscended >= _limiterSpawnThreshold)
        {            
            LimiterController.Instance.SpawnLimiter();
        }
    }
}
