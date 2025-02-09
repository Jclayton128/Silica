using System;
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
    [SerializeField] List<LinkHandler> _activeLinks = new List<LinkHandler>();


    private void Awake()
    {

    }

    private void Start()
    {
        _player = GetComponent<PlayerHandler>();
        _player.CurrentNodeChanged += HandleCurrentNodeChanged;
        _player.RevertedToPreviousNode += HandleRevertedToPreviousNode;
    }



    private void HandleCurrentNodeChanged(NodeHandler newNode)
    {
        var list = PlayerController.Instance.CurrentPlayer.NodesInTrail;
        if (list.Count > 1)
        {
            NodeHandler newestNode = list[list.Count - 1];
            NodeHandler secondNewestNode = list[list.Count - 2];

            LinkHandler newLink = Instantiate(_linkPrefab);
            newLink.Initialize();
            newLink.Setup(secondNewestNode, newestNode);
            _activeLinks.Add(newLink);
        }
        PlayerController.Instance.CurrentArena.ArenaShuttingDown += HandleArenaShutdown;
    }

    private void HandleRevertedToPreviousNode()
    {
        DestroyLastLink();
    }

    private void DestroyLastLink()
    {
        Debug.Log("removing last link");
        LinkHandler lastLink = _activeLinks[_activeLinks.Count - 1];
        _activeLinks.Remove(lastLink);
        lastLink.CloseDown();
    }

    private void HandleArenaShutdown()
    {
        for (int i = _activeLinks.Count-1; i >=0; i--)
        {
            _activeLinks[i].CloseDown();
        }

        _activeLinks.Clear();
        PlayerController.Instance.CurrentArena.ArenaShuttingDown -= HandleArenaShutdown;
    }
}
