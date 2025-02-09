using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlasterHandler : WeaponHandler
{


    //param
    [SerializeField] float _timeBetweenShot = 0.5f;
    [SerializeField] PoolHandler.PoolTypes _projectileType = PoolHandler.PoolTypes.Bullet;
    [SerializeField] float _shotSpeed = 4;
    [SerializeField] float _lifetime = 3;
    [SerializeField] float _energyCostPerShot = 0.2f;

    //state
    [SerializeField] float _timeUntilNextShot = 0;
    [SerializeField] bool _isFiring = false;


    public override void HandleButtonDown()
    {
        _isFiring = true;
    }

    public override void HandleButtonUp()
    {
        _isFiring = false;
        //_timeUntilNextShot = _timeBetweenShot;
    }

    public override void HandleNodeChangedToUsed()
    {
        _isFiring = false;

        PlayerController.Instance.CurrentPlayer.GetComponent<PlayerDataHolder>().ModifySpeed(1);
    }

    private void Update()
    {
        if (_isFiring)
        {
            _timeUntilNextShot -= Time.deltaTime;
            if (_timeUntilNextShot < 0 && _playerEnergyHandler.CheckEnergy(_energyCostPerShot))
            {
                EmitShot();
                _playerEnergyHandler.SpendEnergy(_energyCostPerShot);
                _timeUntilNextShot = _timeBetweenShot;
            }
        }
    }

    private void EmitShot()
    {
        PoolHandler ph = PoolController.Instance.CheckoutPoolObject(_projectileType);

        ph.ActivatePoolObject(_player.CurrentNode.transform.up * _shotSpeed,
            _lifetime,
            _player.OwnerIndex);
        ph.transform.position = _player.CurrentNode.transform.position;
    }
}
