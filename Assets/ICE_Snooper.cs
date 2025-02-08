using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class ICE_Snooper : Ice, IDestroyable, IAlertable
{
    //This thing goes from node to node until it has visited them all.
    //If it detects the player nearby, it becomes Alert.
    //If Alert, it orbits the player while firing bullets until no longer Alert.

    //refs
    [SerializeField] SpriteRenderer _sr = null;
    AlertRingHandler _alertRingHandler;

    //settings
    [SerializeField] float _moveSpeed_Max = 1f;
    [SerializeField] float _moveAccel = 0.4f;
    [SerializeField] float _timeToScanPosition = 2f;
    [SerializeField] float _scanRange = 2f;

    [Header("Alarming")]
    [SerializeField] Color _color_Unalert = Color.yellow;
    [SerializeField] Color _color_Alert = Color.red;
    [SerializeField] float _alertScalingRate = 2f;


    //State
     List<NodeHandler> _unvisitedNodes = new List<NodeHandler>();
    Vector3 _targetPosition = Vector2.zero;
    Vector3 _dir;
    float _moveSpeed_Current;
    Vector3 _movement;
    float _timeAtTargetPosition;
    float _playerDist;
    float _alertFactor;
    bool _isAlerting = false;


    protected override void Awake()
    {
        base.Awake();
        _alertRingHandler = GetComponentInChildren<AlertRingHandler>();
    }

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
        transform.up = _dir.normalized;

        if (_dir.magnitude < _scanRange)
        {
            UpdateRotationalMovement();
            _moveSpeed_Current -= _moveAccel * Time.deltaTime;
            ScanForPlayerAtTargetPosition();
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
                    //DOTween.To(() => transform.up, x => transform.up = x, _dir.normalized, 1);
                }
            }
        }
        else
        {
            _moveSpeed_Current += _moveAccel * Time.deltaTime;
            UpdateLinearMovement();
        }

        Debug.DrawLine(transform.position, _targetPosition, Color.red);

    }

    private void UpdateLinearMovement()
    {
        _moveSpeed_Current = Mathf.Clamp(_moveSpeed_Current, 0, _moveSpeed_Max);
        _movement = _dir.normalized * _moveSpeed_Current * Time.deltaTime;
        transform.position += _movement;
    }

    private void UpdateRotationalMovement()
    {
        _movement = transform.right * _moveSpeed_Max * Time.deltaTime;
        transform.position += _movement;

        //if (_polarPosition == null)
        //{
        //    _polarPosition = PolarCoord.ConvertCartesianToPolar(transform.position);
        //}
        ////if alert to player and within firing range, begin to orbit player node.
        //_polarPosition.Radius += _radialVelocity;
        //_polarPosition.AngleDegree += _rotationalVelocity;
        ////transform.position = (Vector3)_polarPosition.ToCartesianCoords();
        //transform.position = _targetPosition + (Vector3)_polarPosition.ToCartesianCoords();
    }

    private void ScanForPlayerAtTargetPosition()
    {
        _playerDist = (PlayerController.Instance.CurrentPlayer.CurrentNode.transform.position - _targetPosition).magnitude;
        if (_playerDist <= _scanRange)
        {
            _alertFactor += Time.deltaTime * _alertScalingRate;
            if (_alertFactor >= 1)
            {
                StartAlerting();
            }
        }
        else
        {
            _alertFactor -= Time.deltaTime * _alertScalingRate;
            if (_alertFactor < 0.9f)
            {
                StopAlerting();
            }
            
        }
        _alertFactor = Mathf.Clamp01(_alertFactor);
        _sr.color = Color.Lerp(_color_Unalert, _color_Alert, _alertFactor);
    }

    #endregion

    #region Health

    public void HandleHealthDrop(float factorRemaining)
    {
        //TODO
    }

    public void HandleZeroHealth()
    {

        Destroy(gameObject);
    }

    #endregion

    #region Alerting

    public void StartAlerting()
    {
        if (_isAlerting) return; //This becomes true after this function is resolved
        _isAlerting = true;
        _alertRingHandler.AlertOthers();
    }

    private void StopAlerting()
    {
        _isAlerting = false;

        _alertRingHandler.StopAlerting();
    }

    public void SetMaxAlert()
    {
        StartAlerting();
        _alertFactor = 1;
        _sr.color = Color.Lerp(_color_Unalert, _color_Alert, _alertFactor);
        _targetPosition = PlayerController.Instance.CurrentPlayer.CurrentNode.transform.position;
    }

    #endregion
}
