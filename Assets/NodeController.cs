using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NodeController : MonoBehaviour
{
    public static NodeController Instance { get; private set; }

    public NodeHandler CurrentNode;// { get; private set; } = null;

    //settings
    [SerializeField] float _xSpan = 4;
    [SerializeField] float _yMin = 2;
    [SerializeField] float _yMax = 4;
    [SerializeField] Vector2 _startingPos = new Vector2(0, -4);

    //state
    Vector2 _facingDirForActiveNode;
    Vector2 _pos;

    Queue<NodeHandler> _deactivatedNodes = new Queue<NodeHandler>();
    List<NodeHandler> _activatedNodes = new List<NodeHandler>();

    List<NodeHandler> _availableNodes = new List<NodeHandler>();

    private void Awake()
    {
        Instance = this;
    }
    private void Start()
    {
        InputController.Instance.MouseChanged_LMB = HandlePrimaryClick;
        InputController.Instance.MouseChanged_RMB = HandleSecondaryClick;
    }

    #region Flow
    private void Update()
    {
        if (CurrentNode != null)
        {
            UpdateActiveNodeFacing();
        }
    }

    private void UpdateActiveNodeFacing()
    {
        _facingDirForActiveNode = InputController.Instance.MousePosition - CurrentNode.transform.position;
        CurrentNode.AdjustRotation(_facingDirForActiveNode);
    }

    private void HandlePrimaryClick(bool wasPushedDown)
    {
        if (!wasPushedDown)
        {
            //release a packet
            Debug.Log("pew");
            Vector2 location = CurrentNode.transform.position;
            PacketHandler packet = Instantiate(PacketLibrary.Instance.GetPacketPrefab());
            packet.transform.position = CurrentNode.transform.position;
            packet.InitializePacket(CurrentNode.transform.up * GameController.Instance.Player.CurrentSpeed);
        }
    }

    private void HandleSecondaryClick(bool wasPushedDown)
    {

    }

    #endregion

    #region Node Creation

    public void SpawnNode(NodeHandler.NodeStates nodeState)
    {
        NodeHandler newNode;
        if (_deactivatedNodes.Count == 0)
        {
            newNode = Instantiate(NodeLibrary.Instance.GetNodePrefab());

        }
        else
        {
            newNode = _deactivatedNodes.Dequeue();
        }

        
        if (nodeState == NodeHandler.NodeStates.Available)
        {
            _pos = Vector2.zero;
            _pos.y = UnityEngine.Random.Range(_yMin, _yMax);
            _pos.x = UnityEngine.Random.Range(-_xSpan, _xSpan);
            _availableNodes.Add(newNode);
        }
        else if (nodeState == NodeHandler.NodeStates.Current)
        {
            _pos = _startingPos;
            CurrentNode = newNode;
        }

        newNode.transform.position = _pos;
        newNode.ActivateNode(nodeState);
    }
    
    public void DespawnNode(NodeHandler unneededNode)
    {
        if (CurrentNode == unneededNode) CurrentNode = null;
        _activatedNodes.Remove(unneededNode);
        _deactivatedNodes.Enqueue(unneededNode);
    }
    #endregion

    #region Node Dynamics

    internal bool CheckIfNodeIsAvailable(NodeHandler nodeHandler)
    {
        if (_availableNodes.Contains(nodeHandler)) return true;
        else return false;
    }

    public void AdjustCurrentNode(NodeHandler newCurrentNode)
    {
        CurrentNode.ConvertToUsedNode();

        CurrentNode = newCurrentNode;
        CurrentNode.ConvertToCurrentNode();
    }

    public void RemoveNodeFromAvailableNodeList(NodeHandler usedNode)
    {
        _availableNodes.Remove(usedNode);
    }


    #endregion


}
