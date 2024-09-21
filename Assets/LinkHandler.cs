using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class LinkHandler : MonoBehaviour
{
    //This controls the mechanics and visuals of a single link
    //Links have a starting node and ending node

    //ref
    ParticleSystem _ps; 
    ParticleSystem.MainModule _main;
    ParticleSystem.EmissionModule _emis;
    LineRenderer _renderer;
    NodeHandler _startNode;
    NodeHandler _endNode;

    //settings
    [SerializeField] float _fadeTime = 2f;
    [SerializeField] float _particleDensity = 100f;

    //state
    Tween _colorTween;
    Color2 _color;

    private void Awake()
    {
        Initialize();
    }

    public void Initialize()
    {
        _ps = GetComponent<ParticleSystem>();
        _main = _ps.main;
        _emis = _ps.emission;

        _renderer = GetComponentInParent<LineRenderer>();
        _color = new Color2 (_renderer.startColor, _renderer.endColor);
    }

    public void Setup(NodeHandler startNode, NodeHandler endNode)
    {
        _startNode = startNode;
        _endNode = endNode;

        Vector2 start = _startNode.transform.position;
        Vector2 end = _endNode.transform.position;

        Vector2 mid = ( start + end) / 2f;
        Vector2 dir = (end - start);
        transform.position = mid;
        transform.right = dir;

        _emis.rateOverTime = _particleDensity * dir.magnitude;

        _renderer.SetPosition(0, start);
        _renderer.SetPosition(1, end);
        _renderer.startColor = Color.clear;
        _renderer.endColor = Color.clear;
        _colorTween = _renderer.DOColor(new Color2(Color.clear, Color.clear),
            _color, _fadeTime).SetEase(Ease.InBounce);

        var shape = _ps.shape;
        shape.radius = dir.magnitude / 2f;
    }

    public void CloseDown()
    {
        _colorTween.Kill();
        Destroy(gameObject);
    }

}
