using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class WeaponHandler : MonoBehaviour
{
    protected PlayerHandler _player;

    protected virtual void Start()
    {
        _player = GetComponent<PlayerHandler>();
    }

    public abstract void HandleButtonDown();

    public abstract void HandleButtonUp();
}
