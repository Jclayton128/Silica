using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public static GameController Instance { get; private set; }
    public Action RunStarting;
    public Action RunStarted;  


    //settings

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        NodeController.Instance.CurrentNodesUpdated += CheckForLimiterSpawn;
    }



    public void InitializeNewRun()
    {
        RunStarting?.Invoke();

        ServerController.Instance.SetupNewGame();
        PlayerController.Instance.InitializeNewPlayer();

        RunStarted?.Invoke();
    }

    private void CheckForLimiterSpawn(int nodesAscended)
    {
        //if (nodesAscended >= _limiterSpawnThreshold)
        //{            
        //    LimiterController.Instance.SpawnLimiter();
        //}
    }
}
