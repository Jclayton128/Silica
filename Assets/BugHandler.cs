using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BugHandler : MonoBehaviour, IDestroyable
{
    public enum BugTypes { Test, Goblin, Ogre, Bat, Snake }

    public enum BugBehaviors { StraightFall, Path, Oscillate, Dodge, SeekPlayer, ErraticSprint }


    //refs
    Rigidbody2D _rb;
    Collider2D _coll;
    SpriteRenderer _sr;

    HealthHandler _health;

    //settings
    
    [SerializeField] BugPath[] _pathMenu = null;
    [SerializeField] float _pathScale = 2f;
    [SerializeField] float _closeEnough = 0.02f;
    [SerializeField] float _moveSpeed = 2f;
    [SerializeField] BugTypes _bugType = BugTypes.Test;
    [SerializeField] BugBehaviors _bugBehavior = BugBehaviors.StraightFall;
    [SerializeField] float _damage = 1;
    [SerializeField] float _corruption = 0.1f;

    //references
    [SerializeField] ParticleSystem[] _ps = null;

    //state
    bool _isInitialized = false;
    bool _isActivated = false;
    public BugTypes BugType => _bugType;
    public BugBehaviors BugBehavior => _bugBehavior;
    public float Corruption => _corruption;

    //pathing state
    int _sign = 1;
    public BugPath CurrentPath => _currentPath;
    BugPath _currentPath;
    int _currentWaypointIndex = 0;
    Vector3 _currentWaypoint = Vector2.zero;
    Vector3 _waypointZero = Vector2.zero;
    float _dist = Mathf.Infinity;
    Vector3 _vel;

    //private void Awake()
    //{
    //    Initialize();
    //}

    public void Initialize()
    {
        if (_isInitialized) return;
        _rb = GetComponent<Rigidbody2D>();
        _coll = GetComponent<Collider2D>();
        _sr = GetComponent<SpriteRenderer>();
        _health = GetComponent<HealthHandler>();
        _isInitialized = true;
    }

    public void ActivateBug()
    {
        Initialize();

        _isActivated = true;

        _rb.simulated = true;
        _coll.enabled = true;
        _sr.enabled = true;
        if (_ps.Length>0) _ps[0]?.Play();
        _health.ResetHealth();

        SetupMoveType();

    }

    private void SetupMoveType()
    {
        //switch (_bugBehavior)
        //{
        //    case BugBehaviors.Path:

        //        int rand = UnityEngine.Random.Range(0, _pathMenu.Length);
        //        _currentPath = _pathMenu[rand];
        //        _currentWaypointIndex = 0;
        //        _sign = (2 * UnityEngine.Random.Range(0, 2)) - 1;
        //        if (transform.position.x + _currentPath.MaxX + 1 >= ArenaController.Instance.XSpan)
        //        {
        //            Vector3 pos = transform.position;
        //            pos.x *= -1;
        //            transform.position = pos;
        //        }

        //        _waypointZero = transform.position;

        //        _currentWaypoint = 
        //            ((Vector3)_currentPath.Waypoints[_currentWaypointIndex] * _pathScale)
        //            + _waypointZero;
        //        _currentWaypoint.x *= _sign;


        //        return;

        //    case BugBehaviors.StraightFall:
        //        _currentWaypoint = _waypointZero;
        //        _currentWaypoint.y = -9999;
        //        //_rb.velocity = -transform.up * _moveSpeed;
        //        return;

        //}
    }

    public void DeactivateBug()
    {
        BugController.Instance.CheckinDeactivatedBug(BugType, this);
    }

    public void HandleZeroHealth()
    {
        _isActivated = false;
        if (_ps.Length > 0) _ps[0]?.Stop();
        _rb.simulated = false;
        _coll.enabled = false;
        _sr.enabled = false;
    }

    public void OnParticleSystemStopped()
    {
        DeactivateBug();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        HealthHandler hh;
        if (collision.TryGetComponent<HealthHandler>(out hh))
        {
            hh.ApplyDamage(_damage);
        }
    }

    public void HandleHealthDrop(float factorRemaining)
    {
        float a = _sr.color.a;
        Color col = _sr.color;
        float n = Mathf.Lerp(0.5f, 1f, factorRemaining);
        //TODO remove magic numbers. What about things that are alway a little translucent?
        col.a = n;
        _sr.color = col;
    }

    #region Flow

    private void Update()
    {
        if (!_isActivated) return;
        if (_bugBehavior == BugBehaviors.Path)
        {
            UpdatePathing();
        }
        UpdateMovement();

    }


    private void UpdatePathing()
    {
        _dist = (_currentWaypoint - transform.position).magnitude;
        if (_dist < _closeEnough)
        {
            _currentWaypointIndex++;
            _currentWaypoint = ((Vector3)_currentPath.Waypoints[_currentWaypointIndex] * _pathScale)
                + _waypointZero;
            _currentWaypoint.x *= _sign;
        }
        //else if (_dist < _closeEnough * 2f)
        //{
        //    transform.position = _currentWaypoint;
        //}

    }
    private void UpdateMovement()
    {
        _vel = (_currentWaypoint - transform.position).normalized * _moveSpeed;
        transform.position += _vel * Time.deltaTime;
    }

    #endregion

}
