using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthHandler : MonoBehaviour
{
    IDestroyable _host;
    [SerializeField] float _healthMax = 10;

    //state
    [SerializeField] float _currentHealth;
    public float CurrentHealth => _currentHealth;
    NodeHandler _nodeHandler;

    private void Awake()
    {
        _host = GetComponent<IDestroyable>();
        if (_host == null) Debug.LogWarning("Object is has Health but isn't Damageable");
        Reset();
    }

    public void ApplyDamage(float damageToInflict)
    {
        _currentHealth -= damageToInflict;
        if (_currentHealth <= 0) _host.HandleZeroHealth();
        else
        {
            _host.HandleHealthDrop(_currentHealth/_healthMax);
        }
        _currentHealth = Mathf.Clamp(_currentHealth, 0, _healthMax);
    }

    public void Reset()
    {
        _currentHealth = _healthMax;
    }
}
