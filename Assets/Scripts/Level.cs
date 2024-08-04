using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="New Level")]
public class Level : ScriptableObject
{
    [SerializeField] List<NodeHandler.NodeTypes> _possibleNodes = new List<NodeHandler.NodeTypes>();
    public List<NodeHandler.NodeTypes> PossibleNodes => _possibleNodes;

    [SerializeField] float _nodeDensity = 0.5f;
    public float NodeDensity => _nodeDensity;

    [SerializeField] float _minDistanceBetweenNodes = 2f;
    public float MinDistanceBetweenNodes => _minDistanceBetweenNodes;

}
