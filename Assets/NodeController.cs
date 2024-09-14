using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class NodeController : MonoBehaviour
{
    public static NodeController Instance { get; private set; }

    public Action<int> CurrentNodesUpdated;



    //settings
    [SerializeField] float[] _xStarts = { -3f, -1f, 1f, 3f };
    
    [SerializeField] float _yStarting = -4f;
    [SerializeField] int _densityMin = 1;
    [SerializeField] int _densityMax = 5; 
    [SerializeField] float _yOffsetNewNodes = 3.5f;
    [SerializeField] float _ySpan = 2;

    public int CurrentNodesAscended => _currentNodesAscended;
    [SerializeField] int _currentNodesAscended = 0;

    //state
    [SerializeField] Level _currentLevel;
    Vector2 _pos;
    //[SerializeField] float _nodeDensity = 0.5f;

    [SerializeField] List<NodeHandler> _currentNodes = new List<NodeHandler>();// { get; private set; } = null;
    public Vector2 CurrentNodesCentroid => FindCurrentNodesYCentroid();
    [SerializeField] Queue<NodeHandler> _deactivatedNodes = new Queue<NodeHandler>();
    [SerializeField] List<NodeHandler> _activatedNodes = new List<NodeHandler>();

    List<NodeHandler> _availableNodes = new List<NodeHandler>();

    private void Awake()
    {
        Instance = this;
    }


    #region Flow
    

    #endregion

    #region Node Creation

    //public void SpawnStartingNode(int ownerIndex)
    //{
    //    NodeHandler newNode;
    //    newNode = Instantiate(NodeLibrary.Instance.GetNodePrefab());

    //    _pos = ArenaController.Instance.GetNewPlayerPosition();
    //    newNode.transform.position = _pos;

    //    newNode.ActivateNode(NodeHandler.NodeStates.Current,
    //        NodeHandler.NodeTypes.Empty, //The node type should not be fixed
    //        ownerIndex);

    //    _activatedNodes.Add(newNode);
    //    //_currentNodes.Add(newNode);
    //    //CurrentNodesUpdated?.Invoke(_currentNodesAscended);

    //}

    public void SpawnNode(NodeHandler.NodeStates nodeState, NodeHandler.NodeTypes nodeType)
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
            //_pos.y = CurrentNodesCentroid + UnityEngine.Random.Range(_yMin, _yMax);
            //_pos.x += UnityEngine.Random.Range(-_xSpan, _xSpan);
            _pos = GenerateRandomValidNodePosition();

            _availableNodes.Add(newNode);
        }

        _activatedNodes.Add(newNode);

        newNode.transform.position = _pos;
        newNode.ActivateNode(nodeState,
            nodeType,
            0);
    }

    public void SpawnNode(NodeHandler.NodeStates nodeState)
    {
        SpawnNode(nodeState, GenerateRandomNodeType());
    }

    private NodeHandler.NodeTypes GenerateRandomNodeType()
    {
        int rand = UnityEngine.Random.Range(0, _currentLevel.PossibleNodes.Count);
        return _currentLevel.PossibleNodes[rand];
    }

    private Vector2 GenerateRandomValidNodePosition()
    {
        List<Vector3> currentNodePositions = new List<Vector3>();

        foreach (var node in _activatedNodes)
        {
            currentNodePositions.Add(node.transform.position);
        }

        Vector2 pos = CUR.GetRandomPosWithinArenaAwayFromOtherPoints(Vector2.zero,
            PlayerController.Instance.CurrentPlayer.CurrentServer.Arena.ArenaFunctionalRadius,
            currentNodePositions, 1f);


            //0, ArenaController.Instance.XSpan, _yOffsetNewNodes + CurrentNodesCentroid, _ySpan,
            //currentNodePositions, _currentLevel.MinDistanceBetweenNodes);

        return pos;
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
        if (_availableNodes.Contains(nodeHandler))
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

        _currentNodesAscended++;

        Debug.Log($"new current node");
        CurrentNodesUpdated?.Invoke(_currentNodesAscended);

        //SpawnHigherNodes();

        //DespawnLowerNodes();
    }

    private void SpawnHigherNodes()
    {
        //create new nodes
        int spawnCount = Mathf.RoundToInt(Mathf.Lerp(_densityMin, _densityMax,
            _currentLevel.NodeDensity));
        for (int i = 0; i < spawnCount; i++)
        {
            SpawnNode(NodeHandler.NodeStates.Available);
        }

    }

    public void DespawnAllNodes()
    {
        List<NodeHandler> nodesToCull = new List<NodeHandler>();
        foreach (var node in _activatedNodes)
        {
            nodesToCull.Add(node);
        }

        for (int i = nodesToCull.Count - 1; i >= 0; i--)
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



    private Vector3 FindCurrentNodesYCentroid()
    {
        Vector3 centroid = Vector2.zero;

        foreach (var node in _currentNodes)
        {
            centroid += node.transform.position / (float)_currentNodes.Count;
        }

        //Debug.Log("centroid: " + centroid);
        return centroid;

    }

    #endregion


}
