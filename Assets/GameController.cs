using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public static GameController Instance { get; private set; }
    public Action RunStarting;
    public Action RunStarted;  
    public enum GameModes { Title, InRun, Editor}


    //settings

    //state
    public GameModes GameMode => _gameMode;
    GameModes _gameMode = GameModes.Title;

    private void Awake()
    {
        Instance = this;
    }

    //private void Start()
    //{
    //    NodeController.Instance.CurrentNodesUpdated += CheckForLimiterSpawn;
    //}

    public bool RequestEnterEditorMode()
    {
        if (_gameMode == GameModes.Title)
        {
            _gameMode = GameModes.Editor;
            return true;
        }
        else
        {
            Debug.LogWarning("Cannot enter Editor Mode right now");
            return false;
        }
    }

    public bool RequestExitEditorMode()
    {
        if (_gameMode == GameModes.Editor)
        {
            _gameMode = GameModes.Title;
            return true;
        }
        else
        {
            Debug.LogWarning("Cannot exit Editor Mode right now");
            return false;
        }
    }

    public void InitializeNewRun()
    {
        if (_gameMode == GameModes.Title)
        {
            RunStarting?.Invoke();

            ServerController.Instance.SetupNewGame();
            PlayerController.Instance.InitializeNewPlayer();

            _gameMode = GameModes.InRun;

            RunStarted?.Invoke();
        }

    }

    private void CheckForLimiterSpawn(int nodesAscended)
    {
        //if (nodesAscended >= _limiterSpawnThreshold)
        //{            
        //    LimiterController.Instance.SpawnLimiter();
        //}
    }
}
