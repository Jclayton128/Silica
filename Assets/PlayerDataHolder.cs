using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDataHolder : MonoBehaviour
{
    [SerializeField] float _startingSpeed = 3;


    public float CurrentSpeed => _currentSpeed;
    private float _currentSpeed;

    // Start is called before the first frame update
    void Start()
    {
        _currentSpeed = _startingSpeed;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
