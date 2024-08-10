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


    public void AttachUIToAvatar(PlayerHandler player)
    {
        _player = player;
        transform.SetParent(_player.transform, false);
        _player.CurrentNodeChanged += HandleCurrentNodeChanged;
        var pdh = player.GetComponent<PlayerDataHolder>();
        pdh.CurrentEnergyChanged += HandleEnergyChanged;
        pdh.CurrentSoulChanged += HandleSoulChanged;
    }

    private void HandleCurrentNodeChanged(NodeHandler newNode)
    {
        transform.position = newNode.transform.position;
    }

    private void HandleEnergyChanged(float newEnergyFactor)
    {
        _energySliderHandler.SetFactor(newEnergyFactor);
    }

    private void HandleSoulChanged(float newSoulFactor)
    {
        _soulSliderHandler.SetFactor(newSoulFactor);
    }

    private void Update()
    {
        transform.rotation = _player.CurrentNode.transform.rotation;
    }
}
