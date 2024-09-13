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

        if (Input.GetKeyUp(KeyCode.B))
        {
            GenerateTestBug();
        }

        if (Input.GetKeyUp(KeyCode.M))
        {
            GenerateMainframeNode();
        }
    }

    private void GenerateAvailableNode()
    {
        NodeController.Instance.SpawnNode(NodeHandler.NodeStates.Available);
    }

    private void GenerateMainframeNode()
    {
        NodeController.Instance.SpawnNode(NodeHandler.NodeStates.Available,
            NodeHandler.NodeTypes.Mainframe);
    }

    private void StartNewRun()
    {
        Debug.Log("new run!");
        //ArenaController.Instance.CreateNewCurrentArena();


        GameController.Instance.InitializeNewRun();

    }
    private void GenerateTestBug()
    {
        BugController.Instance.SpawnBug(BugHandler.BugTypes.Test);
    }
}
