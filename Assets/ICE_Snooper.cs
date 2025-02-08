using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ICE_Snooper : Ice, IDestroyable
{
    //This thing goes from node to node until it has visited them all.
    //If it detects the player nearby, it becomes Alert.
    //If Alert, it orbits the player while firing bullets until no longer Alert.

    //refs
    [SerializeField] SpriteRenderer _sr = null;

    //settings
    [SerializeField] float _moveSpeed_Max = 1f;
    [SerializeField] float _moveAccel = 0.4f;
    [SerializeField] float _timeToScanPosition = 2f;
    [SerializeField] float _scanRange = 2f;
    [SerializeField] Color _color_Unalert = Color.yellow;
    [SerializeField] Color _color_Alert = Color.red;
    [SerializeField] float _alertScaling = 2f;

    //State
     List<NodeHandler> _unvisitedNodes = new List<NodeHandler>();
    Vector3 _targetPosition = Vector2.zero;
    Vector3 _dir;
    float _moveSpeed_Current;
    Vector3 _movement;
    float _timeAtTargetPosition;
    [SerializeField] float _playerDist;
    float _alertFactor;
    bool _hasDetectedPlayer = false;

    protected override void Start()
    {
        base.Start();
        _targetPosition = GetNextNodeToVisit();
    }

    private void ResetUnvisitedNodes()
    {
        _unvisitedNodes = new List<NodeHandler>(PlayerController.Instance.CurrentArena.AllNodes);
        //pDebug.Log($"resetting nodes. Now have {_unvisitedNodes.Count} nodes to visit");
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
        _dir = (_targetPosition - transform.position);
        _moveSpeed_Current = Mathf.Clamp(_moveSpeed_Current, 0, _moveSpeed_Max);
        _movement = _dir.normalized * _moveSpeed_Current * Time.deltaTime;
        transform.position += _movement;

        if (_dir.magnitude < _scanRange)
        {
            _moveSpeed_Current -= _moveAccel * Time.deltaTime;
            ScanForPlayer();
            if (_alertFactor > 0.1f)
            {
                //keep scanning
            }
            else
            {
                _timeAtTargetPosition += Time.deltaTime;
                if (_timeAtTargetPosition > _timeToScanPosition)
                {
                    _targetPosition = GetNextNodeToVisit();
                }
            }
        }
        else
        {
            _moveSpeed_Current += _moveAccel * Time.deltaTime;
        }

        Debug.DrawLine(transform.position, _targetPosition, Color.red);

    }

    private void ScanForPlayer()
    {
        _playerDist = (PlayerController.Instance.CurrentPlayer.CurrentNode.transform.position - transform.position).magnitude;
        if (_playerDist <= _scanRange)
        {
            _alertFactor += Time.deltaTime;
        }
        else
        {
            _alertFactor -= Time.deltaTime;
        }
        _alertFactor = Mathf.Clamp01(_alertFactor);
        _sr.color = Color.Lerp(_color_Unalert, _color_Alert, _alertFactor);
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
