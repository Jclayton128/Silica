using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PlayerDataHolder : MonoBehaviour
{
    public Action<float> CurrentEnergyChanged;
    public Action<float> CurrentSoulChanged;
    
    //Settings
    [SerializeField] float _startingSpeed = 3;
    [SerializeField] float _maxEnergy = 1;
    [SerializeField] float _maxSoul = 1;
    [SerializeField] float _startingSoulCost = 1;


    //state
    public bool IsAlive => _isAlive;
    bool _isAlive = true;

    public float CurrentSpeed => _currentSpeed;
    private float _currentSpeed;

    public float CurrentEnergy => _currentEnergy;
    private float _currentEnergy;

    public float CurrentSoul => _currentSoul;
    private float _currentSoul;

    private float _packetLifetime;
    public float PacketLifetime => _packetLifetime;

    public float EnergyRegen => _energyRegen;
    [Tooltip("Energy gained per second")]
    [SerializeField] private float _energyRegen = 0.2f;

    public float SoulRegen => _soulRegen;
    [Tooltip("Soul gained per second")]
    [SerializeField] private float _soulRegen = 0.2f;

    public float CurrentSoulCost => _currentSoulCost;
    private float _currentSoulCost = 1;

    // Start is called before the first frame update
    void Start()
    {
        _currentSpeed = _startingSpeed;
        _currentEnergy = _maxEnergy;
        _currentSoul = _maxSoul;
    }

    public void ModifyCurrentEnergy(float energyToAdd)
    {
        _currentEnergy += energyToAdd;
        _currentEnergy = Mathf.Clamp(_currentEnergy, 0, _maxEnergy);
        CurrentEnergyChanged?.Invoke(_currentEnergy/_maxEnergy);
    }

    public void ModifyCurrentSoul(float soulToAdd)
    {
        _currentSoul += soulToAdd;
        _currentSoul = Mathf.Clamp(_currentSoul, 0, _maxSoul);
        CurrentSoulChanged?.Invoke(_currentSoul/_maxSoul);
    }

}
