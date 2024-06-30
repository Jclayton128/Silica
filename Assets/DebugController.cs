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
    }

    private void StartNewRun()
    {
        Debug.Log("new run!");
        GameController.Instance.InitializeNewPlayer();
    }
}
