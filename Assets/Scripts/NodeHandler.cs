using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;  

public class NodeHandler : MonoBehaviour, IDestroyable
{
    public enum NodeStates {Current, Available, Used}

    public enum NodeTypes {Empty, Speed, Might, Intelligence, Constitution, Wisdom,
        Mainframe, Starting, Count}

    //references
    SpriteRenderer _sr;
    [SerializeField] SpriteRenderer _sr_Icon = null;
    [SerializeField] Transform _icon = null;
    [SerializeField] RingSpinner _ring0 = null;
    [SerializeField] RingSpinner _ring1 = null;
    [SerializeField] RingSpinner _ring2 = null;

    //settings
    [SerializeField] float _iconFadeTime = 0.5f;
    [SerializeField] FirewallSettings _firewallSettings = null;


    //state
    PlayerHandler _playerHandler;
    public bool HoldsPlayer => _playerHandler != null;
    private bool _isInitialized = false;
    public NodeStates NodeState => _currentNodeState;// { get; private set; }
    private NodeStates _currentNodeState;
    public NodeTypes NodeType => _currentNodeType;// { get; private set; }
    [SerializeField]  NodeTypes _currentNodeType;
    Tween _iconFade;

    private void Start()
    {
        Initialize();
        SetupRingSpinners(_firewallSettings);
        ActivateNode(NodeState, NodeType, 0);
    }

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
            ConvertToCurrentNode();
        }
        else if (nodeState == NodeStates.Available)
        {
            _sr.sprite = NodeLibrary.Instance.GetAvailableNodeSprite();
            _sr.color = ColorLibrary.Instance.AvailableColor;
        }

        _currentNodeType = nodeType;
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

    private void SetupRingSpinners(FirewallSettings firewallSettings)
    {
        _ring0.SetupRing(firewallSettings.ActiveSegments_0, firewallSettings.SpinRate_0);
        _ring1.SetupRing(firewallSettings.ActiveSegments_1, firewallSettings.SpinRate_1);
        _ring2.SetupRing(firewallSettings.ActiveSegments_2, firewallSettings.SpinRate_2);
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

    public void ConvertToCurrentNode()
    {
        _currentNodeState = NodeStates.Current;
        _sr.sprite = NodeLibrary.Instance.GetCurrentNodeSprite();
        _sr.color = ColorLibrary.Instance.PlayerColors[0];

        //tell the owningplayer that he now has a new current node
        _playerHandler = PlayerController.Instance.CurrentPlayer;
        _playerHandler.AdjustCurrentNode(this);

        NodeController.Instance.RemoveNodeFromAvailableNodeList(this);
    }

    public void ConvertToUsedNode()
    {
        _currentNodeState = NodeStates.Used;
        _sr.sprite = NodeLibrary.Instance.GetUsedNodeSprite();
        AdjustRotation(Vector2.up);
        _sr.color = ColorLibrary.Instance.UsedColor;

        _playerHandler = null;
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
            if (NodeType == NodeTypes.Mainframe)
            {
                //exit
                ServerController.Instance.ExitServerFromArena();
                PlayerController.Instance.CurrentPlayer.ReturnToCurrentServer();
                ph.DeactivatePacket();

            }
            else if (NodeController.Instance.CheckIfNodeIsAvailable(this))
            {
                ConvertToCurrentNode();
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
