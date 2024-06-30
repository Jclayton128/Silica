using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NodeController : MonoBehaviour
{
    public static NodeController Instance { get; private set; }

    public NodeHandler ActiveNode;// { get; private set; } = null;

    //state
    Vector2 _facingDirForActiveNode;

    private void Awake()
    {
        Instance = this;
    }
    private void Start()
    {
        InputController.Instance.MouseChanged_LMB = HandlePrimaryClick;
        InputController.Instance.MouseChanged_RMB = HandleSecondaryClick;
    }

    public void CreateStartingNode()
    {
        NodeHandler newHomeNode = Instantiate(NodeLibrary.Instance.GetHomeNodePrefab());
        ActiveNode = newHomeNode;
    }

    private void Update()
    {
        if (ActiveNode != null)
        {
            UpdateActiveNodeFacing();
        }
    }

    private void UpdateActiveNodeFacing()
    {
        _facingDirForActiveNode = InputController.Instance.MousePosition - ActiveNode.transform.position;
        ActiveNode.AdjustRotation(_facingDirForActiveNode);
    }

    private void HandlePrimaryClick(bool wasPushedDown)
    {
        if (!wasPushedDown)
        {
            //release a packet
            Debug.Log("pew");
            Vector2 location = ActiveNode.transform.position;
            PacketHandler packet = Instantiate(PacketLibrary.Instance.GetPacketPrefab());
            packet.InitializePacket(ActiveNode.transform.up * GameController.Instance.Player.CurrentSpeed);
        }
    }

    private void HandleSecondaryClick(bool wasPushedDown)
    {

    }

}
