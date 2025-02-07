using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ArenaHandler : MonoBehaviour
{
    public Action ArenaShuttingDown;

    //scene refs
    [SerializeField] ParticleSystem _edgePS = null;

    //settings
    [SerializeField] float _radius = 12;
    [SerializeField] Color _startColor = Color.white;

    [Tooltip("Factor of how far along radius from centroid can a node be")]
    float _maxEdgeFactor = 0.9f;

    //state
    ParticleSystem.MainModule _edgePSmain;
    ParticleSystem.ShapeModule _edgePSshape;
    [SerializeField] List<NodeHandler> _allNodes;
    public List<NodeHandler> AllNodes => _allNodes;
    [SerializeField] List<NodeHandler> _starts = new List<NodeHandler>();
    [SerializeField] List<NodeHandler> _availableNodes = new List<NodeHandler>();


    public float ArenaFunctionalRadius => _radius * _maxEdgeFactor;



    public void SetupArena()
    {
        if (_edgePS)
        {
            _edgePSmain = _edgePS.main;
            _edgePSshape = _edgePS.shape;
        }

        _edgePSmain.startColor = _startColor;
        _edgePSshape.radius = _radius;
        _edgePS.Play(true);

        _allNodes = GatherNodes();
        ActivateStartingNode();
    }

    public List<NodeHandler> GatherNodes()
    {
        List<NodeHandler> mainframes = new List<NodeHandler>();


        foreach (var node in gameObject.GetComponentsInChildren<NodeHandler>())
        {
            _allNodes.Add(node);
            _availableNodes.Add(node);
            if (node.NodeType == NodeHandler.NodeTypes.Mainframe)
            {
                mainframes.Add(node);
            }
            if (node.NodeType == NodeHandler.NodeTypes.Starting)
            {
                _starts.Add(node);
            }
        }
        //Debug.Log($"Found {_allNodes.Count} nodes");
        if (mainframes.Count == 0) Debug.LogWarning($"Hey, I found no mainframes to exit!");
        if (_starts.Count == 0) Debug.LogWarning($"Hey, I found no starting nodes");

        return _allNodes;
    }

    public void CloseArena()
    {
        //TODO maybe have some kind of cinematic zoom in on the conquered node?
        ArenaShuttingDown?.Invoke();
        Destroy(gameObject);
        _edgePS.Stop();
    }

    public void ActivateStartingNode()
    {
        _starts[0].ConvertToCurrentNode();
    }

    public void RemoveAvailableNode(NodeHandler node)
    {
        _availableNodes.Remove(node);
    }

    public void AddAvailableNode(NodeHandler node)
    {
        _availableNodes.Add(node);
    }

    public bool CheckIsIfNodeIsAvailable(NodeHandler node)
    {
        return _availableNodes.Contains(node);
    }
}
