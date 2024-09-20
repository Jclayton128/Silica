using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ICEHandler : MonoBehaviour
{
    public enum IceTypes { Undefined, DriftAndDestroy, Tracer, SentryTurret }

    //refs
    protected ParticleSystem _ps;
    protected ArenaHandler _arena;

    //state
    [SerializeField] IceTypes _iceType = IceTypes.Undefined;
    public IceTypes IceType => _iceType;

    protected virtual void Awake()
    {
        _ps = GetComponent<ParticleSystem>();

    }

    protected virtual void Start()
    {
        _arena = GetComponentInParent<ArenaHandler>();
    }

    
    #region Helpers

    #endregion
}
