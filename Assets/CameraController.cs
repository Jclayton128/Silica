using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using System;
using DG.Tweening;

public class CameraController : MonoBehaviour
{
    public Action ZoomingOut;
    public Action ZoomingIn;
    public Action ZoomedMicro;

    public static CameraController Instance { get; private set; }
    //CinemachineVirtualCamera _cvc;
    Camera _cam;

    //settings
    [SerializeField] float _posChangeTime = 1f;
    [SerializeField] float _zoomTime = 0.5f;
    [SerializeField] float _zoomScaleIn = 6;
    [SerializeField] float _zoomScaleOut = 12;
    [SerializeField] float _zoomScaleMicro = 3;
    [SerializeField] float _cameraZOffset = -10;

    //state
    Tween _zoomTween;
    Tween _posTween;
    float _currentZoom;
    bool _isZooming = false;
    Vector3 _zoomInPos;
    Vector3 _zoomOutPos;
    bool _isZoomingOut = false;
    public bool IsZoomedIn => Mathf.Abs(_cam.orthographicSize - _zoomScaleIn) < Mathf.Epsilon;

    private void Awake()
    {
        Instance = this;
        _zoomOutPos = new Vector3(0, 0, _cameraZOffset);
    }
    private void Start()
    {
        //_cvc = Camera.main.GetComponentInChildren<CinemachineVirtualCamera>();
        //this.gameObject.transform.position = _pos;
        //_cvc.Follow = this.transform;

        _cam = Camera.main;

        GameController.Instance.RunStarted += HandleRunStarted;
        _currentZoom = _zoomScaleIn;
    }

    private void HandleRunStarted()
    {
        PlayerController.Instance.CurrentPlayer.PlayerTransformChanged += HandleUpdatedCurrentNodes;
    }


    private void HandleUpdatedCurrentNodes(Transform newTransform)
    {
        _zoomInPos = newTransform.position;
        _zoomInPos.z = _cameraZOffset;

        if (!_isZoomingOut)
        {
            _posTween.Kill();
            _posTween = _cam.transform.DOMove(_zoomInPos, _posChangeTime);
        }

    }

    public void ZoomOut()
    {
        _zoomTween.Kill();

        _zoomTween = _cam.DOOrthoSize(_zoomScaleOut, _zoomTime);
        _isZoomingOut = true;

        _posTween.Kill();
        if (PlayerController.Instance.CurrentPlayer.CurrentNode)
        {
            _zoomOutPos = new Vector3(0, 0, _cameraZOffset);
        }
        else
        {
            _zoomOutPos = PlayerController.Instance.CurrentPlayer.CurrentTransform.position;
            _zoomOutPos.z = _cameraZOffset;
        }
        _posTween = _cam.transform.DOMove(_zoomOutPos, _posChangeTime);

        ZoomingOut?.Invoke();
    }

    public void ZoomIn()
    {
        _zoomTween.Kill();

        _zoomTween = _cam.DOOrthoSize(_zoomScaleIn, _zoomTime);
        _isZoomingOut = false;

        _posTween.Kill();
        _posTween = _cam.transform.DOMove(_zoomInPos, _posChangeTime);

        ZoomingIn?.Invoke();
    }

    public void ZoomMicro()
    {
        _zoomTween.Kill();

        _zoomTween = _cam.DOOrthoSize(_zoomScaleMicro, _zoomTime).OnComplete(HandleZoomMicroCompleted);
        _isZoomingOut = false;

        //_posTween.Kill();
        //_posTween = _cam.transform.DOMove(_zoomInPos, _posChangeTime);


    }

    private void HandleZoomMicroCompleted()
    {
        ZoomedMicro?.Invoke();

        _zoomTween.Kill();
        _cam.orthographicSize = _zoomScaleIn;

    }

}
