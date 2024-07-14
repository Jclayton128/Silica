using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BugHandler : MonoBehaviour, IDestroyable
{
    public enum BugTypes {Test, Goblin, Ogre, Bat, Snake}

    public enum BugBehaviors { StraightFall, Oscillate, Dodge, SeekPlayer, ErraticSprint}

    //refs
    Rigidbody2D _rb;
    Collider2D _coll;
    SpriteRenderer _sr;
    ParticleSystem _ps;
    HealthHandler _health;

    //state
    bool _isInitialized = false;
    bool _isActivated = false;
    [SerializeField] float _moveSpeed = 2f;

    [SerializeField] BugTypes _bugType = BugTypes.Test;
    public BugTypes BugType => _bugType;

    [SerializeField] BugBehaviors _bugBehavior = BugBehaviors.StraightFall;
    public BugBehaviors BugBehavior => _bugBehavior;

    [SerializeField] float _damage = 1;
    public float Corruption => _corruption;
    [SerializeField] float _corruption = 0.1f;

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
        _ps = GetComponent<ParticleSystem>();
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
        if (_ps) _ps?.Play();
        _health.Reset();

        _rb.velocity = -transform.up * _moveSpeed;
    }

    public void DeactivateBug()
    {
        _isActivated = false;

        _rb.simulated = false;
        _coll.enabled = false;
        _sr.enabled = false;
        if (_ps) _ps?.Stop();

        BugController.Instance.CheckinDeactivatedBug(BugType, this);
    }

    public void HandleZeroHealth()
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

}
