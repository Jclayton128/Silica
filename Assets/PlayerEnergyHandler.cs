using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerEnergyHandler : MonoBehaviour
{
    PlayerDataHolder _pdh;
    private void Start()
    {
        _pdh = GetComponent<PlayerDataHolder>();
    }

    private void Update()
    {
        if (!_pdh.IsAlive) return;

        _pdh.ModifyCurrentEnergy(_pdh.EnergyRegen * Time.deltaTime);
        _pdh.ModifyCurrentSoul(_pdh.SoulRegen * Time.deltaTime);
    }

    public bool CheckEnergy(float energyCost)
    {
        if (_pdh.CurrentEnergy >= energyCost)
        {
            return true;
        }
        else return false;
    }

    public void SpendEnergy(float energyCost)
    {
        _pdh.ModifyCurrentEnergy(-energyCost);
    }

    public bool CheckSoul()
    {
        if (_pdh.CurrentSoul >= _pdh.CurrentSoulCost)
        {
            return true;
        }
        else return false;
    }

    public void SpendSoul()
    {
        _pdh.ModifyCurrentSoul(-_pdh.CurrentSoulCost);
    }
}
