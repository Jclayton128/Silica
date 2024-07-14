using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class LimiterController : MonoBehaviour
{

    public static LimiterController Instance { get; private set; }

    public Action LimiterSpeedChanged;

    [SerializeField] LimiterHandler _limiterPrefab = null;
    [SerializeField] GameObject _bugAbsorbFXPrefab = null;

    //param
    [SerializeField] float _limiterStartSpeed = 1f;


    //state
    LimiterHandler _limiter;
    public float LimiterSpeedCurrent => _limiterSpeed_current;
    float _limiterSpeed_current;


    private void Awake()
    {
         Instance = this;
        _limiterSpeed_current = _limiterStartSpeed;
    }

    public void SpawnLimiter()
    {
        if (_limiter) return;

        Vector2 pos = new Vector2(0, NodeController.Instance.YOffScreen_Down);
        _limiter = Instantiate(_limiterPrefab,
            pos, Quaternion.identity);
        _limiter.ActivateLimiter();
        _limiter.SetSpeed(_limiterSpeed_current);
    }

    public void HandleBugAbsorb(Vector2 location, float speedToAdd)
    {
        AdjustLimiterSpeed(speedToAdd);
        Instantiate(_bugAbsorbFXPrefab, location, Quaternion.identity);
    }

    private void AdjustLimiterSpeed(float speedToAdd)
    {
        _limiterSpeed_current += speedToAdd;
        _limiter.SetSpeed(_limiterSpeed_current);
        LimiterSpeedChanged?.Invoke();
    }

   
}
