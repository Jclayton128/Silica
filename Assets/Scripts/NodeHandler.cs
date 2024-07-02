using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NodeHandler : MonoBehaviour
{
    public enum NodeStates {Current, Available, Used}

    public enum NodeTypes {Empty, Blaster, Shotgun, Heal}

    //references
    SpriteRenderer _sr;

    //state
    [SerializeField] private int _ownerIndex = 0;
    PlayerHandler _playerHandler;
    private bool _isInitialized = false;
    public NodeStates NodeState;// { get; private set; }
    public NodeTypes NodeType;// { get; private set; }

    private void Initialize()
    {
        _sr = GetComponent<SpriteRenderer>();
        _isInitialized = true;
    }

    public void ActivateNode(NodeStates nodeState, NodeTypes nodeType, int ownerIndex)
    {
        gameObject.SetActive(true);

        if (!_isInitialized)
        {
            Initialize();
        }

        if (nodeState == NodeStates.Current)
        {
            ConvertToCurrentNode(ownerIndex);
        }
        else if (nodeState == NodeStates.Available)
        {
            _ownerIndex = 0;
            _sr.sprite = NodeLibrary.Instance.GetAvailableNodeSprite();
            _sr.color = ColorLibrary.Instance.UsedColor;
        }

        NodeType = nodeType;
        switch (nodeType)
        {
            //setup node icons
        }

        //Setup other nuances for this node later in this method
    }

    public void ConvertToCurrentNode(int ownerIndex)
    {
        _ownerIndex = ownerIndex;
        NodeState = NodeStates.Current;
        _sr.sprite = NodeLibrary.Instance.GetCurrentNodeSprite();
        _sr.color = ColorLibrary.Instance.PlayerColors[_ownerIndex -1];

        //tell the owningplayer that he now has a new current node
        PlayerHandler player = PlayerController.Instance.GetPlayer(_ownerIndex);
        player.AdjustCurrentNode(this);
    }

    public void ConvertToUsedNode()
    {
        NodeState = NodeStates.Used;
        _sr.sprite = NodeLibrary.Instance.GetUsedNodeSprite();
        AdjustRotation(Vector2.up);
        _sr.color = ColorLibrary.Instance.UsedColor;

        _ownerIndex = -1;
        NodeController.Instance.RemoveNodeFromAvailableNodeList(this);
    }

    public void DeactivateNode()
    {

        NodeController.Instance.DespawnNode(this);

        gameObject.SetActive(false);
    }

    public void AdjustRotation(Vector2 facingDir)
    {
        transform.up = facingDir;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        PacketHandler ph;
        if (!collision.TryGetComponent<PacketHandler>(out ph))
        {
            Debug.LogWarning("triggered collision with a non-packet!");
            return;
        }
        else
        {
            if (_ownerIndex > 0)
            {
                Debug.LogWarning("An owned node can't be captured!");
                return;
            }
            else if (NodeController.Instance.CheckIfNodeIsAvailable(this))
            {
                //capture this node and make it the current node of the owning player
                _ownerIndex = ph.OwnerIndex;

                if (_ownerIndex > 0)
                {
                    ConvertToCurrentNode(_ownerIndex);
                }

                ph.DeactivatePacket();
            }


        }       
    }
}
