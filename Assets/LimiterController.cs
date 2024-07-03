using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LimiterController : MonoBehaviour
{
    public static LimiterController Instance { get; private set; }

    [SerializeField] LimiterHandler _limiterPrefab = null;

    //param
    [SerializeField] float _limiterStartSpeed = 1f;


    //state
    LimiterHandler _currentLimiter;


    private void Awake()
    {
         Instance = this;
    }

    public void SpawnLimiter()
    {
        if (_currentLimiter) return;

        Vector2 pos = new Vector2(0, NodeController.Instance.YOffScreen_Down);
        _currentLimiter = Instantiate(_limiterPrefab,
            pos, Quaternion.identity);
        _currentLimiter.ActivateLimiter();
        _currentLimiter.AdjustSpeed(_limiterStartSpeed);
    }
}
