using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RingSpinner : MonoBehaviour
{
    [SerializeField] List<SpriteRenderer> _segments = null;

    //settings
    

    [SerializeField] float _radius = 2f;

    //state
    
    Vector2 _pos = Vector2.zero;
    float _spinRate = 0;


    private void Awake()
    {
        InitializeSegmentPositions();
    }
    private void InitializeSegmentPositions()
    {
        int count = _segments.Count;
        float spread = 360f / (float)count;
        float rads;

        for (int i = 0; i < count; i++)
        {
            rads = i * spread * Mathf.Deg2Rad;
            //Debug.Log($"Rads: {rads}");
            _pos.x = Mathf.Cos(rads) * _radius;
            _pos.y = Mathf.Sin(rads) * _radius;
            _segments[i].transform.localPosition = _pos;
            _segments[i].transform.localEulerAngles =
                new Vector3(0, 0, -90 + (rads * Mathf.Rad2Deg));
        }
    }

    public void SetupRing(bool isActive, float spinRate)
    {
        if (!isActive)
        {
            gameObject.SetActive(false);
            return;
        }

        _spinRate = spinRate;
    }



    private void Update()
    {
        transform.Rotate(transform.forward, _spinRate * Time.deltaTime);
    }
}
