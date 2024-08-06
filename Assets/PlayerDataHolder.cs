using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PlayerDataHolder : MonoBehaviour
{
    public Action<float> CurrentEnergyChanged;
    
    //Settings
    [SerializeField] float _startingSpeed = 3;
    [SerializeField] float _maxEnergy = 1;


    //state
    public bool IsAlive => _isAlive;
    bool _isAlive = true;

    public float CurrentSpeed => _currentSpeed;
    private float _currentSpeed;

    public float CurrentEnergy => _currentEnergy;
    private float _currentEnergy;


    public float EnergyRegen => _energyRegen;
    [Tooltip("Energy gained per second")]
    [SerializeField] private float _energyRegen = 0.2f;

    // Start is called before the first frame update
    void Start()
    {
        _currentSpeed = _startingSpeed;
        _currentEnergy = _maxEnergy;
    }

    public void ModifyCurrentEnergy(float energyToAdd)
    {
        _currentEnergy += energyToAdd;
        _currentEnergy = Mathf.Clamp(_currentEnergy, 0, _maxEnergy);
        CurrentEnergyChanged?.Invoke(_currentEnergy);
    }

}
