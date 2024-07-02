using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHandler : MonoBehaviour
{

    //references
    [SerializeField] WeaponHandler _blaster;
    [SerializeField] WeaponHandler _shotgun;


    //state
    [SerializeField] private NodeHandler _currentNode;
    public NodeHandler CurrentNode => _currentNode;
    [SerializeField] int _ownerIndex = 0;
    public int OwnerIndex => _ownerIndex;
    Vector2 _facingDirForActiveNode;


    private void Start()
    {
        InputController.Instance.MouseChanged_LMB = HandlePrimaryClick;
        InputController.Instance.MouseChanged_RMB = HandleSecondaryClick;
        _ownerIndex = PlayerController.Instance.RegisterNewPlayer(this);
        NodeController.Instance.SpawnStartingNode(_ownerIndex);
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

            case NodeHandler.NodeTypes.Blaster:
                if (wasPushedDown) _blaster.HandleButtonDown();
                else _blaster.HandleButtonUp();
                return;

            case NodeHandler.NodeTypes.Shotgun:
                if (wasPushedDown) _shotgun.HandleButtonDown();
                else _shotgun.HandleButtonUp();
                return;


        }
        
    }

    private void HandleSecondaryClick(bool wasPushedDown)
    {
        if (!wasPushedDown)
        {
            //release a packet
            Vector2 location = _currentNode.transform.position;
            PacketHandler packet = Instantiate(PacketLibrary.Instance.GetPacketPrefab());
            packet.transform.position = _currentNode.transform.position;
            packet.InitializePacket(
                _currentNode.transform.up * GameController.Instance.Player.CurrentSpeed,
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

        //tell the node controller IOT update the current nodes list (for camera and spawning)
        NodeController.Instance.AdjustCurrentNodes(oldNode, newCurrentNode);
    }

    #endregion
}
