using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ServerHandler : MonoBehaviour
{
    public enum ServerStates { Unvisited, Visited, Beaten, Current }
    public enum ServerTypes {Undefined, Type1, Type2, Type3 }

    //refs
    [SerializeField] SpriteRenderer _sr = null;
    [SerializeField] SpriteRenderer _iconSR = null;

    //state
    [SerializeField] ServerStates _serverState = ServerStates.Unvisited;
    [SerializeField] ServerTypes _serverType = ServerTypes.Undefined;
    public ServerTypes ServerType => _serverType;
    public ServerStates ServerState => _serverState;
    [SerializeField] bool _isHome = false;
    
    private void Start()
    {
        Initialize();
    }

    public void Initialize()
    {
        _sr = GetComponent<SpriteRenderer>();

    }

    public void SetupServer(ServerStates state, ServerTypes type, bool isHome)
    {
        _serverState = state;
        _serverType = type;
        _isHome = isHome;

        RenderServerIcon();
    }

    public void ModifyServerState(ServerStates newState)
    {
        _serverState = newState;
        RenderServerIcon();
    }

    private void RenderServerIcon()
    {
        if (_isHome && _serverState != ServerStates.Current)
        {
            _sr.sprite = ServerLibrary.Instance.HomeServerIcon;
        }
        else
        {
            switch (_serverState)
            {
                case ServerStates.Unvisited:
                    _sr.sprite = ServerLibrary.Instance.UnvisitedIcon;
                    break;

                case ServerStates.Visited:
                    _sr.sprite = ServerLibrary.Instance.VisitedIcon;
                    break;

                case ServerStates.Beaten:
                    _sr.sprite = ServerLibrary.Instance.BeatenIcon;
                    break;

                case ServerStates.Current:
                    _sr.sprite = ServerLibrary.Instance.CurrentIcon;
                    break;
            }
        }
        transform.up = Vector3.up;
    }

    public void AdjustRotation(Vector2 facingDir)
    {
        transform.up = facingDir;
        _iconSR.transform.up = Vector2.up;
    }

    public void ConvertToCurrentServer()
    {
        ModifyServerState(ServerStates.Current);

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (PlayerController.Instance.CurrentPlayer.CurrentServer == this) return;

        PacketHandler ph;
        if (!collision.TryGetComponent<PacketHandler>(out ph))
        {
            Debug.LogWarning("triggered collision with a non-packet!");
            return;
        }
        else
        {
            Debug.Log("triggered collision with packet");
            if (true) //Use this IF to check if player is allowed to enter this server (ie, locked, military, secret?)
            {
                PlayerController.Instance.CurrentPlayer.AdjustCurrentServer(this);
                ph.DeactivatePacket();
            }


        }
    }
}
