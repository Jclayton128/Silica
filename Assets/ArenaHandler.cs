using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArenaHandler : MonoBehaviour
{
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
    List<NodeHandler> _allNodes;
    List<NodeHandler> starts = new List<NodeHandler>();

    public float ArenaFunctionalRadius => _radius * _maxEdgeFactor;



    private void Start()
    {
        if (_edgePS) 
        {
            _edgePSmain = _edgePS.main;
            _edgePSshape = _edgePS.shape;
        }

        _edgePS.Pause(true);
    }

    public void SetupArena()
    {
        _edgePSmain.startColor = _startColor;
        _edgePSshape.radius = _radius;
        _edgePS.Play(true);

        _allNodes = GatherNodes();
    }

    public List<NodeHandler> GatherNodes()
    {
        List<NodeHandler> nodes = new List<NodeHandler>();

        List<NodeHandler> mainframes = new List<NodeHandler>();


        foreach (var node in gameObject.GetComponentsInChildren<NodeHandler>())
        {
            nodes.Add(node);
            if (node.NodeType == NodeHandler.NodeTypes.Mainframe)
            {
                mainframes.Add(node);
            }
            if (node.NodeType == NodeHandler.NodeTypes.Starting)
            {
                starts.Add(node);
            }
        }
        Debug.Log($"Found {nodes.Count} nodes");
        if (mainframes.Count == 0) Debug.LogWarning($"Hey, I found no mainframes to exit!");
        if (starts.Count == 0) Debug.LogWarning($"Hey, I found no starting nodes");

        return nodes;
    }

    public void CloseArena()
    {
        //TODO maybe have some kind of cinematic zoom in on the conquered node?
        Destroy(gameObject);
        _edgePS.Stop();
    }

    public void ActivateStartingNode()
    {
        starts[0].ConvertToCurrentNode();
    }

}
