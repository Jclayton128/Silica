using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;  

public class NodeHandler : MonoBehaviour, IDestroyable
{
    public enum NodeStates {Current, Available, Used}

    public enum NodeTypes {Empty, Speed, Might, Intelligence, Constitution, Wisdom, Mainframe,
        Count}

    //references
    SpriteRenderer _sr;
    [SerializeField] SpriteRenderer _sr_Icon = null;
    [SerializeField] Transform _icon = null;
    [SerializeField] RingSpinner _ring0 = null;
    [SerializeField] RingSpinner _ring1 = null;
    [SerializeField] RingSpinner _ring2 = null;

    //settings
    [SerializeField] float _iconFadeTime = 0.5f;


    //state
    [SerializeField] private int _ownerIndex = 0;
    PlayerHandler _playerHandler;
    public bool HoldsPlayer => _playerHandler != null;
    private bool _isInitialized = false;
    public NodeStates NodeState;// { get; private set; }
    public NodeTypes NodeType;// { get; private set; }
    Tween _iconFade;


    private void Initialize()
    {
        _sr = GetComponent<SpriteRenderer>();
        _isInitialized = true;

        CameraController.Instance.ZoomingIn += HandleZoomingIn;
        CameraController.Instance.ZoomingOut += HandleZoomingOut;
        
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
            case NodeTypes.Speed:
                _sr_Icon.sprite = NodeLibrary.Instance.IconSpeed;
                break;

            case NodeTypes.Might:
                _sr_Icon.sprite = NodeLibrary.Instance.IconMight;
                break;

            case NodeTypes.Wisdom:
                _sr_Icon.sprite = NodeLibrary.Instance.IconWisdom;
                break;

            case NodeTypes.Constitution:
                _sr_Icon.sprite = NodeLibrary.Instance.IconConstitution;
                break;

            case NodeTypes.Intelligence:
                _sr_Icon.sprite = NodeLibrary.Instance.IconIntelligence;
                break;

            case NodeTypes.Mainframe:
                _sr_Icon.sprite = NodeLibrary.Instance.IconMainframe;
                break;
        }

        _iconFade.Kill();
        if (CameraController.Instance.IsZoomedIn)
        {
            _iconFade = _sr_Icon.DOFade(1, 0.01f);
        }
        else
        {
            _iconFade = _sr_Icon.DOFade(0, 0.01f);
        }



        SetupRingSpinners();
        //Setup other nuances for this node later in this method
    }

    private void SetupRingSpinners()
    {
        if (NodeType == NodeTypes.Mainframe)
        {
            _ring0.SetupRing(true, 20);
            _ring1.SetupRing(false, 20);
            _ring2.SetupRing(true, -90);
        }
        else
        {
            _ring0.SetupRing(false, 0);
            _ring1.SetupRing(false, 0);
            _ring2.SetupRing(false, 0);
        }

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
            else if (NodeType == NodeTypes.Mainframe)
            {
                //exit
                ServerController.Instance.ExitServerFromArena();
                PlayerController.Instance.CurrentPlayer.ReturnToCurrentServer();
                ph.DeactivatePacket();

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

    private void HandleZoomingIn()
    {
        _iconFade.Kill();
        _iconFade = _sr_Icon.DOFade(1, _iconFadeTime);
    }

    private void HandleZoomingOut()
    {
        _iconFade.Kill();
        _iconFade = _sr_Icon.DOFade(0, _iconFadeTime*4);
    }
}
