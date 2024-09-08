using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArenaController : MonoBehaviour
{
    public static ArenaController Instance { get; private set; }

    public float XSpan => _xSpan;
    public int XSpanAsInts => Mathf.RoundToInt(_xSpan);
    [SerializeField] float _xSpan = 4;


   

    private void Awake()
    {
        Instance = this;
    }
}
