using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Firewall Settings")]
public class FirewallSettings : ScriptableObject
{
    [SerializeField] float _spinRate_0 = 0; //8
    [SerializeField] bool[] _activeSegments_0 =
        { false, false, false, false,false,false,false, false };

    [SerializeField] float _spinRate_1 = 0; //12
    [SerializeField] bool[] _activeSegments_1 =
        { false, false, false, false, false, false, false, false,
        false, false, false, false };

    [SerializeField] float _spinRate_2 = 0; //16
    [SerializeField] bool[] _activeSegments_2 =
        { false, false, false, false, false, false, false, false,
        false, false, false, false,
        false, false, false, false};

    public float SpinRate_0 => _spinRate_0;
    public bool[] ActiveSegments_0 => _activeSegments_0;
    public float SpinRate_1 => _spinRate_1;
    public bool[] ActiveSegments_1 => _activeSegments_1;
    public float SpinRate_2 => _spinRate_2;
    public bool[] ActiveSegments_2 => _activeSegments_2;
}
