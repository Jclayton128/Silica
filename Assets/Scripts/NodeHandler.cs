using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NodeHandler : MonoBehaviour
{
    public enum NodeStates {Current, Available, Used}

    //references
    SpriteRenderer _sr;

    //state
    private bool _isInitialized = false;
    public NodeStates NodeState { get; private set; }

    private void Initialize()
    {
        _sr = GetComponent<SpriteRenderer>();
        _isInitialized = true;
    }

    public void ActivateNode(NodeStates nodeState)
    {
        gameObject.SetActive(true);

        if (!_isInitialized)
        {
            Initialize();
        }

        if (nodeState == NodeStates.Current)
        {
            _sr.sprite = NodeLibrary.Instance.GetCurrentNodeSprite();
        }
        else if (nodeState == NodeStates.Available)
        {
            _sr.sprite = NodeLibrary.Instance.GetAvailableNodeSprite();
        }

        //Setup other nuances for this node later in this method
    }

    public void ConvertToCurrentNode()
    {
        NodeState = NodeStates.Current;
        _sr.sprite = NodeLibrary.Instance.GetCurrentNodeSprite();

        NodeController.Instance.RemoveNodeFromAvailableNodeList(this);
    }

    public void ConvertToUsedNode()
    {
        NodeState = NodeStates.Used;
        _sr.sprite = NodeLibrary.Instance.GetUsedNodeSprite();
        AdjustRotation(Vector2.up);

        NodeController.Instance.RemoveNodeFromAvailableNodeList(this);
    }

    public void DeactivateNode()
    {
        Debug.Log("deactivating node");
        NodeController.Instance.DespawnNode(this);

        gameObject.SetActive(false);
    }

    public void AdjustRotation(Vector2 facingDir)
    {
        transform.up = facingDir;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (NodeController.Instance.CurrentNode == this)
        {
            //Debug.Log("self collision");
            //Destroy(collision.transform.gameObject);
        }
        else if (NodeController.Instance.CheckIfNodeIsAvailable(this))
        {
            //transfer active node to this one
            NodeController.Instance.AdjustCurrentNode(this);
            PacketHandler ph;
            if (collision.TryGetComponent<PacketHandler>(out ph))
            {
                ph.DeactivatePacket();
            }
        }
    }

}
