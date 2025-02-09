using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PlayerDataHolder : MonoBehaviour
{
    public Action<float> CurrentEnergyChanged;
    public Action<float> CurrentSoulChanged;
    public Action StatsChanged;

    //Settings
    [Header("Start Settings")]
    [SerializeField] float _startingSpeed = 3;
    [SerializeField] float _startingRange = 3;
    [SerializeField] float _maxEnergy = 1;
    [SerializeField] float _maxSoul = 1;
    [SerializeField] float _startingSoulCost = 1;


    //state

    public bool IsAlive => _isAlive;

    bool _isAlive = true;

    public float CurrentSpeed => _currentSpeed;
    [Header("Current Params")]
    [SerializeField] private float _currentSpeed;

    [SerializeField] private float _currentRange;

    public float CurrentEnergy => _currentEnergy;
    [SerializeField] private float _currentEnergy;

    public float CurrentSoul => _currentSoul;
    [SerializeField] private float _currentSoul;


    public float EnergyRegen => _energyRegen;
    [Tooltip("Energy gained per second")]
    [SerializeField] private float _energyRegen = 0.2f;

    public float SoulRegen => _soulRegen;
    [Tooltip("Soul gained per second")]
    [SerializeField] private float _soulRegen = 0.2f;

    public float CurrentSoulCost => _currentSoulCost;
    private float _currentSoulCost = 1;

    public float PacketLifetime => _currentRange/_currentSpeed;
    //public float ServersPacketLifetime => _currentRange / _currentSpeed;

    [Header("Base Stats")]
    [SerializeField] int _speed_Base = 0;
    [SerializeField] int _might_Base = 0;
    [SerializeField] int _intelligence_Base = 0;
    [SerializeField] int _constitution_Base = 0;
    [SerializeField] int _wisdom_Base = 0;

    [Header("Temporary Stats")]
    [SerializeField] int _speed_Mod = 0;
    [SerializeField] int _might_Mod = 0;
    [SerializeField] int _intelligence_Mod = 0;
    [SerializeField] int _constitution_Mod = 0;
    [SerializeField] int _wisdom_Mod = 0;

    public int Stat_Speed => _speed_Base + _speed_Mod;
    public int Stat_Might => _might_Base + _might_Mod;
    public int Stat_Intelligence => _intelligence_Base + _intelligence_Mod;
    public int Stat_Constitution => _constitution_Base + _constitution_Mod;
    public int Stat_Wisdom => _wisdom_Base + _wisdom_Mod;


    // Start is called before the first frame update
    void Start()
    {
        _currentSpeed = _startingSpeed;
        _currentRange = _startingRange;
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

    public void ModifySpeed(int amountToAdd)
    {
        _speed_Mod += amountToAdd;
        StatsChanged?.Invoke();
    }

    public void ModifyMight(int amountToAdd)
    {
        _might_Mod += amountToAdd;
        StatsChanged?.Invoke();
    }

    public void ModifyIntelligence(int amountToAdd)
    {
        _intelligence_Mod += amountToAdd;
        StatsChanged?.Invoke();
    }

    public void ModifyConstitution(int amountToAdd)
    {
        _constitution_Mod += amountToAdd;
        StatsChanged?.Invoke();
    }

    public void ModifyWisdom(int amountToAdd)
    {
        _wisdom_Mod += amountToAdd;
        StatsChanged?.Invoke();
    }
}
