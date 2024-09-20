using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MineBrain : MonoBehaviour, IDestroyable
{
    //Gradually drift. Check periodically for  player within range.
    //If player within detection range, home in on it

    //ref
    Rigidbody2D _rb;
    ParticleSystem _psSeeker;
    ParticleSystem.MainModule _psm;
    SpriteRenderer _sr;
    DamageHandler _dh;

    //settings
    [SerializeField] float _lifetime = 20f;
    [SerializeField] float _minDrag = 0.01f;
    [SerializeField] float _maxDrag = 0.1f;
    [SerializeField] float _timeBetweenScans = 6f;
    [SerializeField] float _scanRange = 1f;
    [SerializeField] Color _colorOnceDetect = Color.cyan;
    [SerializeField] float _seekSpeed = 1f;
    [SerializeField] PuffHandler _explodePuff = null;
    [SerializeField] bool _willTargetPackets = true;
    [SerializeField] bool _willTargetCurrentNode = true;

    //state
    Ice_DriftLayer _maker;
    Transform _targetTransform;
    Vector2 _targetVec;
    float _timeForNextScan;
    int _layerMask_PacketNode;

    public void Initialize(Vector2 velocity, Ice_DriftLayer maker)
    {
        _dh = GetComponent<DamageHandler>();
        _dh.DamageDealt += HandleZeroHealth;
        _sr = GetComponent<SpriteRenderer>();
        _psSeeker = GetComponent<ParticleSystem>();
        _psm = _psSeeker.main;
        _rb = GetComponent<Rigidbody2D>();
        _rb.velocity = velocity;
        _rb.drag = UnityEngine.Random.Range(_minDrag, _maxDrag);
        _maker = maker;
        _timeForNextScan = _timeBetweenScans;

        if (_willTargetCurrentNode && _willTargetPackets)
        {
            _layerMask_PacketNode = (1 << 6) | (1 << 7);
        }
        else if (!_willTargetCurrentNode && _willTargetPackets)
        {
            _layerMask_PacketNode = (1 << 6);
        }
        else if (_willTargetCurrentNode && !_willTargetPackets)
        {
            _layerMask_PacketNode = (1 << 7);
        }
        else
        {
            _layerMask_PacketNode = 1;
        }

    }

    private void Update()
    {

        //check periodically for player
        if (!_targetTransform)
        {
            _lifetime -= Time.deltaTime;
            if (_lifetime <= 0)
            {
                DestroyMine(false);
            }
            else if (Time.time >= _timeForNextScan)
            {
                Invoke(nameof(Scan), _psm.startLifetime.constant / 2f);
                _psSeeker.Emit(30);
                _timeForNextScan = Time.time + _timeBetweenScans;
            }
        }
        else
        {
            _targetVec = _targetTransform.position - transform.position;
            _rb.velocity = _targetVec.normalized *
                Mathf.Clamp((_seekSpeed/_targetVec.magnitude), 0.1f, _seekSpeed);
        }

    }

    private void Scan()
    {
        var hit = Physics2D.OverlapCircle(transform.position, _scanRange, _layerMask_PacketNode);

        if (hit)
        {
            //signal detect
            _targetTransform = hit.transform;
            _sr.color = _colorOnceDetect;
        }
    }

    public void DestroyMine(bool shouldDieInstantly)
    {
        _maker.DeregisterMineUponItsHealthZero(this);
        if (!shouldDieInstantly)
        {
            //if false, show death effect
            Instantiate(_explodePuff, transform.position, transform.rotation);
            Destroy(gameObject);
        }

        if (shouldDieInstantly)
        {
            Destroy(gameObject);
        }
    }

    public void HandleZeroHealth()
    {
        DestroyMine(false);
    }

    public void HandleHealthDrop(float factorRemaining)
    {
        //
    }
}
