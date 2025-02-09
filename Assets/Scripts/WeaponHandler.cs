using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class WeaponHandler : MonoBehaviour
{
    protected PlayerHandler _player;
    protected PlayerEnergyHandler _playerEnergyHandler;

    protected virtual void Start()
    {
        _player = PlayerController.Instance.CurrentPlayer;
        _playerEnergyHandler = _player.GetComponent<PlayerEnergyHandler>();

    }

    public abstract void HandleButtonDown();

    public abstract void HandleButtonUp();

    public abstract void HandleNodeChangedToUsed();
}
