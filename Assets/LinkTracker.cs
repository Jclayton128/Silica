using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class LinkTracker : MonoBehaviour
{
    //refs
    PlayerHandler _player;

    //settings
    [SerializeField] LinkHandler _linkPrefab;

    //state
    [SerializeField] List<NodeHandler> _nodesInChain = new List<NodeHandler>();
    [SerializeField] List<LinkHandler> _activeLinks = new List<LinkHandler>();


    private void Awake()
    {

    }

    private void Start()
    {
        _player = GetComponent<PlayerHandler>();
        _player.CurrentNodeChanged += HandleCurrentNodeChanged;
    }

    private void HandleCurrentNodeChanged(NodeHandler newNode)
    {
        _nodesInChain.Add(newNode);
        if (_nodesInChain.Count > 1)
        {
            NodeHandler newestNode = _nodesInChain[_nodesInChain.Count - 1];
            NodeHandler secondNewestNode = _nodesInChain[_nodesInChain.Count -2];

            LinkHandler newLink = Instantiate(_linkPrefab);
            newLink.Initialize();
            newLink.Setup(secondNewestNode, newestNode);
            _activeLinks.Add(newLink);
        }
        PlayerController.Instance.CurrentArena.ArenaShuttingDown += HandleArenaShutdown;
    }

    private void HandleArenaShutdown()
    {
        for (int i = _activeLinks.Count-1; i >=0; i--)
        {
            _activeLinks[i].CloseDown();
        }

        _activeLinks.Clear();
        _nodesInChain.Clear();
        PlayerController.Instance.CurrentArena.ArenaShuttingDown -= HandleArenaShutdown;
    }
}
