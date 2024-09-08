using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArenaController : MonoBehaviour
{
    public static ArenaController Instance { get; private set; }

    //scene refs
    [SerializeField] ArenaHandler _arenaHandler = null;

    //settings
    [Tooltip("Factor of how far along radius from centroid can a node be")]
    [SerializeField] float _maxEdgeFactor = 0.9f;

    [Tooltip("Radius of arena...")]
    [SerializeField] float _arenaRadius = 6;

    //state
    private Arena _currentArena;   
   

    private void Awake()
    {
        Instance = this;
    }


    public void CreateNewCurrentArena()
    {
        Arena newArena = new Arena(_arenaRadius,
            UnityEngine.Random.insideUnitCircle.normalized,
            UnityEngine.Random.ColorHSV(0,1, 0.2f, 1f, 0.2f, 1f));
        _currentArena = newArena;
        _arenaHandler.SetupArena(newArena);   
    }

    public Vector2 GetNewPlayerPosition()
    {
        Vector2 pos = _currentArena.StartVector * _currentArena.Radius * _maxEdgeFactor;
        return pos;
    }
}
