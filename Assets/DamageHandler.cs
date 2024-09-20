using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageHandler : MonoBehaviour
{
    public Action DamageDealt;
    PoolHandler _ph;

    [SerializeField] float _damage = 1;
    [SerializeField] bool _isPiercing = false;


    private void Awake()
    {
        Initialize();
    }
    private void Initialize()
    {
        _ph = GetComponent<PoolHandler>();
    }

    public void SetDamage(float damageToDeal)
    {
        _damage = damageToDeal; 
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        HealthHandler hh;
        if (collision.TryGetComponent<HealthHandler>(out hh))
        {
            hh.ApplyDamage(_damage);
            if (_isPiercing)
            {

            }
            else
            {
                DamageDealt?.Invoke();
                if (_ph) _ph.DeactivatePoolObject();
            }
        }
    }
}
