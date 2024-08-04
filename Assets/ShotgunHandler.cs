using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShotgunHandler : WeaponHandler
{

    [SerializeField] PoolHandler.PoolTypes _projectileType = PoolHandler.PoolTypes.Pellet;


    [SerializeField] float _degreeSpread = 30f;

    [SerializeField] float _chargeRate = 3.3f; // units per second;
    [SerializeField] float _maxCharge = 5f;
    [SerializeField] float _minChargeToFire = 2f;

    [SerializeField] float _shotSpeed = 4;
    [SerializeField] float _lifetime = 3;


    //state
    bool _isCharging = false;
     [SerializeField] float _chargeLevel;

    public override void HandleButtonDown()
    {
        _isCharging = true;
    }

    public override void HandleButtonUp()
    {
        ActivateInternal();
        _isCharging = false;
        _chargeLevel = 0;
    }

    public override void HandleNodeChange()
    {
        _isCharging = false;
        _chargeLevel = 0;
    }

    private void Update()
    {
        if (_isCharging)
        {
            _chargeLevel += _chargeRate * Time.deltaTime;
            _chargeLevel = Mathf.Clamp(_chargeLevel, 0, _maxCharge);
        }
    }

    protected void ActivateInternal()
    {
        if (_chargeLevel >= _minChargeToFire) 
        {
            Fire();
        }
    }

    private void Fire()
    {
        int amount = Mathf.RoundToInt(_chargeLevel);
        Debug.Log($"firing {amount} with charge of {_chargeLevel}");
        float spreadSubdivided = _degreeSpread / amount;
        for (int i = 0; i < amount; i++)
        {
            Quaternion sector = Quaternion.Euler(0, 0,
                (i * spreadSubdivided) - (_degreeSpread / 2f) + _player.CurrentNode.transform.eulerAngles.z);

            PoolHandler ph = PoolController.Instance.CheckoutPoolObject(_projectileType);

            ph.ActivatePoolObject(_player.CurrentNode.transform.up * _shotSpeed,
                _lifetime,
                _player.OwnerIndex);
            ph.transform.position = _player.CurrentNode.transform.position;
            ph.transform.rotation = sector;

            ph.ForceVelocityToLocalUp();

        }

        _chargeLevel = 0;
    }

}
