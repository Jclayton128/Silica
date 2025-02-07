using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ICE_Snooper : Ice, IDestroyable
{
    //This thing goes from node to node until it has visited them all.
    //If it detects the player nearby, it becomes Alert.
    //If Alert, it orbits the player while firing bullets until no longer Alert.

    //settings
    [SerializeField] float _moveSpeed = 1f;
    float _timeToScanPosition = 2f;
    float _scanRange = 2f;

    //State
    [SerializeField] List<NodeHandler> _unvisitedNodes = new List<NodeHandler>();
    Vector3 _targetPosition = Vector2.zero;
    Vector3 _dir;
    float _moveFactor;
    Vector3 _movement;
    float _timeAtTargetPosition;

    protected override void Start()
    {
        base.Start();
        _targetPosition = GetNextNodeToVisit();
    }

    private void ResetUnvisitedNodes()
    {

        _unvisitedNodes = new List<NodeHandler>(PlayerController.Instance.CurrentArena.AllNodes);
        //Debug.Log($"resetting nodes. Now have {_unvisitedNodes.Count} nodes to visit");
    }

    private Vector2 GetNextNodeToVisit()
    {
        if (_unvisitedNodes.Count == 0)
        {
            ResetUnvisitedNodes();
        }

        _timeAtTargetPosition = 0;

        int rand = UnityEngine.Random.Range(0, _unvisitedNodes.Count);
        var t = _unvisitedNodes[rand].transform.position;
        _unvisitedNodes.RemoveAt(rand);
        return (t);
    }

    #region Flow

    private void Update()
    {
        if (_targetPosition.magnitude > Mathf.Epsilon)
        {
            _dir = (_targetPosition - transform.position);
            //_moveFactor = Mathf.InverseLerp(0, 3f, )
            _movement = _dir.normalized * _moveSpeed * Time.deltaTime;
            transform.position += _movement;

            if (_dir.magnitude < _scanRange)
            {
                _timeAtTargetPosition += Time.deltaTime;
                if (_timeAtTargetPosition > _timeToScanPosition)
                {
                    _targetPosition = GetNextNodeToVisit();
                }
            }
        }
    }

    #endregion

    public void HandleHealthDrop(float factorRemaining)
    {
        //TODO
    }

    public void HandleZeroHealth()
    {
        Destroy(gameObject);
    }
}
