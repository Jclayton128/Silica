using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class NodeController : MonoBehaviour
{
    public static NodeController Instance { get; private set; }

    public Action CurrentNodesUpdated;



    //settings
    [SerializeField] float[] _xStarts = { -3f, -1f, 1f, 3f };
    public float XSpan => _xSpan;
    [SerializeField] float _xSpan = 4;
    [SerializeField] float _yMin = 2;
    [SerializeField] float _yMax = 5;
    [SerializeField] float _yOffscreenOffset = 10;
    public float YOffScreen => _yOffscreenOffset + CurrentNodesCentroid;
    [SerializeField] float _yStarting = -4f;
    [SerializeField] int _densityMin = 1;
    [SerializeField] int _densityMax = 5;


    //state

    Vector2 _pos;
    [SerializeField] float _nodeDensity = 0.5f;

    [SerializeField] List<NodeHandler> _currentNodes = new List<NodeHandler>();// { get; private set; } = null;
    public float CurrentNodesCentroid => FindCurrentNodesYCentroid();
    Queue<NodeHandler> _deactivatedNodes = new Queue<NodeHandler>();
    List<NodeHandler> _activatedNodes = new List<NodeHandler>();

    List<NodeHandler> _availableNodes = new List<NodeHandler>();

    private void Awake()
    {
        Instance = this;
    }


    #region Flow
    

    #endregion

    #region Node Creation

    public void SpawnStartingNode(int ownerIndex)
    {
        NodeHandler newNode;
        newNode = Instantiate(NodeLibrary.Instance.GetNodePrefab());

        _pos = Vector2.zero;
        _pos.x = _xStarts[ownerIndex - 1];
        newNode.transform.position = _pos;

        newNode.ActivateNode(NodeHandler.NodeStates.Current,
            NodeHandler.NodeTypes.Empty, //The node type should not be fixed
            ownerIndex);

        _activatedNodes.Add(newNode);
        //_currentNodes.Add(newNode);
        CurrentNodesUpdated?.Invoke();

    }
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
            _pos.y = CurrentNodesCentroid + UnityEngine.Random.Range(_yMin, _yMax);
            _pos.x += UnityEngine.Random.Range(-_xSpan, _xSpan);
            _availableNodes.Add(newNode);
        }

        _activatedNodes.Add(newNode);

        newNode.transform.position = _pos;
        newNode.ActivateNode(nodeState,
            NodeHandler.NodeTypes.Blaster, //The node type should not be fixed
            0);
    }
    
    public void DespawnNode(NodeHandler unneededNode)
    {
        _currentNodes.Remove(unneededNode);
        _availableNodes.Remove(unneededNode);
        _activatedNodes.Remove(unneededNode);
        _deactivatedNodes.Enqueue(unneededNode);
    }
    #endregion

    #region Node Dynamics

    internal bool CheckIfNodeIsAvailable(NodeHandler nodeHandler)
    {
        if (_availableNodes.Contains(nodeHandler) &&
            nodeHandler.transform.position.y > CurrentNodesCentroid)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public void AdjustCurrentNodes(NodeHandler oldCurrentNode, NodeHandler newCurrentNode)
    {
        RemoveNodeFromAvailableNodeList(newCurrentNode);
        
        _currentNodes.Remove(oldCurrentNode);
        _currentNodes.Add(newCurrentNode);

        CurrentNodesUpdated?.Invoke();

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
            if (node.transform.position.y < CurrentNodesCentroid + (2 * _yStarting))
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

    #region Helpers

    private float FindCurrentNodesYCentroid()
    {
        float centroid = 0;

        foreach (var node in _currentNodes)
        {
            centroid += node.transform.position.y / (float)_currentNodes.Count;
        }

        //Debug.Log("centroid: " + centroid);
        return centroid;

    }

    #endregion


}
