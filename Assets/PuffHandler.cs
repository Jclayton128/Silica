using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuffHandler : MonoBehaviour
{
    private void Start()
    {
        PlayerController.Instance.CurrentArena.ArenaShuttingDown += HandleArenaShuttingDown;
    }

    private void HandleArenaShuttingDown()
    {
        PlayerController.Instance.CurrentArena.ArenaShuttingDown -= HandleArenaShuttingDown;
        Destroy(gameObject);
    }

    private void OnDestroy()
    {        
        PlayerController.Instance.CurrentArena.ArenaShuttingDown -= HandleArenaShuttingDown;
        
    }
}
