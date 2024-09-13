using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AvatarUIDriver : MonoBehaviour
{
    [SerializeField] AdjustableImageBar _energySliderHandler = null;
    [SerializeField] AdjustableImageBar _soulSliderHandler = null;

    PlayerHandler _player;
    PlayerDataHolder _pdh;

    public void AttachUIToAvatar(PlayerHandler player)
    {
        _player = player;
        transform.SetParent(_player.transform, false);
        _player.PlayerTransformChanged += HandlePlayerLocationChanged;
        _player.PlayerDying += HandlePlayerDeath;
        _pdh = player.GetComponent<PlayerDataHolder>();
        _pdh.CurrentEnergyChanged += HandleEnergyChanged;
        _pdh.CurrentSoulChanged += HandleSoulChanged;

    }

    private void HandlePlayerLocationChanged(Transform newTransform)
    {
        if (_player.CurrentNode)
        {
            transform.position = newTransform.position;
        }
        else
        {
            //remain hidden somewhere; UI not needed on server map
        }

    }

    private void HandleEnergyChanged(float newEnergyFactor)
    {
        _energySliderHandler.SetFactor(newEnergyFactor);
    }

    private void HandleSoulChanged(float newSoulFactor)
    {
        _soulSliderHandler.SetFactor(newSoulFactor);
    }

    private void HandlePlayerDeath()
    {
        _player.PlayerTransformChanged -= HandlePlayerLocationChanged;
        _player.PlayerDying -= HandlePlayerDeath;
        _pdh.CurrentEnergyChanged -= HandleEnergyChanged;
        _pdh.CurrentSoulChanged -= HandleSoulChanged;
        Destroy(this.gameObject);
    }

    private void Update()
    {
        if (_player.CurrentNode)
        {
            transform.rotation = _player.CurrentNode.transform.rotation;
        }
        else if (_player.CurrentServer)
        {
            transform.rotation = _player.CurrentServer.transform.rotation;
        }
        
    }
}
