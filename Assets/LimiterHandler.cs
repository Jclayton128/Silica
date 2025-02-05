using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LimiterHandler : MonoBehaviour
{
    //ref
    Rigidbody2D _rb;
    //ParticleSystem _ps;
    SpriteRenderer _sr;
    Collider2D _coll;

    //state
    bool _isInitialized = false;
    float _currentSpeed = 0f;
    
    public void ActivateLimiter()
    {
        Initialize();
    }

    private void Initialize()
    {
        if (_isInitialized) return;

        _rb = GetComponent<Rigidbody2D>();
        //_ps = GetComponent<ParticleSystem>();
        _sr = GetComponent<SpriteRenderer>();
        _coll = GetComponent<Collider2D>();
        _isInitialized = true;
    }

    public void SetSpeed(float speedToSet)
    {
        _currentSpeed = speedToSet;
        _rb.velocity = transform.up * _currentSpeed;
    }

    public void DeactivateLimiter()
    {
        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        BugHandler bh;
        if (collision.TryGetComponent<BugHandler>(out bh))
        {
            LimiterController.Instance.HandleBugAbsorb(bh.transform.position, bh.Corruption);
        }

        HealthHandler hh;
        if (collision.TryGetComponent<HealthHandler>(out hh))
        {
            hh.ApplyDamage(999);
        }


    }
}
