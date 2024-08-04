using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolHandler : MonoBehaviour
{
    public enum PoolTypes {Bullet, Pellet, Missile}

    //references
    Rigidbody2D _rb;
    SpriteRenderer _sr;
    Collider2D _coll;
    ParticleSystem _ps;
    ParticleSystem.MainModule _psm;

    //state
    [SerializeField] PoolTypes _poolType;
    public PoolTypes PoolType => _poolType;
    bool _isInitialized = false;
    bool _isDeactivating = false;
    float _lifetimeRemaining;


    public void ActivatePoolObject(Vector2 velocity, float lifetime, int ownerIndex)
    {
        if (!_isInitialized)
        {
            _rb = GetComponent<Rigidbody2D>();
            _sr = GetComponent<SpriteRenderer>();
            _coll = GetComponent<Collider2D>();
            _ps = GetComponent<ParticleSystem>();
            _psm = _ps.main;
            _isInitialized = true;
        }

        _rb.simulated = true;
        _sr.enabled = true;
        _coll.enabled = true;
        _ps.Play();

        _sr.color = ColorLibrary.Instance.PlayerColors[ownerIndex - 1];
        _psm.startColor = _sr.color;
        transform.up = velocity.normalized;
        _rb.velocity = velocity;
        _lifetimeRemaining = lifetime;
        _isDeactivating = false;
    }

    public void ForceVelocityToLocalUp()
    {
        float mag = _rb.velocity.magnitude;
        _rb.velocity = transform.up * mag;
    }

    public void DeactivatePoolObject()
    {
        if (_isDeactivating) return;

        _rb.simulated = false;
        _sr.enabled = false;
        _coll.enabled = false;
        _ps.Stop();

        PoolController.Instance.CheckinDeactivatedPoolObject(PoolType, this);

        _isDeactivating = true;
    }

    private void Update()
    {
        if (_isDeactivating) return;

        _lifetimeRemaining -= Time.deltaTime;
        if (_lifetimeRemaining < 0)
        {
            DeactivatePoolObject();
        }
    }
}
