using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Bug Path")]
public class BugPath : ScriptableObject
{
    [SerializeField] List<Vector2> _waypoints;
    public List<Vector2> Waypoints => _waypoints;

    [Tooltip("Maximum signed x offset for this path to spawn.")]
    [SerializeField] float _maxX = 0;
    public float MaxX => _maxX;
}
