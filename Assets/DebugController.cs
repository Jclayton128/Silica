using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugController : MonoBehaviour
{
    
    void Update()
    {
        if (Input.GetKeyUp(KeyCode.N))
        {
            StartNewRun();

        }

        if (Input.GetKeyUp(KeyCode.P))
        {
            GenerateAvailableNode();
        }
    }

    private void GenerateAvailableNode()
    {
        NodeController.Instance.SpawnNode(NodeHandler.NodeStates.Available);
    }

    private void StartNewRun()
    {
        Debug.Log("new run!");
        GameController.Instance.InitializeNewPlayer();
    }
}
