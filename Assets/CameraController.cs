using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using System;

public class CameraController : MonoBehaviour
{
    public static CameraController Instance { get; private set; }
    CinemachineVirtualCamera _cvc;

    //state
    Vector2 _pos;


    private void Awake()
    {
        Instance = this;
    }
    private void Start()
    {
        _cvc = Camera.main.GetComponentInChildren<CinemachineVirtualCamera>();
        this.gameObject.transform.position = _pos;
        _cvc.Follow = this.transform;
        NodeController.Instance.CurrentNodesUpdated = HandleUpdatedCurrentNodes;
    }

    private void HandleUpdatedCurrentNodes()
    {
        _pos.y = NodeController.Instance.CurrentNodesCentroid;
        //_pos.x = 0;
        this.gameObject.transform.position = _pos;
    }
}
