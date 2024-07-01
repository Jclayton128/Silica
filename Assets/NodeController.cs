using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class NodeController : MonoBehaviour
{
    public static NodeController Instance { get; private set; }

    public Action CurrentNodeUpdated;

    public NodeHandler CurrentNode;// { get; private set; } = null;

    //settings
    [SerializeField] float _xSpan = 4;
    [SerializeField] float _yMin = 2;
    [SerializeField] float _yMax = 5;
    [SerializeField] float _yStarting = -4f;
    [SerializeField] int _densityMin = 1;
    [SerializeField] int _densityMax = 5;


    //state
    Vector2 _facingDirForActiveNode;
    Vector2 _pos;
    [SerializeField] float _nodeDensity = 0.5f;


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

    }

    private void HandleSecondaryClick(bool wasPushedDown)
    {
        if (!wasPushedDown)
        {
            //release a packet
            Vector2 location = CurrentNode.transform.position;
            PacketHandler packet = Instantiate(PacketLibrary.Instance.GetPacketPrefab());
            packet.transform.position = CurrentNode.transform.position;
            packet.InitializePacket(CurrentNode.transform.up * GameController.Instance.Player.CurrentSpeed);
        }
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

        _pos = Vector2.zero;
        if (nodeState == NodeHandler.NodeStates.Available)
        {
            //Should check if proposed point is too close to existing nodes
            _pos.y = CurrentNode.transform.position.y + UnityEngine.Random.Range(_yMin, _yMax);
            _pos.x += UnityEngine.Random.Range(-_xSpan, _xSpan);
            _availableNodes.Add(newNode);
        }
        else if (nodeState == NodeHandler.NodeStates.Current)
        {
            CurrentNode = newNode;
            CurrentNodeUpdated?.Invoke();
        }

        _activatedNodes.Add(newNode);

        newNode.transform.position = _pos;
        newNode.ActivateNode(nodeState);
    }
    
    public void DespawnNode(NodeHandler unneededNode)
    {
        if (CurrentNode == unneededNode) CurrentNode = null;
        _availableNodes.Remove(unneededNode);
        _activatedNodes.Remove(unneededNode);
        _deactivatedNodes.Enqueue(unneededNode);
    }
    #endregion

    #region Node Dynamics

    internal bool CheckIfNodeIsAvailable(NodeHandler nodeHandler)
    {
        if (_availableNodes.Contains(nodeHandler) &&
            nodeHandler.transform.position.y > CurrentNode.transform.position.y)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public void AdjustCurrentNode(NodeHandler newCurrentNode)
    {
        CurrentNode.ConvertToUsedNode();

        CurrentNode = newCurrentNode;
        CurrentNode.ConvertToCurrentNode();

        CurrentNodeUpdated?.Invoke();

        SpawnHigherNodes();

        DespawnLowerNodes();
    }

    private void SpawnHigherNodes()
    {
        //create new nodes
        int spawnCount = Mathf.RoundToInt(Mathf.Lerp(_densityMin, _densityMax, _nodeDensity));
        for (int i = 0; i < spawnCount; i++)
        {
            SpawnNode(NodeHandler.NodeStates.Available);
        }

    }

    private void DespawnLowerNodes()
    {
        //cull nodes off bottom of screen
        List<NodeHandler> nodesToCull = new List<NodeHandler>();
        foreach (var node in _activatedNodes)
        {
            if (node.transform.position.y < CurrentNode.transform.position.y + (2 * _yStarting))
            {
                nodesToCull.Add(node);
            }
        }

        for (int i = nodesToCull.Count-1; i > 0; i--)
        {
            nodesToCull[i].DeactivateNode();
        }
    }

    public void RemoveNodeFromAvailableNodeList(NodeHandler usedNode)
    {
        _availableNodes.Remove(usedNode);
    }


    #endregion


}
