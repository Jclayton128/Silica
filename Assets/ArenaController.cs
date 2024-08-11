using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArenaController : MonoBehaviour
{
    public static ArenaController Instance { get; private set; }

    public float XSpan => _xSpan;
    public int XSpanAsInts => Mathf.RoundToInt(_xSpan);
    [SerializeField] float _xSpan = 4;


    //[SerializeField] float _yMin = 2;
    //[SerializeField] float _yMax = 5;
    [SerializeField] float _yOffscreenOffset = 10;
    public float YOffScreen_Up => _yOffscreenOffset + NodeController.Instance.CurrentNodesCentroid;
    public float YOffScreen_Down => -_yOffscreenOffset + NodeController.Instance.CurrentNodesCentroid;

    private void Awake()
    {
        Instance = this;
    }
}
