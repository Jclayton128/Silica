using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arena : object
{
    public Arena(float radius, Vector2 startVector, Color startColor)
    {
        _radius = radius;
        _startVector = startVector;
        _startColor = startColor;
    }

    public float Radius => _radius;

    private float _radius;

    public Vector2 StartVector => _startVector;
    private Vector2 _startVector;

    public Color StartColor => _startColor;
    private Color _startColor;


}
