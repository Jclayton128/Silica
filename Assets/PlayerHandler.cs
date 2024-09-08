using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PlayerHandler : MonoBehaviour
{
    public Action<NodeHandler> CurrentNodeChanged;
    public Action PlayerDying;


    //references
    [SerializeField] WeaponHandler _blaster;
    [SerializeField] WeaponHandler _shotgun;


    //state
    PlayerDataHolder _pdh;
    PlayerEnergyHandler _peh;
    [SerializeField] private NodeHandler _currentNode;
    public NodeHandler CurrentNode => _currentNode;
    [SerializeField] int _ownerIndex = 0;
    public int OwnerIndex => _ownerIndex;
    Vector2 _facingDirForActiveNode;
    PacketHandler packet;

    private void Start()
    {
        InputController.Instance.MouseChanged_LMB = HandlePrimaryClick;
        InputController.Instance.MouseChanged_RMB = HandleSecondaryClick;
        _ownerIndex = PlayerController.Instance.RegisterNewPlayer(this);
        _ownerIndex = 1;

        NodeController.Instance.SpawnStartingNode(_ownerIndex);
        _peh = GetComponent<PlayerEnergyHandler>();
        _pdh = GetComponent<PlayerDataHolder>();
    }

    #region Flow
    private void Update()
    {
        if (_currentNode != null)
        {
            UpdateActiveNodeFacing();
        }
    }

    private void UpdateActiveNodeFacing()
    {
        _facingDirForActiveNode = InputController.Instance.MousePosition - _currentNode.transform.position;
        _currentNode.AdjustRotation(_facingDirForActiveNode);
    }

    private void HandlePrimaryClick(bool wasPushedDown)
    {
        switch (_currentNode.NodeType)
        {
            case NodeHandler.NodeTypes.Empty:
                //do nothing
                return;

            case NodeHandler.NodeTypes.Speed:
                if (wasPushedDown) _blaster.HandleButtonDown();
                else _blaster.HandleButtonUp();
                return;

            case NodeHandler.NodeTypes.Might:
                if (wasPushedDown) _shotgun.HandleButtonDown();
                else _shotgun.HandleButtonUp();
                return;


        }
        
    }

    private void HandleSecondaryClick(bool wasPushedDown)
    {
        if (!wasPushedDown)
        {
            if (!_peh.CheckSoul()) return;
            else _peh.SpendSoul();

            //release a packet
            Vector2 location = _currentNode.transform.position;

            if (packet) packet.DeactivatePacket();
            else
            {
                packet = Instantiate(PacketLibrary.Instance.GetPacketPrefab());
            }


            packet.transform.position = _currentNode.transform.position;
            packet.InitializePacket(
                _currentNode.transform.up * _pdh.CurrentSpeed,
                _ownerIndex);
        }
    }

    #endregion

    #region Current Node Management

    public void AdjustCurrentNode(NodeHandler newCurrentNode)
    {
        NodeHandler oldNode = null;
        if (_currentNode) 
        {
            _currentNode.ConvertToUsedNode();
            oldNode = _currentNode;
        } 

        _currentNode = newCurrentNode;
        CurrentNodeChanged?.Invoke(_currentNode);

        _blaster.HandleNodeChange();
        _shotgun.HandleNodeChange();

        //tell the node controller IOT update the current nodes list (for camera and spawning)
        NodeController.Instance.AdjustCurrentNodes(oldNode, newCurrentNode);
    }

    public void HandleCurrentNodeDestruction()
    {
        Debug.Log("game over!");
        _currentNode = null;
        //TODO handle destruction of player's current node from game mechanics, not graphics
    }

    #endregion

    public void ForcePlayerDeath()
    {
        _currentNode.DeactivateNode();
        PlayerController.Instance.DeregisterPlayer(this);
        PlayerDying?.Invoke();
        Destroy(gameObject);
    }

}
