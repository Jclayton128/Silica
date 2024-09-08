using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArenaHandler : MonoBehaviour
{
    //scene refs
    [SerializeField] ParticleSystem _edgePS = null;

    //state
    ParticleSystem.MainModule _edgePSmain;
    ParticleSystem.ShapeModule _edgePSshape;
    private void Start()
    {
        if (_edgePS) 
        {
            _edgePSmain = _edgePS.main;
            _edgePSshape = _edgePS.shape;
        }

        _edgePS.Pause(true);
    }

    public void SetupArena(Arena arena)
    {
        _edgePSmain.startColor = arena.StartColor;
        _edgePSshape.radius = arena.Radius;
        _edgePS.Play(true);
    }

}
