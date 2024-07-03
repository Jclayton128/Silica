using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArenaController : MonoBehaviour
{
    public static ArenaController Instance { get; private set; }

    [SerializeField] float[] _xStarts = { -3f, -1f, 1f, 3f };
    public float XSpan => _xSpan;
    [SerializeField] float _xSpan = 4;
    [SerializeField] float _yMin = 2;
    [SerializeField] float _yMax = 5;
    [SerializeField] float _yOffscreenOffset = 10;

    private void Awake()
    {
        Instance = this;
    }
}
