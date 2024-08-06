using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class WeaponHandler : MonoBehaviour
{
    protected PlayerHandler _player;
    protected PlayerEnergyHandler _playerEnergyHandler;

    protected virtual void Start()
    {
        _player = GetComponent<PlayerHandler>();
        _playerEnergyHandler = GetComponent<PlayerEnergyHandler>();

    }

    public abstract void HandleButtonDown();

    public abstract void HandleButtonUp();

    public abstract void HandleNodeChange();
}
