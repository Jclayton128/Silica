using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class SegmentHandler : MonoBehaviour, IDestroyable
{
    [SerializeField] float _timeBetweenHeal = 10f;
    [SerializeField] Color _damageColorFlash = Color.red;
    [SerializeField] float _timeToFlash = 0.5f;

    //state 
    float _timeToHeal = Mathf.Infinity;
    SpriteRenderer _sr;
    Tween _colorTween;
    Color _startingColor;
    Collider2D _coll;
    HealthHandler _health;
    
    public bool IsAlive => _sr.enabled == true;

    private void Awake()
    {
        _sr = GetComponent<SpriteRenderer>();
        _startingColor = _sr.color;

        _health = GetComponent<HealthHandler>();
        _coll = GetComponent<Collider2D>();
    }


    public void HandleHealthDrop(float factorRemaining)
    {
        //Flash

        _colorTween.Kill();
        _sr.color = _damageColorFlash;
        _colorTween = _sr.DOColor(_startingColor, _timeToFlash);
    }

    public void HandleZeroHealth()
    {
        //disappear
        _sr.enabled = false;
        _coll.enabled = false;
        _timeToHeal = Time.time + _timeBetweenHeal;
    }

    private void Update()
    {
        if (IsAlive) return;

        if (Time.time >= _timeToHeal)
        {
            _coll.enabled = true;
            _sr.enabled = true;
            _health.ResetHealth();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        PacketHandler ph;
        if (!collision.TryGetComponent<PacketHandler>(out ph))
        {
            Debug.LogWarning("triggered collision with a non-packet!");
            return;
        }
        ph.DeactivatePacket();
    }

}
