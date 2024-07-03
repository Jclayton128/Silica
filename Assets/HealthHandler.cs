using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthHandler : MonoBehaviour
{
    IDamageable _host;
    [SerializeField] float _healthStarting = 1;
    [SerializeField] float _healthMax = 10;

    //state
    float _currentHealth;
    public float CurrentHealth => _currentHealth;

    private void Awake()
    {
        _host = GetComponent<IDamageable>();
        if (_host == null) Debug.LogWarning("Object is has Health but isn't Damageable");
    }

    public void ApplyDamage(float damageToInflict)
    {
        _currentHealth -= damageToInflict;
        if (_currentHealth <= 0) _host.HandleZeroHealth();
        _currentHealth = Mathf.Clamp(_currentHealth, 0, _healthMax);
    }

    public void Reset()
    {
        _currentHealth = _healthStarting;
    }
}
