using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class ServerHandler : MonoBehaviour
{
    public enum ServerStates { Unvisited, Visited, Beaten, Current }
    public enum ServerTypes {Undefined, Type1, Type2, Type3 }

    //refs
    [SerializeField] SpriteRenderer _sr = null;
    [SerializeField] SpriteRenderer _iconSR = null;
    Collider2D _coll;

    //settings
    [SerializeField] float _fadeTime = 1f;

    //state
    [SerializeField] ServerStates _serverState = ServerStates.Unvisited;
    [SerializeField] ServerTypes _serverType = ServerTypes.Undefined;
    public ServerTypes ServerType => _serverType;
    public ServerStates ServerState => _serverState;
    [SerializeField] bool _isHome = false;

    Tween _srTween;
    Tween _iconTween;
    
    private void Awake()
    {
        Initialize();
    }

    public void Initialize()
    {
        _sr = GetComponent<SpriteRenderer>();
        _coll = GetComponent<Collider2D>();
    }

    public void SetupServer(ServerStates state, ServerTypes type, bool isHome)
    {
        _serverState = state;
        _serverType = type;
        _isHome = isHome;
        if (_isHome) gameObject.layer = 10;
        else gameObject.layer = 12;
        RenderServerIcon();
    }

    public void ModifyServerState(ServerStates newState)
    {
        _serverState = newState;
        RenderServerIcon();
    }


    public void AdjustRotation(Vector2 facingDir)
    {
        transform.up = facingDir;
        _iconSR.transform.up = Vector2.up;
    }

    public void ConvertToCurrentServer()
    {
        ModifyServerState(ServerStates.Current);
        gameObject.layer = 10;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (PlayerController.Instance.CurrentPlayer.CurrentServer == this) return;

        PacketHandler ph;
        if (!collision.gameObject.TryGetComponent<PacketHandler>(out ph))
        {
            Debug.LogWarning("triggered collision with a non-packet!");
            return;
        }
        else
        {
            Debug.Log("triggered collision with packet");
            if (true) //Use this IF to check if player is allowed to enter this server (ie, locked, military, secret?)
            {
                ph.DeactivatePacket();
                PlayerController.Instance.CurrentPlayer.AdjustCurrentServer(this);

            }


        }
    }

    #region Server Visuals

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

    public void HideServer()
    {
        //fade away
        _srTween.Kill();
        _iconTween.Kill();

        _srTween = _sr.DOFade(0, _fadeTime);
        _iconTween = _iconSR.DOFade(0, _fadeTime);

        _coll.enabled = false;

    }

    public void ShowServer()
    {
        _srTween.Kill();
        _iconTween.Kill();

        _srTween = _sr.DOFade(1, _fadeTime);
        _iconTween = _iconSR.DOFade(1, _fadeTime);
        _coll.enabled = true;
    }

    #endregion
}
