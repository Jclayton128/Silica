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
        NodeController.Instance.CurrentNodeUpdated = HandleNewCurrentNode;
    }

    private void HandleNewCurrentNode()
    {
        _pos = NodeController.Instance.CurrentNode.transform.position;
        //_pos.x = 0;
        this.gameObject.transform.position = _pos;
    }
}
