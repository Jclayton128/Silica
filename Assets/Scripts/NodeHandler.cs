using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NodeHandler : MonoBehaviour, IDestroyable
{
    public enum NodeStates {Current, Available, Used}

    public enum NodeTypes {Empty, Blaster, Shotgun, Heal,    Count}

    //references
    SpriteRenderer _sr;
    [SerializeField] SpriteRenderer _sr_Icon = null;
    [SerializeField] Transform _icon = null;

    //state
    [SerializeField] private int _ownerIndex = 0;
    PlayerHandler _playerHandler;
    public bool HoldsPlayer => _playerHandler != null;
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
            _sr.color = ColorLibrary.Instance.AvailableColor;
        }

        NodeType = nodeType;
        switch (nodeType)
        {
            case NodeTypes.Blaster:
                _sr_Icon.sprite = NodeLibrary.Instance.IconBlaster;
                break;

            case NodeTypes.Shotgun:
                _sr_Icon.sprite = NodeLibrary.Instance.IconShotgun;
                break;

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
        _playerHandler = PlayerController.Instance.GetPlayer(_ownerIndex);
        _playerHandler.AdjustCurrentNode(this);

        NodeController.Instance.RemoveNodeFromAvailableNodeList(this);
    }

    public void ConvertToUsedNode()
    {
        NodeState = NodeStates.Used;
        _sr.sprite = NodeLibrary.Instance.GetUsedNodeSprite();
        AdjustRotation(Vector2.up);
        _sr.color = ColorLibrary.Instance.UsedColor;

        _playerHandler = null;
        _ownerIndex = -1;

    }

    public void DeactivateNode()
    {

        NodeController.Instance.DespawnNode(this);

        gameObject.SetActive(false);
    }

    public void AdjustRotation(Vector2 facingDir)
    {
        transform.up = facingDir;
        _icon.up = Vector2.up;
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



    public void HandleZeroHealth()
    {
        //TODO have a cool node destruction sequence
        DeactivateNode();
        if (_playerHandler) _playerHandler.HandleCurrentNodeDestruction();
    }

    public void HandleHealthDrop(float factorRemaining)
    {
        float h;
        float s;
        float v;
        Color.RGBToHSV(_sr.color, out h, out s, out v);

        v = Mathf.Lerp(0.1f, 1f, factorRemaining); //TODO remove magic numbers.    
        //v = factorRemaining;
 

        _sr.color = Color.HSVToRGB(h, s, v);
    }
}
