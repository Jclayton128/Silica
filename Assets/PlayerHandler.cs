using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PlayerHandler : MonoBehaviour
{
    public Action<NodeHandler> CurrentNodeChanged;
    public Action<Transform> PlayerTransformChanged;
    public Action PlayerDying;


    //references
    [SerializeField] WeaponHandler _blaster;
    [SerializeField] WeaponHandler _shotgun;


    //state
    PlayerDataHolder _pdh;
    PlayerEnergyHandler _peh;

    [SerializeField] private NodeHandler _currentNode;
    public NodeHandler CurrentNode => _currentNode;

    ServerHandler _currentServer;
    public ServerHandler CurrentServer => _currentServer;

    public Transform CurrentTransform => CurrentNode ? _currentNode.transform : _currentServer.transform;

    [SerializeField] int _ownerIndex = 0;
    public int OwnerIndex => _ownerIndex;
    Vector2 _facingDir;
    PacketHandler packet;

    private void Start()
    {
        InputController.Instance.MouseChanged_LMB = HandlePrimaryClick;
        InputController.Instance.MouseChanged_RMB = HandleSecondaryClick;
        _ownerIndex = PlayerController.Instance.RegisterNewPlayer(this);
        _ownerIndex = 1;

        _currentServer = ServerController.Instance.StartingServer;
        //NodeController.Instance.SpawnStartingNode(_ownerIndex);
        _peh = GetComponent<PlayerEnergyHandler>();
        _pdh = GetComponent<PlayerDataHolder>();
    }

    #region Flow
    private void Update()
    {

        if (_currentNode != null)
        {            
            UpdateCurrentNodeFacing();
        }

        else if (_currentServer != null)
        {
            UpdateCurrentServerFacing();
        }
    }


    private void UpdateCurrentNodeFacing()
    {
        _facingDir = InputController.Instance.MousePosition - _currentNode.transform.position;
        _currentNode.AdjustRotation(_facingDir);
    }

    private void UpdateCurrentServerFacing()
    {
        _facingDir = InputController.Instance.MousePosition - _currentServer.transform.position;
        _currentServer.AdjustRotation(_facingDir);
    }

    private void HandlePrimaryClick(bool wasPushedDown)
    {
        if (_currentNode)
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
        else
        {
            //Do LMB thing while in server map mode... maybe enter server?
        }

        
    }

    private void HandleSecondaryClick(bool wasPushedDown)
    {
        if (_currentNode)
        {
            if (!wasPushedDown) HandleSecondaryClick_Nodes();
        }
        else
        {
            if (!wasPushedDown) HandleSecondaryClick_Servers();
        }


    }

    private void HandleSecondaryClick_Nodes()
    {
        if (!_peh.CheckSoul()) return;
        else _peh.SpendSoul();

        //release a packet
        Vector2 location = _currentNode.transform.position;

        if (packet) packet.DeactivatePacket();
        else
        {
            packet = Instantiate(PacketLibrary.Instance.GetPacketPrefab());
            packet.InitializePacket(_ownerIndex);
        }

        packet.gameObject.layer = 6;
        packet.ActivatePacket(_currentNode.transform.up * _pdh.CurrentSpeed, _pdh.PacketLifetime);
        packet.transform.position = _currentNode.transform.position;

    }

    private void HandleSecondaryClick_Servers()
    {
        //release a packet
        Vector2 location = _currentServer.transform.position;

        if (packet) packet.DeactivatePacket();
        else
        {
            packet = Instantiate(PacketLibrary.Instance.GetPacketPrefab());
            packet.InitializePacket(_ownerIndex);
        }


        packet.transform.position = _currentServer.transform.position;
        packet.ActivatePacket(_currentServer.transform.up * _pdh.CurrentSpeed, _pdh.PacketLifetime);
        packet.gameObject.layer = 11;
    }

    #endregion

    #region Current Node Management

    public void ClearCurrentNode()
    {
        _currentNode = null;
    }

    public void AdjustCurrentNode(NodeHandler newCurrentNode)
    {
        NodeHandler oldNode = null;
        if (_currentNode) 
        {
            _currentNode.ConvertToUsedNode();
            oldNode = _currentNode;
        } 

        _currentNode = newCurrentNode;
        PlayerTransformChanged?.Invoke(_currentNode.transform);

        _blaster.HandleNodeChange();
        _shotgun.HandleNodeChange();

        //tell the node controller IOT update the current nodes list (for camera and spawning)
        //NodeController.Instance.AdjustCurrentNodes(oldNode, newCurrentNode);
    }

    public void HandleCurrentNodeDestruction()
    {
        Debug.Log("game over!");
        _currentNode = null;
        //TODO handle destruction of player's current node from game mechanics, not graphics
    }

    #endregion

    #region Current Server Management

    public void ReturnToCurrentServer()
    {
        _currentNode = null;
        PlayerTransformChanged?.Invoke(CurrentTransform);
    }

    public void AdjustCurrentServer(ServerHandler newServerHandler)
    {
        ServerHandler oldServer = null;
        if (_currentServer)
        {
            _currentServer.ModifyServerState(ServerHandler.ServerStates.Beaten);
            oldServer = _currentServer;
        }

        _currentServer = newServerHandler;
        _currentServer.ConvertToCurrentServer();

        //Enter the Server...
        ServerController.Instance.EnterServerToArena();

        PlayerTransformChanged?.Invoke(CurrentTransform);
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
